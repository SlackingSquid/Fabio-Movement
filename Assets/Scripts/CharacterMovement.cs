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
    public float airControl = 20f;
    public float airControlMaxSpeed = 5f;
    [HideInInspector] public float moveInputMagnitude;
    Vector3 moveDir;

    public AnimationCurve jumpCurve;
    public float jumpMinHeight = 1f;
    public float jumpMaxHeight = 1.5f;
    float jumpHeight = 1f;
    public float jumpTime = 1f;
    public float jumpHoldMul = 1.4f;
    float jumpVel = -1f;
    [HideInInspector] public float jumpCounter = 0f;
    [HideInInspector] public bool hasJumped = false;
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool leftground = false;

    // new jump stuff
    float jumpGravMul = 4f;
    float fallGravMul = 4f;
    float holdJumpGravMul = 2f;
    float jumpheightGravMul = 0f;
    float currentJumpGravMul = 2f;
    float jumpForce = 15f;
    float rollJumpForce = 8f;
    float currentJumpForce = 15f;

    Vector3 vel;
    Vector3 lVel;

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

    // Use this for initialization
    void Start() {

        RB = GetComponent<Rigidbody>();
        moveDir = Vector3.forward;
        jumpVel = jumpCurve.Evaluate(1f) * jumpMaxHeight;
        Jump();
        jumpCounter = 0f;
        currentFriction = maxMoveFriction;
        currentTurnSpeed = turnSpeed;
    }

    // Update is called once per frame
    void Update() {

        if (takingMoveInput)
        {
            GetInput();
        }



        lVel = Vector3.Lerp(RB.velocity, vel, currentFriction);

        if (currentFriction < maxMoveFriction)
            currentFriction = Mathf.Lerp(currentFriction, maxMoveFriction, Time.deltaTime * 1f);

        if (isChargeShooting)
        {
            if(isGrounded)
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
                    vel = (moveDir * moveInputMagnitude * slideControl * Mathf.Abs(Vector3.Dot(moveDir, vec))) + (slopeVector * slideSpeed);
                    //jumpVel = RB.velocity.y + (slopeVector.y * slideSpeed);
                    jumpVel = RB.velocity.y;
                }
                else
                {
                    if (moveInputMagnitude > 0.6)
                    {
                        vel = moveDir * moveInputMagnitude * walkSpeed;
                        //jumpVel = RB.velocity.y;
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
                if (RB.velocity.magnitude < airControlMaxSpeed || Vector3.Dot(moveDir, RB.velocity.normalized) < 0.5f)
                    RB.AddForce(moveDir.x * airControl * moveInputMagnitude, 0f, moveDir.z * airControl * moveInputMagnitude);
                vel = RB.velocity;
                jumpVel = RB.velocity.y;
            }
        }


        if (Input.GetButtonDown("Jump"))//|| rolling))  // OPTION
        {
            if(isGrounded && !hasJumped && takingJumpInput || isMounted)
            {
                if(canRollJump)
                {
                    EndRoll();
                }
                Jump();
                hasJumped = false;
                RB.velocity = new Vector3(RB.velocity.x, 0f, RB.velocity.z);
                //RB.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
                if (sliding)
                {
                    jumpheightGravMul = RB.velocity.magnitude / slideSpeed;
                    currentJumpForce = Mathf.Lerp( rollJumpForce, jumpForce, jumpheightGravMul);
                }
                else
                {
                    jumpheightGravMul = RB.velocity.magnitude / rollSpeed;
                    currentJumpForce = Mathf.Lerp(jumpForce, rollJumpForce, jumpheightGravMul);
                }
                RB.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            }
        }
        /*if (jumpCounter > 0)
        {
            if (Input.GetButton("Jump"))
            {
                //jumpCounter -= Time.deltaTime / (jumpTime * jumpHoldMul);
                currentJumpGravMul = holdJumpGravMul;
            }
            else
            {
                //jumpCounter -= Time.deltaTime / jumpTime;
                currentJumpGravMul = jumpGravMul;
            }
            jumpCounter -= Time.deltaTime / jumpTime;

           // jumpVel = jumpCurve.Evaluate(1f - jumpCounter) * jumpHeight;
        }*/
        //else
       // {
           // if (!sliding)
                //jumpVel = RB.velocity.y;
           // hasJumped = false;
        //}


        if (canRoll && Input.GetButtonDown("Roll") && isGrounded && takingMoveInput && !sliding)
        {
            StartCoroutine(ExecuteRoll());
        }


        if (isGrounded)
        {
            if (leftground) // happenes on landing
            {
               // if (jumpCounter < 0.2f)
                GameManager.Instance.cameraShake.Shake(0.2f, 0.2f, 0.6f);
                //jumpVel = 0f;
                jumpCounter = 0f;
                hasJumped = false;
                canAirAttack = true;
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
                jumpCounter = 0.5f;
                leftground = true;
            }
            SetJumpVelTest();
        }

        if(Input.GetButtonDown("Fire1") && !rolling)
        {
            //playerAnim.Attack();
            if(!isGrounded && canAirAttack)
            {
                //jumpCounter = 0.8f;
                //RB.velocity = Vector3.zero;
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

        if (moveInputMagnitude > 0.3)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * currentTurnSpeed);
        // transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * 10f);

        //Debug.Log(RB.velocity.magnitude);


        RaycastHit hit;
        if (!isMounted)
        {
            if (Physics.Raycast((transform.position + (transform.forward * 0.2f)) + (transform.up * 0.1f), Vector3.down, out hit, 0.25f) ||
                Physics.Raycast((transform.position + (transform.forward * -0.2f)) + (transform.up * 0.1f), Vector3.down, out hit, 0.25f) ||
                Physics.Raycast((transform.position + (transform.right * 0.2f)) + (transform.up * 0.1f), Vector3.down, out hit, 0.25f) ||
                Physics.Raycast((transform.position + (transform.right * -0.2f)) + (transform.up * 0.1f), Vector3.down, out hit, 0.25f))
            {
                isGrounded = true;

                if (hit.collider.gameObject.isStatic == false) // perant to not static objects / maybe switch to the two point parant system from cyberlight
                {
                    transform.parent = hit.collider.transform;
                }
                else
                {
                    transform.parent = null;
                }

                groundVector = hit.normal;
                slopeVector = Vector3.Cross(-Vector3.Cross(groundVector, Vector3.up), groundVector);
                if (slideAngle <= (Vector3.Angle(Vector3.up, slopeVector) - 90f))
                {
                    sliding = true;
                }
                else
                {
                    sliding = false;
                }
                //Debug.Log(Vector3.Angle(Vector3.up, slopeVector)-90f);
                //Debug.DrawLine(hit.point, hit.point + slopeVector,Color.red);
            }
            else
            {
                isGrounded = false;
                transform.parent = null;
                sliding = false;
            }
        }
    }

    private void FixedUpdate()
    {
        RB.velocity = new Vector3(lVel.x, jumpVel, lVel.z);
    }

    void SetJumpVelTest()
    {
        if (Input.GetButton("Jump"))
        {
            //jumpCounter -= Time.deltaTime / (jumpTime * jumpHoldMul);
            currentJumpGravMul = holdJumpGravMul;
        }
        else
        {
            //jumpCounter -= Time.deltaTime / jumpTime;
            currentJumpGravMul = jumpGravMul;
        }

        float fallMul = Mathf.Lerp(fallGravMul, 1f, jumpheightGravMul);
        float riseMul = Mathf.Lerp(currentJumpGravMul, 1f, jumpheightGravMul);
        if (RB.velocity.y < -0.4f)
        {
            jumpVel +=  (Physics.gravity.y * fallMul * Time.deltaTime);
        }
        if (RB.velocity.y > 0f)
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
        //jumpCounter = 1f;
        //jumpVel = 0f;
        transform.parent = null;
        hasJumped = true;
        //jumpHeight = Mathf.Lerp(jumpMaxHeight, jumpMinHeight, RB.velocity.magnitude / rollSpeed);
    }

    public void ApplyForce (Vector3 Force,float friction)
    {
        RB.velocity = Force;
        currentFriction = friction;
    }

    IEnumerator ExecuteRoll()
    {
        takingMoveInput = false;
        takingJumpInput = false;
        playerCollision.SetActive(false);
        rollCollision.SetActive(true);
        vel = moveDir * rollSpeed;
        rolling = true;
        yield return new WaitForSeconds(rollDuration-rollJumpTimeWindow);
        canRollJump = true;
        takingJumpInput = true;
        yield return new WaitForSeconds(rollJumpTimeWindow);
        if(rolling)
            EndRoll();
    }
    void EndRoll()
    {
        takingMoveInput = true;
        takingJumpInput = true;
        playerCollision.SetActive(true);
        rollCollision.SetActive(false);
        rolling = false;
        currentFriction = 0.1f;
        StartCoroutine(RollCooldownCounter());
        canRollJump = false;
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
            currentMount = mount;
            transform.parent = mount.transform;
            transform.position = mount.transform.position;
            transform.forward = mount.transform.forward;
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
}
