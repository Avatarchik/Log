using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnconqueredZone : UIBase {
    TweenPosition mPanelTween;

    UIToggle mFirstToggle;
    UIToggle mSecondToggle;
    UIToggle mThirdToggle;
    UIToggle mFourthToggle;
    UIToggle mFifthToggle;

    UISprite[] mUnitSprites = new UISprite[CommonDef.MAX_SHIP_GROUP_COUNT];

    UILabel mCostLabel;

    Dictionary<int, int> mUnitCounterDic = new Dictionary<int, int>();

    int mAddTotalCost = 0;

    void Awake()
    {
        mPanelTween = GetComponent<TweenPosition>();

        mFirstToggle = transform.Find("UnitGroupSelect/FirstToggleButton").GetComponent<UIToggle>();
        mSecondToggle = transform.Find("UnitGroupSelect/SecondToggleButton").GetComponent<UIToggle>();
        mThirdToggle = transform.Find("UnitGroupSelect/ThirdToggleButton").GetComponent<UIToggle>();
        mFourthToggle = transform.Find("UnitGroupSelect/FourthToggleButton").GetComponent<UIToggle>();
        mFifthToggle = transform.Find("UnitGroupSelect/FifthToggleButton").GetComponent<UIToggle>();
        
        mCostLabel = transform.Find("CostLabel").GetComponent<UILabel>();

        for (int i = 0; i < CommonDef.MAX_SHIP_GROUP_COUNT; i++)
            mUnitSprites[i] = transform.Find("UnitGroupView/Icon" + (i + 1).ToString() + "/UnitSprite").GetComponent<UISprite>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnEnableAnimation()
    {
        mPanelTween.ResetToBeginning();
        mPanelTween.PlayForward();

        switch (GameData.Local.tacticsPage)
        {
            case 0:
                mFirstToggle.value = true;
                OnUnitGroupToggleClick(mFirstToggle.GetComponent<UIData>());
                break;
            case 1:
                mSecondToggle.value = true;
                OnUnitGroupToggleClick(mSecondToggle.GetComponent<UIData>());
                break;
            case 2:
                mThirdToggle.value = true;
                OnUnitGroupToggleClick(mThirdToggle.GetComponent<UIData>());
                break;
            case 3:
                mFourthToggle.value = true;
                OnUnitGroupToggleClick(mFourthToggle.GetComponent<UIData>());
                break;
            case 4:
                mFifthToggle.value = true;
                OnUnitGroupToggleClick(mFifthToggle.GetComponent<UIData>());
                break;
        }
    }
    
    public void OnUnitGroupToggleClick(UIData _data)
    {
        mUnitCounterDic.Clear();
        mAddTotalCost = 0;
        GameData.Local.tacticsPage = _data.kNumber;
        
        for (int i = 0; i < CommonDef.MAX_SHIP_GROUP_COUNT; i++)
        {
            int shipID = GameData.Local.GetSlotData(i);
            if (shipID == 0)
            {
                mUnitSprites[i].spriteName = null;
                continue;
            }

            if (mUnitCounterDic.ContainsKey(shipID) == false)
                mUnitCounterDic[shipID] = 1;
            else
                mUnitCounterDic[shipID] += 1;

            if (shipID == 0)
            {
                mUnitSprites[i].enabled = false;
                continue;
            }
            mUnitSprites[i].enabled = true;
            mUnitSprites[i].spriteName = ((Model)shipID).ToString();
        }

        IDictionaryEnumerator e = mUnitCounterDic.GetEnumerator();
        while (e.MoveNext())
        {
            int shipID = (int)e.Key;
            float needCount = (int)e.Value;
            float haveCount = GameData.User.GetHaveUnitCount(shipID);

            float buyCount = needCount - haveCount;
            float cost = 0;
            if (buyCount > 0 )
            {
                DT_UnitData_Info info = CDT_UnitData_Manager.Instance.GetInfo(shipID);
                cost = info.Cost * buyCount;
            }

            mAddTotalCost += (int)cost;
        }

        mCostLabel.text = mAddTotalCost.ToString();
    }

    public void OnCommandConquerClick()
    {
        if (mUnitCounterDic.Count == 0)
        {
            MessageBox.Open(3000022, null);
            return;
        }

        string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000004), mAddTotalCost.ToString());
        MessageBox.Open(msg, ConquerStart, null);
    }  

    public void ConquerStart()
    {
        GameData.User.gold -= mAddTotalCost;

        IDictionaryEnumerator e = mUnitCounterDic.GetEnumerator();
        while (e.MoveNext())
        {
            int shipID = (int)e.Key;
            int shipCount = (int)e.Value;
            GameData.User.SetHaveUnitCount(shipID, shipCount);
        }

        GameData.Lobby.kSelectMode = StageEnum.Mode.Conquer;
        SceneLoadManager.Instance.SetLoadScene(SceneState.Stage);
    }

    public void OnCloseClick()
    {
        LobbyUIRoot.Instance.SetMenu(LobbyEnum.MenuSelect.WorldMap);
    }
}
