using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public AnimationCurve shakePosCurve;
    public AnimationCurve shakeRotCurve;
    public float shakeDuration = 1f;
    public float shakeAmp = 1f;
    public float shakeRotAmp = 1f;
    float counter = 1f;
    Vector3 randPosMul;
    Vector3 randRotMul;
    float[] val = new float[] { 1f, -1f };

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(counter <= 1f)
        {
            transform.localPosition = new Vector3(shakePosCurve.Evaluate(counter) * randPosMul.x * shakeAmp, shakePosCurve.Evaluate(counter) * randPosMul.y * shakeAmp, 0f);// shakePosCurve.Evaluate(counter) * randPosMul.z * shakeAmp);
            transform.localEulerAngles = new Vector3(shakeRotCurve.Evaluate(counter) * randRotMul.x * shakeRotAmp, shakeRotCurve.Evaluate(counter) * randRotMul.y * shakeRotAmp, shakeRotCurve.Evaluate(counter) * randRotMul.z * shakeRotAmp);
            counter += Time.deltaTime / shakeDuration;
        }


        //if(Input.GetKeyDown(KeyCode.S))
        //{
        //    Shake();
        //}
		
	}
    public void Shake(float amp, float rotAmp, float duration)
    {
        counter = 0f;
        randPosMul = new Vector3(val[Random.Range(0, 1)], val[Random.Range(0, 1)], val[Random.Range(0, 1)]);
        randRotMul = new Vector3(val[Random.Range(0, 1)], val[Random.Range(0, 1)], val[Random.Range(0, 1)]);

        shakeDuration = duration;
        shakeAmp = amp;
        shakeRotAmp = rotAmp;
    }
}
