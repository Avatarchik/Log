using UnityEngine;
using System.Collections;

public class UILoading : UIBase {
    UILabel mLoadingPerLabel;

    void Awake()
    {
        mLoadingPerLabel = transform.Find("PercentLabel").GetComponent<UILabel>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        	
	}

    public void SetPercent(int _percent)
    {        
        mLoadingPerLabel.text = StringUtil.TwoMix(_percent.ToString(), "%");

        if (_percent == 100)
            Invoke("HideLoading", 0.3f);
    }

    void HideLoading()
    {
        gameObject.SetActive(false);
    }
}
