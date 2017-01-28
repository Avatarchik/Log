using UnityEngine;
using System.Collections;
using LobbyEnum;

public class CommonUIRoot : SequenceController
{
    public static CommonUIRoot Instance;
    Camera mCamera;

    [HideInInspector]
    public UITitle kTitle;
    [HideInInspector]
    public UILoading kLoading;
    [HideInInspector]
    public UIMessageBox kMessageBox;
    [HideInInspector]
    public UIOption kOption;

    void Awake()
    {
        Instance = this;

        mCamera = transform.Find("Camera").GetComponent<Camera>();
                
        kTitle      = mCamera.transform.Find("CenterAnchor/Title").GetComponentInChildren<UITitle>(true);
        kLoading    = mCamera.transform.Find("CenterAnchor/Loading").GetComponentInChildren<UILoading>(true);
        kLoading    = mCamera.transform.Find("CenterAnchor/Loading").GetComponentInChildren<UILoading>(true);        
        kOption     = mCamera.transform.Find("CenterAnchor/Option").GetComponentInChildren<UIOption>(true);
        kMessageBox = mCamera.transform.Find("CenterAnchor/MessageBox").GetComponentInChildren<UIMessageBox>(true);

        kOption.gameObject.SetActive(false);
        kTitle.gameObject.SetActive(CommonManager.Title);
        kLoading.gameObject.SetActive(CommonManager.Title);

        kMessageBox.gameObject.SetActive(false);

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    public override void OnStart () {        
    }

    // Update is called once per frame
    public override void OnUpdate() {
	
	}

    public void SetLoading()
    {
        kTitle.gameObject.SetActive(false);
        kLoading.gameObject.SetActive(true);
        kLoading.SetPercent(0);
    }

    public void SetTouchCamera()
    {
        EasyTouch.instance.nGUICameras.Add(mCamera);
    }
}
