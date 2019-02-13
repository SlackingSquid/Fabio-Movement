using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    [HideInInspector] public bool takingMoveInput = true;
    [HideInInspector] public bool takingJumpInput = true;

    [HideInInspector] public Rigidbody RB;
    public GameObject cam;
    public float walkSpeed = 5f;
    public float turnSpeed = 100f;
    [HideInInspector] public float currentTurnSpeed = 100f;
    bool enableAirControl = true;
    public float airControl = 20f;
    public float airControlMaxSpeed = 5f;
    [HideInInspector] public float moveInputMagnitude;
    [HideInInspector] public Vector3 moveDir;

    // new jump stuff
    public float jumpGravMul = 4f;
    public float fallGravMul = 4f;
    public float holdJumpGravMul = 2f;
    float jumpheightGravMul = 0f;
    float currentJumpGravMul = 2f;
    public float jumpForce = 15f;
    public float rollJumpForce = 8f;
    float currentJumpForce = 15f;

    [HideInInspector] public float jumpVel = -1f;
    [HideInInspector] public bool hasJumped = false;
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool leftground = false;

    

    Vector3 vel;
    [HideInInspector] public Vector3 lVel;

    public float maxMoveFriction = 0.2f;
    float externalFriction = 0.2f;
    float currentFriction = 0.2f;

    public GameObject playerCollision;
    public GameObject rollCollision;
    public float rollDuration = 0.5f;
    public float rollSpeed = 8f;
    public float rollCooldown = 0.5f;
    public float rollJumpTimeWindow = 0.1f;
    bool canRollJump = false;
    bool canRoll = true;
    [HideInInspector] public bool rolling = false;

    Vector3 groundVector;
    Vector3 slopeVector;
    public float slideSpeed = 8f;
    public float slideControl = 4f;
    public float slideAngle = 30f;
    [HideInInspector] public bool sliding = false;

    bool isMounted = false;
    Mount currentMount;

    [HideInInspector] public bool isChargeShooting = false;
    public float chargeShootingTurnSpeed = 100f;

    public PlayerAnimations playerAnim;

    bool canAirAttack = true;

    bool checkForSliding = true;
    [HideInInspector] public bool afterSlide = false;
    float afterSlideTime = 0.2f;
    float afterSlideCounter = 0f;

    public GameObject anchorPoint;
    public GameObject anchorPointFollow;

    Coroutine rollCoroutine;

    // Use this for initialization
    void Start() {

        RB = GetComponent<Rigidbody>();
        moveDir = Vector3.forward;
        //Jump();
        currentFriction = maxMoveFriction;
        currentTurnSpeed = turnSpeed;
    }

    // Update is called once per frame
    void Update() {

        GetInput();

        if (takingMoveInput)
        {
            if (sliding || afterSlide)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(vel.x, 0f, vel.z)), Time.deltaTime * currentTurnSpeed);
            }
            else
            {
                if (moveInputMagnitude > 0.3)//rotating the character
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * currentTurnSpeed);
            }

            if (isChargeShooting)
            {
                if (isGrounded)
                    vel = Vector3.zero;
            }
            else
            {
                if (isGrounded)
                {
                    if (rolling)
                    {
                        vel = moveDir * rollSpeed;
                        jumpVel = RB.velocity.y;
                    }

                    else if (sliding)
                    {
                        Vector3 vec = Vector3.Cross(slopeVector, Vector3.up).normalized;
                        //vel = (moveDir * moveInputMagnitude * slideControl * Mathf.Abs(Vector3.Dot(moveDir, vec))) + (slopeVector * slideSpeed);
                        vel = (vec * Vector3.Dot(moveDir, vec) * moveInputMagnitude * slideControl) + (slopeVector * slideSpeed);
                        jumpVel = RB.velocity.y + (slopeVector.y * slideSpeed);
                        //jumpVel = RB.velocity.y;
                    }
                    else if (afterSlide)
                    {
                        vel = RB.velocity;
                        jumpVel = (slopeVector.y * slideSpeed);
                    }
                    else
                    {
                        if (moveInputMagnitude > 0.6)
                        {
                            vel = moveDir * moveInputMagnitude * walkSpeed;
                        }
                        else
                        {
                            vel = Vector3.zero - (slopeVector * 1.6f);
                        }
                        jumpVel = RB.velocity.y;
                    }
                }
                else
                {
                    if (enableAirControl && (RB.velocity.magnitude < airControlMaxSpeed || Vector3.Dot(moveDir, RB.velocity.normalized) < 0.5f))
                    {
                        RB.AddForce(moveDir.x * airControl * moveInputMagnitude, 0f, moveDir.z * airControl * moveInputMagnitude);
                    }
                    vel = RB.velocity;
                    jumpVel = RB.velocity.y;
                }
            }
        }



        lVel = Vector3.Lerp(RB.velocity, vel, currentFriction);

        if (currentFriction < maxMoveFriction)
            currentFriction = Mathf.Lerp(currentFriction, maxMoveFriction, Time.deltaTime * 1f);

        


        if (Input.GetButtonDown("Jump"))//|| rolling))  // OPTION
        {
            if(isGrounded && !hasJumped && takingJumpInput || isMounted)
            {
                if(canRollJump)
                {
                    EndRoll();
                }
                Jump();
                //hasJumped = false;
                
            }
        }

        
        if (canRoll && Input.GetButtonDown("Roll") && isGrounded && takingMoveInput && !sliding)
        {
            ExecuteRoll();
        }


        if (isGrounded)
        {
            if (leftground) // happenes on landing
            {
                GameManager.Instance.cameraShake.Shake(0.1f, 0.1f, 0.4f);
                hasJumped = false;
                canAirAttack = true;
            }
            else
            {
                hasJumped = false;
            }
            leftground = false;
        }
        else
        {
            if (!leftground && hasJumped)
            {
                leftground = true;
            }
            else if (!leftground && !hasJumped)
            {
                leftground = true;
            }
            SetJumpVel();

            if (Physics.Raycast((transform.position + (transform.forward * 0.2f)) + (transform.up * 0.5f), transform.forward, 0.5f))
            {
                enableAirControl = false;
            }
            else
            {
                enableAirControl = true;
            }
        }

        if(Input.GetButtonDown("Fire1") && !rolling && !sliding &&!isMounted)
        {
            if(!isGrounded && canAirAttack)
            {
                if (RB.velocity.y < 0f)
                {
                    jumpVel = 0f;
                    RB.AddForce(Vector3.up * 10f * (-RB.velocity.y*0.03f), ForceMode.Impulse);
                }
                canAirAttack = false;
                playerAnim.Attack();
            }
            else if(isGrounded)
            {
                playerAnim.Attack();
            }
        }

        



        RaycastHit hit;
        if (!isMounted)
        {
            if (Physics.Raycast((transform.position + (transform.forward * 0.2f)) + (transform.up * 0.1f), Vector3.down, out hit, 0.25f) ||
                Physics.Raycast((transform.position + (transform.forward * -0.2f)) + (transform.up * 0.1f), Vector3.down, out hit, 0.25f) ||
                Physics.Raycast((transform.position + (transform.right * 0.2f)) + (transform.up * 0.1f), Vector3.down, out hit, 0.25f) ||
                Physics.Raycast((transform.position + (transform.right * -0.2f)) + (transform.up * 0.1f), Vector3.down, out hit, 0.25f) ||
                Physics.Raycast(transform.position + (transform.up * 0.1f), Vector3.down, out hit, 0.25f))
            {
                isGrounded = true;

                if (hit.collider.gameObject.isStatic == false) // perant to not static objects / maybe switch to the two point parant system from cyberlight // done
                {
                    //transform.parent = hit.collider.transform;
                    if (anchorPoint.transform.parent == null)// || hit.collider.transform != anchorPoint.transform.parent.transform)
                    {
                        //transform.position = hit.point;
                        anchorPoint.transform.position = hit.point;
                        anchorPoint.transform.parent = hit.collider.transform; 
                        transform.parent = anchorPointFollow.transform;
                    }
                    //transform.position = anchorPointFollow.transform.position;
                    //transform.parent = anchorPointFollow.transform;
                }
                else
                {
                    transform.parent = null;
                    anchorPoint.transform.parent = null;
                    anchorPoint.transform.position = transform.position;
                }

                groundVector = hit.normal;
                slopeVector = Vector3.Cross(-Vector3.Cross(groundVector, Vector3.up), groundVector);

                if (hit.collider.tag == "SlipperySlope" && checkForSliding)//slideAngle <= (Vector3.Angle(Vector3.up, slopeVector) - 90f) && checkForSliding)
                {
                    sliding = true;
                    afterSlide = true;
                    afterSlideCounter = 0f;
                }
                else
                {
                    if(afterSlide)
                    {
                        if(afterSlideCounter > afterSlideTime)
                        {
                            afterSlide = false;
                            afterSlideCounter = 0f;
                        }
                        afterSlideCounter += Time.deltaTime;
                    }
                    
                    sliding = false;
                }
                //Debug.Log(Vector3.Angle(Vector3.up, slopeVector)-90f);
                //Debug.DrawLine(hit.point, hit.point + slopeVector,Color.red);
            }
            else
            {
                afterSlide = false;
                isGrounded = false;
                transform.parent = null;
                anchorPoint.transform.parent = null;
                anchorPoint.transform.position = transform.position;
                sliding = false;
            }
        }

        //Debug.Log(isGrounded);


    }

    private void FixedUpdate()
    {
        RB.velocity = new Vector3(lVel.x, jumpVel, lVel.z);
    }

    void SetJumpVel()
    {
        if (Input.GetButton("Jump"))
        {
            currentJumpGravMul = holdJumpGravMul;
        }
        else
        {
            currentJumpGravMul = jumpGravMul;
        }

        float fallMul = Mathf.Lerp(fallGravMul, 1f, jumpheightGravMul);
        float riseMul = Mathf.Lerp(currentJumpGravMul, 1f, jumpheightGravMul);
        if (RB.velocity.y <= 0f)
        {
            jumpVel +=  (Physics.gravity.y * fallMul * Time.deltaTime);
        }
        else
        {
            jumpVel += (Physics.gravity.y * riseMul * Time.deltaTime);
        }

    }

    void GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        Vector3 newX = cam.transform.right * x;

        float y = Input.GetAxis("Vertical");
        Vector3 newY = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized * y;

        Vector3 inputVector = newX + newY;

        moveInputMagnitude = Mathf.Clamp01(inputVector.magnitude);
        if (moveInputMagnitude > 0.15)
        {
            moveDir = inputVector.normalized;
        }
    }

    public void Jump()
    {
        if (isMounted)
            DismountPlayer();
        transform.parent = null;
        hasJumped = true;
        RB.velocity = new Vector3(RB.velocity.x, 0f, RB.velocity.z);
        jumpVel = 0f;

        if (sliding)
        {
            StartCoroutine(TurnOffSlideCheckForSeconds());
            //jumpheightGravMul = RB.velocity.magnitude / slideSpeed;
            //currentJumpForce = Mathf.Lerp(rollJumpForce, jumpForce, jumpheightGravMul);
            jumpheightGravMul = 0.5f;
            currentJumpForce = rollJumpForce;
            lVel = RB.velocity;
        }
        else
        {
            jumpheightGravMul = RB.velocity.magnitude / rollSpeed;
            currentJumpForce = Mathf.Lerp(jumpForce, rollJumpForce, jumpheightGravMul);
        }
        //Debug.Log(currentJumpForce);
        RB.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
        //jumpVel = currentJumpForce;
    }

    public void ApplyForce (Vector3 Force,float friction)
    {
        RB.velocity = Force;
        currentFriction = friction;
    }

    void ExecuteRoll()
    {
        takingMoveInput = false;
        takingJumpInput = false;
        playerCollision.SetActive(false);
        rollCollision.SetActive(true);
        //vel = moveDir * rollSpeed;
        vel = transform.forward * rollSpeed;
        rolling = true;
        rollCoroutine = StartCoroutine(ExecuteRollEnding());
    }
    IEnumerator ExecuteRollEnding()
    {
        yield return new WaitForSeconds(rollDuration - rollJumpTimeWindow);
        canRollJump = true;
        takingJumpInput = true;
        yield return new WaitForSeconds(rollJumpTimeWindow);
        if (rolling)
            EndRoll();
    }
    void EndRoll()
    {
        if (CheckForRoof())
        {
            ExecuteRoll();
            return;
        }
        takingMoveInput = true;
        takingJumpInput = true;
        playerCollision.SetActive(true);
        rollCollision.SetActive(false);
        rolling = false;
        currentFriction = 0.1f;
        StartCoroutine(RollCooldownCounter());
        canRollJump = false;
    }
    private bool CheckForRoof()
    {
        if ((Physics.Raycast(transform.position + (transform.up * 0.8f) + (transform.forward*0.0f), Vector3.up, 1f) || 
            Physics.Raycast(transform.position + (transform.up * 0.8f) + (transform.forward * 0.2f), Vector3.up, 1f) ||
            Physics.Raycast(transform.position + (transform.up * 0.8f) + (transform.forward * -0.2f), Vector3.up, 1f) ||
            Physics.Raycast(transform.position + (transform.up * 0.8f) + (transform.right * 0.2f), Vector3.up, 1f) ||
            Physics.Raycast(transform.position + (transform.up * 0.8f) + (transform.right * -0.2f), Vector3.up, 1f)) 
            && rolling)
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    IEnumerator RollCooldownCounter()
    {
        canRoll = false;
        yield return new WaitForSeconds(rollDuration);
        canRoll = true;
    }

    

    public void MountPlayer(Mount mount)
    {
        if(transform.parent == null)
        {
            EndRoll();
            currentMount = mount;
            transform.parent = mount.transform;
            transform.position = mount.transform.position;
            transform.rotation = mount.transform.rotation;
            takingMoveInput = false;
            RB.isKinematic = true;
            isMounted = true;
            playerCollision.SetActive(false);
            rollCollision.SetActive(false);
            currentMount.playerIsOn = true;
            currentMount.mountTrigger.SetActive(false);
        }
    }
    void DismountPlayer()
    {
        RB.isKinematic = false;
        lVel = currentMount.RB.velocity;
        currentMount.PlayerDismounting();
        transform.parent = null;
        transform.position += Vector3.up;
        takingMoveInput = true;
        isMounted = false;
        playerCollision.SetActive(true);
        transform.up = Vector3.up;
    }

    public void EnterChargeShootingMode()
    {
        isChargeShooting = true;
        takingJumpInput = false;
        //currentTurnSpeed = chargeShootingTurnSpeed;
    }
    public void ExitChargeShootingMode()
    {
        isChargeShooting = false;
        takingJumpInput = true;
        currentTurnSpeed = turnSpeed;
        StartCoroutine(PauseMoveInputForSeconds(0.2f));
        moveInputMagnitude = 0f;
    }

    IEnumerator PauseMoveInputForSeconds(float time)
    {
        takingMoveInput = false;
        yield return new WaitForSeconds(time);
        takingMoveInput = true;
    }

    IEnumerator TurnOffSlideCheckForSeconds()
    {
        checkForSliding = false;
        afterSlide = false;
        afterSlideCounter = 0f;
        yield return new WaitForSeconds(0.2f);
        checkForSliding = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "UpWindZone" && !isGrounded)
        {
            //Debug.Log(other.gameObject.tag);
            // GameManager.Instance.player.RB.AddForce(transform.up * force);
            /*if(GameManager.Instance.player.RB.velocity.y < 2f)
            {
                GameManager.Instance.player.RB.velocity += Vector3.up * force;
            }*/
            //GameManager.Instance.player.RB.velocity += Vector3.up*force;
            if (Input.GetButton("Jump"))
            {
                if (GameManager.Instance.player.RB.velocity.y < 10f)
                    GameManager.Instance.player.RB.velocity += Vector3.up * 1.5f;
            }
            else
            {
                if (GameManager.Instance.player.RB.velocity.y < -4f)
                    GameManager.Instance.player.RB.velocity += Vector3.up * 0.8f;
                else
                    GameManager.Instance.player.RB.velocity += Vector3.up * 0.4f;
            }
        }

    }
}
