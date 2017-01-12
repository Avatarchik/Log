using UnityEngine;
using System.Collections;
using StageEnum;

public class StageUIRoot : SequenceController{
    public static StageUIRoot Instance;

    [HideInInspector]
    public UIGroupInfo kGroupInfo;
    [HideInInspector]
    public UIStateBoard kStateBoard;
    [HideInInspector]
    public UIPausePopup kPausePopup;
    
    [HideInInspector]
    public UIControlMenu kContorlMenu;

    [HideInInspector]
    public Camera kCamera;
    Camera mStageCamera;

    Transform mInstanceText;
    
    void Awake()
    {
        Instance = this;

        kCamera = transform.Find("Camera").GetComponent<Camera>();

        kGroupInfo = transform.Find("GroupInfo").GetComponentInChildren<UIGroupInfo>(true);
        kStateBoard = kCamera.transform.Find("TopAnchor/StateBoard").GetComponentInChildren<UIStateBoard>(true);        
        kPausePopup = kCamera.transform.Find("CenterAnchor/PausePopup").GetComponentInChildren<UIPausePopup>(true);        
        kContorlMenu = kCamera.transform.Find("BottomAnchor/ControlMenu").GetComponentInChildren<UIControlMenu>(true);

        mInstanceText = transform.Find("InstanceText");
                
        kPausePopup.gameObject.SetActive(false);
    }

    public override void OnPrepare()
    {
        mStageCamera = StageManager.Instance.kStageCamera.GetComponent<Camera>();
        kStateBoard.OnPrepare();
    }

    // Use this for initialization
    public override void OnStart () {        
        
    }

    // Update is called once per frame
    public override void OnUpdate () {
        
    }

    public void SetMenu(Mode _selectMode)
    {   
        switch (_selectMode)
        {
            case Mode.Battle:
                kStateBoard.gameObject.SetActive(true);
                break;
        }        
    }

    public void TypoMessage(Vector3 _pos, string _msg, UITypoText.Type _typo = UITypoText.Type.Damage)
    {
        Vector3 screenPos = mStageCamera.WorldToScreenPoint(_pos);
        Vector3 pos = kCamera.ScreenToWorldPoint(screenPos);

        GameObject damageLabel = ObjectPoolManager.Instance.GetGameObejct(StrDef.UI_TYPOTEXT, mInstanceText);
        damageLabel.transform.position = pos;
        damageLabel.transform.localScale = Vector3.one;
        damageLabel.transform.localRotation = Quaternion.identity;
        damageLabel.layer = transform.gameObject.layer;
        damageLabel.GetComponent<UITypoText>().Play(_msg, _typo);
    }

    public void StageClear()
    {
        kGroupInfo.Clear();
    }
}
