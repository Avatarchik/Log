using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConqueredZone : UIBase {
    TweenPosition mPanelTween;

    UILabel mMilitaryLevelLabel;

    UILabel mMaxMilitaryLabel;
    UILabel mCurMilitaryLabel;
    UILabel mAddMilitaryLabel;
    UILabel mExpensesGoldLabel;    

    UISlider mAddMilitarySlider;

    UIButton mMilitaryEnterButton;

    float mMinSliderValue = 0.0f;
    float mTotalMilitaryScore = 0;
    float mCurMilitaryScore = 0;
    float mAddMilitaryScore = 0.0f;
    float mExpensesGold = 0.0f;

    void Awake()
    {
        mPanelTween = GetComponent<TweenPosition>();

        mMilitaryLevelLabel = transform.Find("MilitaryLevelLabel").GetComponent<UILabel>();

        mMaxMilitaryLabel = transform.Find("MaxMilitaryLabel").GetComponent<UILabel>();
        mMaxMilitaryLabel.text = "0";
        mCurMilitaryLabel = transform.Find("CurMilitaryLabel").GetComponent<UILabel>();
        mCurMilitaryLabel.text = "0";
        mAddMilitaryLabel = transform.Find("AddMilitaryLabel").GetComponent<UILabel>();
        mAddMilitaryLabel.text = "0";
        mExpensesGoldLabel = transform.Find("ExpensesGoldLabel").GetComponent<UILabel>();
        mExpensesGoldLabel.text = "0";

        mAddMilitarySlider = transform.Find("AddMilitarySlider").GetComponent<UISlider>();

        mMilitaryEnterButton = transform.Find("MilitaryEnterButton").GetComponent<UIButton>();
        mMilitaryEnterButton.isEnabled = false;
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
    }
    
    public void Refresh()
    {
        Zone zone = ZoneManager.Instance.zoneSelect;
        if (zone == null )
            return;

        mAddMilitaryLabel.text = "0";
        mExpensesGoldLabel.text = StringUtil.ThreeMix("0", " ", LocalizationManager.Instance.GetLocalValue(23));

        mMilitaryLevelLabel.text = StringUtil.ThreeMix(LocalizationManager.Instance.GetLocalValue(22), " ", (GameData.User.militaryAbility + 1).ToString());

        mTotalMilitaryScore = EditDef.USER_MILITARY_SCORE + GameData.User.militaryAbility * EditDef.USER_MILITARY_SCORE;
        mMaxMilitaryLabel.text = ((int)mTotalMilitaryScore).ToString();

        mCurMilitaryScore = GameData.User.GetHaveZoneMilitary(zone.name);
        mCurMilitaryLabel.text = ((int)mCurMilitaryScore).ToString();
                
        mMinSliderValue = mCurMilitaryScore / mTotalMilitaryScore;
        mAddMilitarySlider.value = mMinSliderValue;

        mAddMilitaryScore   = 0.0f;
        mExpensesGold       = 0.0f;

        mMilitaryEnterButton.isEnabled = false;
    }

    public void OnChangeMilitarySlider()
    {
        if (mMinSliderValue >= mAddMilitarySlider.value)
        {
            mMilitaryEnterButton.isEnabled = false;
            mAddMilitarySlider.value = mMinSliderValue;
        }
        else
        {
            mMilitaryEnterButton.isEnabled = true;
        }

        float addValue = mAddMilitarySlider.value - mMinSliderValue;
        addValue = Mathf.Round(addValue * 100) / 100;

        mAddMilitaryScore = addValue * mTotalMilitaryScore;
        mAddMilitaryLabel.text = ((int)mAddMilitaryScore).ToString();
        mExpensesGold = ((int)mAddMilitaryScore / 50.0f) * EditDef.USER_50_MILITARY_TO_GOLD;
        mExpensesGoldLabel.text = StringUtil.ThreeMix(((int)mExpensesGold).ToString(), " ", LocalizationManager.Instance.GetLocalValue(23));
    }

    public void OnClickMilitaryEnterButton()
    {
        if( mExpensesGold > GameData.User.gold )
        {
            MessageBox.NotEnoughResource(CommonEnum.ResourceType.Gold);
        }
        else
        {
            string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000023), ((int)mExpensesGold).ToString());            
            msg = StringUtil.MacroString(msg, ((int)mAddMilitaryScore + (int)mCurMilitaryScore).ToString());

            MessageBox.Open(msg, MilitaryEnter, null);
        }
    }

    void MilitaryEnter()
    {
        MessageBox.Open(3000024, null);

        Zone zone = ZoneManager.Instance.zoneSelect;
        GameData.User.SetHaveZoneMilitary(zone.name, (int)mAddMilitaryScore + (int)mCurMilitaryScore);
        zone.RefreshMilitary(false);
        LobbyUIRoot.Instance.kUserInfo.AddResource(CommonEnum.ResourceType.Gold, (int)-mExpensesGold);        
        Refresh();
    }

    public void OnClickClose()
    {
        LobbyUIRoot.Instance.SetMenu(LobbyEnum.MenuSelect.WorldMap);
    }
}
