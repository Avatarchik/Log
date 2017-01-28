using UnityEngine;
using System.Collections;

public class WorldCamera : MonoBehaviour
{
    public static WorldCamera Instance = null;

    public enum Mode
    {
        Center,
        Side
    }
    
    [HideInInspector]
    public Mode kCurrentMode = Mode.Center;
    
    float mCurDistance = 0;
    float mTotalDistance = 2.0f;

    //줌 인, 아웃 시 카메라 이동 제한
    bool mIsOnPinch = false;
    [HideInInspector]
    public bool kIsSwipe = false;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        EasyTouch.On_Swipe += OnSwipe;
        EasyTouch.On_PinchIn += OnPinchIn;
        EasyTouch.On_PinchOut += OnPinchOut;
        EasyTouch.On_PinchEnd += OnPinchEnd;

        Vector3 pos = new Vector3();
        pos.x = PlayerPrefs.GetFloat("WorldCameraPositionX", transform.position.x);
        pos.y = PlayerPrefs.GetFloat("WorldCameraPositionY", transform.position.y);
        pos.z = PlayerPrefs.GetFloat("WorldCameraPositionZ", transform.position.z);        
        transform.position = pos;
        mCurDistance = PlayerPrefs.GetFloat("WorldCameraDist", 0);

        LobbyUIRoot.Instance.kWorldMap.Zoom(mCurDistance / mTotalDistance);
    }

    void OnDisable()
    {
        EasyTouch.On_Swipe -= OnSwipe;
        EasyTouch.On_PinchIn -= OnPinchIn;
        EasyTouch.On_PinchOut -= OnPinchOut;
        EasyTouch.On_PinchEnd -= OnPinchEnd;
        
        PlayerPrefs.SetFloat("WorldCameraPositionX", transform.position.x);
        PlayerPrefs.SetFloat("WorldCameraPositionY", transform.position.y);
        PlayerPrefs.SetFloat("WorldCameraPositionZ", transform.position.z);
        PlayerPrefs.SetFloat("WorldCameraDist", mCurDistance);
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
        if (mIsOnPinch == true)
            return;

        Vector3 pos = transform.localPosition;
        pos.x -= gesture.deltaPosition.x;
        pos.y -= gesture.deltaPosition.y;
        transform.localPosition = pos;
        
        if (LobbyUIRoot.Instance.kCurSelectMenu != LobbyEnum.MenuSelect.WorldMap)
        {
            kIsSwipe = true;
            LobbyUIRoot.Instance.SetMenu(LobbyEnum.MenuSelect.WorldMap);
        }
    }
    
    void OnPinchIn(Gesture gesture)
    {
        mIsOnPinch = true;
        float zoom = Time.unscaledDeltaTime * gesture.deltaPinch * 0.5f;
        
        if (mCurDistance + zoom > mTotalDistance)
        {
            zoom = mTotalDistance - mCurDistance;
            mCurDistance = mTotalDistance;
        }
        else
        {
            mCurDistance += zoom;
        }

        transform.position += -transform.forward * zoom;

        LobbyUIRoot.Instance.kWorldMap.Zoom(mCurDistance / mTotalDistance);
    }

    void OnPinchOut(Gesture gesture)
    {
        mIsOnPinch = true;
        float zoom = Time.unscaledDeltaTime * gesture.deltaPinch * 0.5f;
        if (mCurDistance - zoom < 0)
        {
            zoom = mCurDistance;
            mCurDistance = 0;
        }
        else
        {
            mCurDistance -= zoom;
        }

        transform.position += transform.forward * zoom;

        LobbyUIRoot.Instance.kWorldMap.Zoom(mCurDistance / mTotalDistance);
    }

    void OnPinchEnd(Gesture gesture)
    {
        mIsOnPinch = false;
    }

    public void ZoneFocus(Zone _zone)
    {
        if (_zone == null)
            return;

        gameObject.SetActive(true);
        Vector3 pos = _zone.transform.position;
        pos += new Vector3(0.5f, 1.0f, -0.3f);
        StartCoroutine(Translate(pos));

        kCurrentMode = Mode.Side;
        mCurDistance = 0.0f;
        LobbyUIRoot.Instance.kWorldMap.Zoom(0.0f);
    }

    public void CenterView()
    {
        if (kCurrentMode == Mode.Center)
            return;

        gameObject.SetActive(true);
        Vector3 pos = transform.position;
        pos += new Vector3(-0.5f, 0.0f, 0.0f);
        StartCoroutine(Translate(pos));

        kCurrentMode = Mode.Center;
    }

    public void SideView()
    {
        if (kCurrentMode == Mode.Side)
            return;

        gameObject.SetActive(true);
        Vector3 pos = transform.position;
        pos += new Vector3(0.5f, 0.0f, 0.0f);
        StartCoroutine(Translate(pos));

        kCurrentMode = Mode.Side;
    }

    IEnumerator Translate(Vector3 _pos)
    {
        while(transform.position != _pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _pos, 7.5f * Time.deltaTime);

            yield return null;
        }

        yield break;
    }
}
