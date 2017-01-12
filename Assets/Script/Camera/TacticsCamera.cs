using UnityEngine;
using System.Collections;

public class TacticsCamera : MonoBehaviour
{ 
    Vector3 mViewPos;
    float mBetweenDistCenter;
    
    //줌 인, 아웃 시 카메라 이동 제한
    bool mIsOnPinch = false;
        
    void Awake()
    {   
    }
    
    void OnEnable()
    {        
        EasyTouch.On_Swipe += OnSwipe;
    }

    void OnDisable()
    {        
        EasyTouch.On_Swipe -= OnSwipe;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void OnSwipe(Gesture gesture)
    {
        /*
        if (mIsOnPinch == true)
            return;
        
        Vector3 angles = transform.eulerAngles;
        angles.z = 0.0f;
        transform.eulerAngles = angles;

        Vector3 backPos1 = transform.eulerAngles;
        Vector3 backPos2 = transform.position;

        transform.RotateAround(mViewPos, transform.up, gesture.deltaPosition.x * Time.unscaledDeltaTime * 10.0f);
        transform.RotateAround(mViewPos, transform.right, -gesture.deltaPosition.y * Time.unscaledDeltaTime * 10.0f);
        
        //*Z축 회전에 의한 카메라 뒤집힘 방지
        float dotValue = Vector3.Dot((transform.position - mViewPos).normalized, Vector3.up);
        if (dotValue > 0.95f || dotValue < -0.95f)
        {
            transform.eulerAngles = backPos1;
            transform.position = backPos2;
        }*/
    }    
}
