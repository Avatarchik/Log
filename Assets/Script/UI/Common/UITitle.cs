using UnityEngine;
using System.Collections;

public class UITitle : UIBase {

    UIButton mStartButton;

    UILabel mLoadingLabel;
    UILabel mLoadingPerLabel;

    void Awake()
    {
        mStartButton = transform.Find("StartButton").GetComponentInChildren<UIButton>(true);
        
        mLoadingLabel = transform.Find("LoadingLabel").GetComponent<UILabel>();
        mLoadingPerLabel = transform.Find("PercentLabel").GetComponent<UILabel>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetLoadingPercent(int _percent)
    {
        mLoadingPerLabel.text = StringUtil.TwoMix(_percent.ToString(), "%");

        if (_percent == 100)
            Invoke("HideLoading", 0.3f);
    }

    void HideLoading()
    {
        mLoadingLabel.enabled = false;
        mLoadingPerLabel.enabled = false;
        mStartButton.gameObject.SetActive(true);
    }

    public void OnClickGotoLobby()
    {
        SceneLoadManager.Instance.SetLoadScene(CommonEnum.SceneState.Lobby);
    }
}
