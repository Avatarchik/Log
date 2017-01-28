using UnityEngine;
using System.Collections;
using CommonEnum;

public class UIWorldMap : UIBase {
    public enum Mode
    {
        None,
        Main,
        Resource
    }

    TweenPosition mMainPosTween;
    TweenPosition mResourcePosTween;

    UISlider mZoomSlider;

    [HideInInspector]
    public Mode mCurrentMode = Mode.None;

    void Awake()
    {
        mMainPosTween       = transform.Find("Main").GetComponent<TweenPosition>();
        mResourcePosTween   = transform.Find("Resource").GetComponent<TweenPosition>();
        mZoomSlider         = transform.Find("ZoomSlider").GetComponent<UISlider>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnEnableAnimation()
    {
        ToMainMode();
    }

    public void OnClickMainMenu()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.Main);
    }

    public void OnClickConquerInfo()
    {
        LobbyUIRoot.Instance.SetMenu(LobbyEnum.MenuSelect.ConqueredList);
    }

    public void OnClickGoldCollect()
    {
        int amount = ZoneManager.Instance.GetTotalResource(ResourceType.Gold);
        if (amount > 0)
        {
            LobbyUIRoot.Instance.kUserInfo.AddResource(ResourceType.Gold, amount);
            ZoneManager.Instance.CollectResource(ResourceType.Gold);
            string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000010), amount.ToString());
            MessageBox.Open(msg, null);
        }
        else
        {
            MessageBox.Open(3000011, null);
        }
    }

    public void OnClickMaterialCollect()
    {
        int amount = ZoneManager.Instance.GetTotalResource(ResourceType.Material);
        if (amount > 0)
        {
            LobbyUIRoot.Instance.kUserInfo.AddResource(ResourceType.Material, amount);
            ZoneManager.Instance.CollectResource(ResourceType.Material);
            string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000012), amount.ToString());
            MessageBox.Open(msg, null);
        }
        else
        {
            MessageBox.Open(3000013, null);
        }
    }

    public void OnClickCristalCollect()
    {
        int amount = ZoneManager.Instance.GetTotalResource(ResourceType.Cristal);
        if (amount > 0)
            MessageBox.Open(3000014, CristalCollect, null); 
        else
            MessageBox.Open(3000016, null);
    }

    public void CristalCollect()
    {
        int amount = ZoneManager.Instance.GetTotalResource(ResourceType.Cristal);
        if (amount > 0)
        {
            LobbyUIRoot.Instance.kUserInfo.AddResource(ResourceType.Cristal, amount);
            ZoneManager.Instance.CollectResource(ResourceType.Cristal);
            string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000015), amount.ToString());
            MessageBox.Open(msg, null);
        }
    }

    public void ToResourceMode()
    {
        if (mCurrentMode == Mode.Resource)
            return;

        mMainPosTween.gameObject.SetActive(false);
        mResourcePosTween.gameObject.SetActive(true);        

        mResourcePosTween.ResetToBeginning();
        mResourcePosTween.PlayForward();

        mCurrentMode = Mode.Resource;
    }

    public void ToMainMode()
    {
        mZoomSlider.gameObject.SetActive(true);

        if (mCurrentMode == Mode.Main)
            return;

        mResourcePosTween.gameObject.SetActive(false);
        mMainPosTween.gameObject.SetActive(true);

        mMainPosTween.ResetToBeginning();
        mMainPosTween.PlayForward();

        mCurrentMode = Mode.Main;
    }
    
    public void Zoom(float _value)
    {
        mZoomSlider.value = _value;
    }

    public void SideView()
    {
        mZoomSlider.gameObject.SetActive(false);
    }
}
