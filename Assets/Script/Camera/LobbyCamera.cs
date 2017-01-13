using UnityEngine;
using System.Collections;

public class LobbyCamera : MonoBehaviour
{ 
    Vector3 mViewPos;
    float mBetweenDistCenter;
    
    BezierCurve[] mMoveCurve = new BezierCurve[4];
    BezierCurve[] mViewCurve = new BezierCurve[4];
    int mCameraCurveIndex = 0;

    public bool kIsCinemaView = true;

    void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            mMoveCurve[i] = GameObject.Find("CinemaPath/Move" + i).GetComponent<BezierCurve>();
            mViewCurve[i] = GameObject.Find("CinemaPath/View" + i).GetComponent<BezierCurve>();
        }
    }

    void OnEnable()
    {
        if (kIsCinemaView == true)
        {
            mCameraCurveIndex = Random.Range(0, 4);
            StartCoroutine(CameraView(30.0f));
        }
    }    

    IEnumerator CameraView(float _duration)
    {
        float curTime = 0.0f;        
        BezierCurve curveMove = mMoveCurve[mCameraCurveIndex];
        BezierCurve curveView = mViewCurve[mCameraCurveIndex];

        while (curTime <= _duration)
        {
            gameObject.GetComponent<Camera>().enabled = true;

            curTime += Time.deltaTime;

            if (curTime > _duration)
                curTime = _duration;

            transform.position = curveMove.GetPointAt(curTime / _duration);
            transform.LookAt(curveView.GetPointAt(curTime / _duration));

            if (curTime == _duration)
            {
                mCameraCurveIndex++;
                if (mCameraCurveIndex >= 4)
                    mCameraCurveIndex = 0;

                curveMove = mMoveCurve[mCameraCurveIndex];
                curveView = mViewCurve[mCameraCurveIndex];
                curTime = 0.0f;
            }

            yield return null;
        }

        yield break;
    } 

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }    
}
