using UnityEngine;
using System.Collections;

public class StageCamera : MonoBehaviour
{ 
    Vector3 mViewPos;
    float mBetweenDistCenter;
    
    float mMaxDistance = 0.0f;
    float mMinDistance = 0.0f;
    float mCurDistance = 0;
    
    //줌 인, 아웃 시 카메라 이동 제한
    bool mIsOnPinch = false;
        
    void Awake()
    {
        float dist = Vector3.Distance(mViewPos, transform.position);
        mMaxDistance = dist + EditDef.CAMERA_DISTANCE_MAX;
        mMinDistance = dist + EditDef.CAMERA_DISTANCE_MIN;
        mCurDistance = dist;
        mViewPos = new Vector3(0.0f, -10.0f, 0.0f);
    }

    void OnEnable()
    {
        EasyTouch.On_Swipe += OnSwipe;
        EasyTouch.On_PinchIn += OnPinchIn;
        EasyTouch.On_PinchOut += OnPinchOut;
        EasyTouch.On_PinchEnd += OnPinchEnd;
    }

    void OnDisable()
    {
        EasyTouch.On_Swipe -= OnSwipe;
        EasyTouch.On_PinchIn -= OnPinchIn;
        EasyTouch.On_PinchOut -= OnPinchOut;
        EasyTouch.On_PinchEnd -= OnPinchEnd;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (StagePlayManager.Instance.kEnemyShipList.Count == 0 ||
            StagePlayManager.Instance.kPlayerShipList.Count == 0)
            return;
        
        float oldPos = mViewPos.z;
        mViewPos.z = Mathf.Lerp(mViewPos.z, mBetweenDistCenter, Time.unscaledDeltaTime);
        float deltaPos = mViewPos.z - oldPos;
        
        transform.Translate(0.0f, 0.0f, deltaPos, Space.World);
        
        transform.rotation.SetLookRotation(mViewPos, transform.up);        
        transform.position = mViewPos + (transform.position - mViewPos).normalized * mCurDistance;
    }

    void GroupCenterMove()
    {
        if(StagePlayManager.Instance == null)        
            return;

        float leftMinDist = Mathf.Infinity;
        float rightMaxDist = Mathf.Infinity;
        for (int i = 0; i < StagePlayManager.Instance.kPlayerShipList.Count; i++)
        {
            Ship ship = StagePlayManager.Instance.kPlayerShipList[i];
            if (ship.kIsDie == true)
                continue;

            if (leftMinDist == Mathf.Infinity)
                leftMinDist = ship.transform.position.z;
            if (rightMaxDist == Mathf.Infinity)
                rightMaxDist = ship.transform.position.z;

            if (leftMinDist > ship.transform.position.z)
                leftMinDist = ship.transform.position.z;
            if (rightMaxDist < ship.transform.position.z)
                rightMaxDist = ship.transform.position.z;
        }

        for (int i = 0; i < StagePlayManager.Instance.kEnemyShipList.Count; i++)
        {
            Ship ship = StagePlayManager.Instance.kEnemyShipList[i];
            if (ship == null)
                continue;
            if (ship.kIsDie == true)
                continue;

            if (leftMinDist == Mathf.Infinity)
                leftMinDist = ship.transform.position.z;
            if (rightMaxDist == Mathf.Infinity)
                rightMaxDist = ship.transform.position.z;

            if (leftMinDist > ship.transform.position.z)
                leftMinDist = ship.transform.position.z;
            if (rightMaxDist < ship.transform.position.z)
                rightMaxDist = ship.transform.position.z;
        }

        mBetweenDistCenter = (leftMinDist + rightMaxDist) * 0.5f;
    }    

    void OnSwipe(Gesture gesture)
    {
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
        }
    }
    
    void OnPinchIn(Gesture gesture)
    {
        mIsOnPinch = true;
        float zoom = Time.unscaledDeltaTime * gesture.deltaPinch * 10.0f;
        mCurDistance += zoom;

        if (mCurDistance > mMaxDistance)
            mCurDistance =  mMaxDistance;
    }

    void OnPinchOut(Gesture gesture)
    {
        mIsOnPinch = true;
        float zoom = Time.unscaledDeltaTime * gesture.deltaPinch * 10.0f;
        mCurDistance -= zoom;

        if (mCurDistance < mMinDistance)
            mCurDistance = mMinDistance;
    }    

    void OnPinchEnd(Gesture gesture)
    {
        mIsOnPinch = false;
    }
}
