using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

public class UIUserInfo : UIBase {
    UILabel mLevelLabel;
    UILabel mExpLabel;
    UISlider mExpProgressSlider;

    UILabel mGoldLabel;
    UILabel mMaterialLabel;
    UILabel mCristalLabel;

    void Awake()
    {
        mGoldLabel = transform.Find("GoldLabel").GetComponent<UILabel>();
        mMaterialLabel = transform.Find("MaterialLabel").GetComponent<UILabel>();
        mCristalLabel = transform.Find("CristalLabel").GetComponent<UILabel>();

        mLevelLabel = transform.Find("LevelLabel").GetComponent<UILabel>();
        mExpLabel = transform.Find("ExpLabel").GetComponent<UILabel>();
        mExpProgressSlider = transform.Find("ExpProgress").GetComponent<UISlider>();

        Refresh();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Refresh()
    {
        mGoldLabel.text = GameData.User.gold.ToString();
        mMaterialLabel.text = GameData.User.material.ToString();
        mCristalLabel.text = GameData.User.cristal.ToString();

        mLevelLabel.text = GameData.User.level.ToString();

        string curExpStr = StringUtil.FloatTo2FrontString((float)GameData.User.exp);
        mExpLabel.text = StringUtil.TwoMix(curExpStr, "%");

        int nextExp = CDT_UserLevelData_Manager.Instance.GetInfo(GameData.User.level).NextLevelUpExp;
        mExpProgressSlider.value = (float)GameData.User.exp / (float)nextExp;
    }

    public void AddResource(ResourceType _type, int _addAmount)
    {
        int amount = 0;
        switch (_type)
        {
            case ResourceType.Gold:
                amount = GameData.User.gold;
                GameData.User.gold = GameData.User.gold + _addAmount;
                break;
            case ResourceType.Material:
                amount = GameData.User.material;
                GameData.User.material = GameData.User.material + _addAmount;
                break;
            case ResourceType.Cristal:
                amount = GameData.User.cristal;
                GameData.User.cristal = GameData.User.cristal + _addAmount;
                break;
        }

        StartCoroutine(AddResourceUpdate(_type, amount, _addAmount, 1.0f));
    }

    IEnumerator AddResourceUpdate(ResourceType _type, int _fromAmount, int _addAmount, float _duration)
    {
        float curTime = 0.0f;

        UILabel label = null;        
        switch (_type)
        {
            case ResourceType.Gold:
                label = mGoldLabel;
                break;
            case ResourceType.Material:
                label = mMaterialLabel;
                break;
            case ResourceType.Cristal:
                label = mCristalLabel;
                break;
        }

        int fromAmount = _fromAmount;
        while (true)
        {
            curTime += Time.deltaTime;
            
            if (curTime >= _duration)
                curTime = _duration;

            int value = fromAmount + (int)((float)_addAmount * (curTime / _duration));
            label.text = value.ToString();

            if ( curTime == _duration )
                yield break;
            else
                yield return null;
        }
    }
}
