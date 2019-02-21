using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public GameObject cameraHolder;
    public Camera gameCamera;
    public float HorRotSpeed = 10f;
    public float VerRotSpeed = 10f;
    float verRotMul;
    public bool invertY = true;
    float invY = 1f;
    public float cameraMaxDistance = 15f;
    Vector3 cameraDestination;
    float cameraDistanceFromPlayer = 0f;
    float xRotFixer = 0f;
    Coroutine setCamDir;
    // Use this for initialization
    void Start () {

        cameraDestination = transform.position - (transform.forward * cameraMaxDistance);

    }
	
	// Update is called once per frame
	void Update () {

        if(invertY)
            invY = 1f;
        else
            invY = -1f;

        if(Input.GetButtonDown("Submit"))
        {
            invertY = !invertY;
        }

        if(transform.localEulerAngles.x > 300f && transform.localEulerAngles.x < 360f)
            xRotFixer = transform.localEulerAngles.x - 360f;
        else
            xRotFixer = transform.localEulerAngles.x;
        
        transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("R_Horizontal")*Time.deltaTime*HorRotSpeed);
        
        if((invY * Input.GetAxis("R_Vertical") > 0f && xRotFixer < 70f) || (invY * Input.GetAxis("R_Vertical") < 0f && xRotFixer > -40f))
                transform.RotateAround(transform.position, transform.right, invY * Input.GetAxis("R_Vertical") * Time.deltaTime * VerRotSpeed);


        if(Input.GetButtonDown("R_Joystick_Button"))
        {
            if(setCamDir == null)
                setCamDir = StartCoroutine(SetCameraDirection((player.transform.forward - (Vector3.up*0.6f)).normalized, 250f,1f));
            else
            {
                StopCoroutine(setCamDir);
                setCamDir = StartCoroutine(SetCameraDirection((player.transform.forward - (Vector3.up * 0.6f)).normalized, 250f,1f));
            }
        }
       // KeepCameraFacingDirection((player.transform.forward - Vector3.up).normalized, 100f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), -transform.forward, out hit, cameraMaxDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Solid"))//hit.collider.gameObject.isStatic == true)
            {
                cameraDestination = hit.point + (transform.forward * 0.1f) + (hit.normal * 0.5f);
            }
            else
            {
                cameraDestination = transform.position - (transform.forward * cameraMaxDistance);
            }
        }
        else
        {
            cameraDestination = transform.position - (transform.forward * cameraMaxDistance);
        }

        cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, cameraDestination, Time.deltaTime * 10f);

        cameraDistanceFromPlayer = Vector3.Distance(player.transform.position, cameraHolder.transform.position);

        gameCamera.fieldOfView = Mathf.Lerp(80f, 60f, cameraDistanceFromPlayer / cameraMaxDistance);
    }
    void FixedUpdate()
    {

        transform.position = Vector3.Lerp(transform.position + (Vector3.up*0.5f), player.transform.position, Time.deltaTime * 10f);

    }

    public IEnumerator SetCameraDirection(Vector3 direction, float speed, float safetyTime)
    {
        float safeCounter = safetyTime;
        while (Vector3.Dot(transform.forward,direction) < 0.999f && safeCounter > 0f)
        {
            float smoothy = Mathf.Clamp01(Mathf.Abs(transform.eulerAngles.y - Quaternion.LookRotation(direction).eulerAngles.y)*0.02f);
            if (Vector3.Dot(transform.right, direction) > 0f)
                transform.eulerAngles += new Vector3(0f, 1f * Time.deltaTime * speed * smoothy, 0f);
            else
                transform.eulerAngles += new Vector3(0f, -1f * Time.deltaTime * speed * smoothy, 0f);

            float smoothx = Mathf.Clamp01( Mathf.Abs(transform.eulerAngles.x - Quaternion.LookRotation(direction).eulerAngles.x)*0.02f);
            if (Vector3.Dot(transform.up, direction) > 0f)
                transform.eulerAngles += new Vector3(-1f * Time.deltaTime * speed * smoothx, 0f, 0f);
            else
                transform.eulerAngles += new Vector3( 1f * Time.deltaTime * speed * smoothx, 0f, 0f);
            safeCounter -= Time.deltaTime;

            if(Vector3.Dot(transform.up,Vector3.up) < 0.2f)
            {
                ResetFuckedUpCamera();
            }
            yield return null;
        }
    }

    public void ResetFuckedUpCamera()
    {
        transform.forward = (player.transform.forward - (Vector3.up * 0.6f)).normalized;
    }

    public void KeepCameraFacingDirection(Vector3 direction, float speed)
    {
        if (Vector3.Dot(transform.forward, direction) < 0.999f)
        {
            float smoothy = Mathf.Clamp01(Mathf.Abs(transform.eulerAngles.y - Quaternion.LookRotation(direction).eulerAngles.y) * 0.02f);
            if (Vector3.Dot(transform.right, direction) > 0f)
                transform.eulerAngles += new Vector3(0f, 1f * Time.deltaTime * speed * smoothy, 0f);
            else
                transform.eulerAngles += new Vector3(0f, -1f * Time.deltaTime * speed * smoothy, 0f);

            float smoothx = Mathf.Clamp01(Mathf.Abs(transform.eulerAngles.x - Quaternion.LookRotation(direction).eulerAngles.x) * 0.02f);
            if (Vector3.Dot(transform.up, direction) > 0f)
                transform.eulerAngles += new Vector3(-1f * Time.deltaTime * speed * smoothx, 0f, 0f);
            else
                transform.eulerAngles += new Vector3(1f * Time.deltaTime * speed * smoothx, 0f, 0f);
        }
        if (Vector3.Dot(transform.up, Vector3.up) < 0.2f)
        {
            ResetFuckedUpCamera();
        }
    }
}
