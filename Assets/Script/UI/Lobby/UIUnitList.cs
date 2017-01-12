using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonEnum;

public class UIUnitList : UIBase {
    List<Transform> mItemList = new List<Transform>();

    UILabel mNameLabel;
    UILabel mArmorLabel;
    UILabel mAttackLabel;
    UILabel mCostLabel;
    UILabel mGradeLabel;

    UISprite mUnitSprite;

    UIButton mEditButton;
    UIButton mDetailButton;

    TacticsBoard mTacticsBoard;

    TweenPosition mPanelTween;

    void Awake()
    {
        Transform gridTrans = transform.Find("List/Grid");
        for (int i = 0; i < gridTrans.childCount; i++)
        {
            Transform item = gridTrans.Find("Item" + (i + 1).ToString());
            mItemList.Add(item);

            UIEventListener.Get(item.gameObject).onDoubleClick += ItemDoubleClick;
            
            DT_ShipData_Info info = CDT_ShipData_Manager.Instance.GetInfo(NumDef.SHIP_ID_NUMBERING + i + 1);
            
            UISprite unitSpr = item.Find("UnitSprite").GetComponent<UISprite>();
            unitSpr.spriteName = info.ResourceName;
            UISprite unitRound = item.Find("Background").GetComponent<UISprite>();

            switch ((Grade)info.Grade)
            {
                case Grade.Common:
                    unitRound.color = Color.white;
                    break;
                case Grade.Uncommon:
                    unitRound.color = new Color(146.0f / 255.0f, 208.0f / 255.0f, 80.0f / 255.0f);
                    break;
                case Grade.Rare:
                    unitRound.color = new Color(0.0f / 255.0f, 176.0f / 255.0f, 255.0f / 255.0f);                    
                    break;
                case Grade.Epic:
                    unitRound.color = new Color(146.0f / 255.0f, 48.0f / 255.0f, 160.0f / 255.0f);
                    break;
                case Grade.Legendary:
                    unitRound.color = new Color(255.0f / 255.0f, 192.0f / 255.0f, 0.0f / 255.0f);
                    break;
            }
            
            Transform selectTrans = item.Find("Select");
            selectTrans.gameObject.SetActive(false);
        }

        transform.position.Set(0.0f, 0.0f, 0.0f);

        mUnitSprite = transform.Find("SelectUnit").GetComponent<UISprite>();
        mNameLabel = transform.Find("SelectUnit/Name").GetComponent<UILabel>();
        mArmorLabel = transform.Find("Info/Armor").GetComponent<UILabel>();
        mAttackLabel = transform.Find("Info/Attack").GetComponent<UILabel>();
        mCostLabel = transform.Find("Info/Cost").GetComponent<UILabel>();        
        mGradeLabel = transform.Find("Info/Grade").GetComponent<UILabel>();
        
        mUnitSprite.spriteName = "";
        mNameLabel.text = "";
        mGradeLabel.text = "";
        mArmorLabel.text = "";
        mAttackLabel.text = "";
        mCostLabel.text = "";

        mEditButton = transform.Find("EditButton").GetComponent<UIButton>();
        mEditButton.gameObject.SetActive(false);
        mDetailButton = transform.Find("DetailViewButton").GetComponent<UIButton>();
        mDetailButton.gameObject.SetActive(false);

        mPanelTween = GetComponent<TweenPosition>();
        mPanelTween.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPrepare()
    {
        mTacticsBoard = LobbyManager.Instance.kTacticsBoard;
    }

    public void OnClickSelect(GameObject _obj)
    {
        SelectClear();
        
        string spriteName = _obj.transform.Find("UnitSprite").GetComponent<UISprite>().spriteName;

        string indexStr = _obj.name.Replace("Item", "");
        int index = int.Parse(indexStr);
        DT_ShipData_Info info = CDT_ShipData_Manager.Instance.GetInfo(NumDef.SHIP_ID_NUMBERING + index);
        if (info == null)
            return;

        mItemList[index - 1].Find("Select").gameObject.SetActive(true);
        mUnitSprite.spriteName = spriteName;
        mNameLabel.text = LocalizationManager.Instance.GetLocalValue(info.Name);
        mArmorLabel.text = info.ShieldAmount.ToString() + "/" + info.BodyAmount.ToString();
        mAttackLabel.text = info.MinAttack.ToString() + "-" + info.MaxAttack.ToString();

        int shipID = NumDef.STAGE_ID_NUMBERING + index;
        Model model = (Model)shipID;
        int refID = shipID;
        if (ShipSupport.IsSingleSpawn(model) == false)
            refID = CDT_ShipData_Manager.Instance.GetInfo(shipID).Reference;

        mNameLabel.text = LocalizationManager.Instance.GetLocalValue(info.Name);
        mGradeLabel.text = LocalizationManager.Instance.GetLocalValue(1) + " : " + ShipSupport.GetGradeString((Grade)info.Grade);
        mCostLabel.text = LocalizationManager.Instance.GetLocalValue(4) + " : " + info.Cost.ToString();
        mGradeLabel.text = LocalizationManager.Instance.GetLocalValue(1) + " : " + ShipSupport.GetGradeString((Grade)info.Grade);

        info = CDT_ShipData_Manager.Instance.GetInfo(refID);
        mArmorLabel.text = LocalizationManager.Instance.GetLocalValue(2) + " : " + info.ShieldAmount.ToString() + "/" + info.BodyAmount.ToString();
        mAttackLabel.text = LocalizationManager.Instance.GetLocalValue(3) + " : " + info.MinAttack.ToString() + " ~ " + info.MaxAttack.ToString();

        mEditButton.gameObject.SetActive(true);
        mDetailButton.gameObject.SetActive(true);

        mTacticsBoard.SelectPickUnitIndex(index);
        LobbyUIRoot.Instance.kUnitDetailInfo.SelectPickUnitIndex(index);
    }

    public void ItemDoubleClick(GameObject _obj)
    {
        OnClickEditButton();
    }

    public void SelectClear()
    {
        for (int i = 0; i < mItemList.Count; i++)
        {
            Transform selectTrans = mItemList[i].Find("Select");
            selectTrans.gameObject.SetActive(false);
        }

        mUnitSprite.spriteName = "";
        mGradeLabel.text = "";
        mNameLabel.text = "";
        mArmorLabel.text = "";
        mAttackLabel.text = "";
        mCostLabel.text = "";
                
        mEditButton.gameObject.SetActive(false);
        mDetailButton.gameObject.SetActive(false);

        mTacticsBoard.SelectPickUnitIndex(0);
        LobbyUIRoot.Instance.kUnitDetailInfo.SelectPickUnitIndex(-1);
    }
    
    public void OnClickEditButton()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.Tactics);
    }

    public void OnEditCompletButton()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.Main);
    }

    public void OnClickCloseButton()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.Main);
    }

    public void OnClickUnitDetail()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.UnitDetail);
    }

    public override void OnEnableAnimation()
    {
        mPanelTween.ResetToBeginning();
        mPanelTween.PlayForward();
    }
}