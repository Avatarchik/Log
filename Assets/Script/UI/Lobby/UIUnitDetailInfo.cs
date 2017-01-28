using UnityEngine;
using System.Collections;
using CommonEnum;

public class UIUnitDetailInfo : UIBase {
    int mSelectUnitIndex = -1;

    TweenPosition mPanelTween;

    Transform mUnitBoard;

    UILabel mNameLabel;
    UILabel mGradeLabel;
    UILabel mCostLabel;
    UILabel mArmorLabel;
    UILabel mAttackLabel;
    UILabel mAttackSpeedLabel;
    UILabel mAttackRangeLabel;
    UILabel mMoveSpeedLabel;
    UILabel mTurnSpeedLabel;
    UILabel mUnitCountLabel;
    UILabel mFeatureLabel;

    void Awake()
    {
        mPanelTween = GetComponent<TweenPosition>();
        mPanelTween.enabled = false;

        mNameLabel = transform.Find("View/Label").GetComponent<UILabel>();

        mGradeLabel = transform.Find("Info/Grade").GetComponent<UILabel>();
        mCostLabel = transform.Find("Info/Cost").GetComponent<UILabel>();
        mArmorLabel = transform.Find("Info/Armor").GetComponent<UILabel>();
        mAttackLabel = transform.Find("Info/Attack").GetComponent<UILabel>();
        mAttackSpeedLabel = transform.Find("Info/AttackSpeed").GetComponent<UILabel>();
        mAttackRangeLabel = transform.Find("Info/AttackRange").GetComponent<UILabel>();
        mMoveSpeedLabel = transform.Find("Info/Speed").GetComponent<UILabel>();
        mTurnSpeedLabel = transform.Find("Info/Turn").GetComponent<UILabel>();
        mUnitCountLabel = transform.Find("Info/Count").GetComponent<UILabel>();
        mFeatureLabel = transform.Find("Feature").GetComponent<UILabel>();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if( mIsRightRot == true )
            LobbyManager.Instance.kUnitBoard.transform.Rotate(0.0f, -Time.deltaTime * 100.0f, 0.0f);
        if (mIsLeftRot == true)
            LobbyManager.Instance.kUnitBoard.transform.Rotate(0.0f, Time.deltaTime * 100.0f, 0.0f);
    }

    public void SelectPickUnitIndex(int _index)
    {
        mSelectUnitIndex = _index;
    }

    public void OnPrepare()
    {
        if (mSelectUnitIndex == -1)
            return;

        OnClickRotationZero();

        //UnitCamera camera = LobbyManager.Instance.kUnitCamera;
        mUnitBoard = GameObject.Find("UnitBoard").transform;
        Ship[] shipArray = mUnitBoard.transform.GetComponentsInChildren<Ship>();
        for (int i = 0; i < shipArray.Length; i++)
            ObjectPoolManager.Instance.Release(shipArray[i].gameObject);

        for (int i = 0; i < mUnitBoard.transform.childCount; i++)
            ObjectPoolManager.Instance.Release(mUnitBoard.transform.GetChild(i).gameObject);

        Vector3 pos = mUnitBoard.transform.position;

        int shipID = CommonDef.STAGE_ID_NUMBERING + mSelectUnitIndex;
        Model model = (Model)shipID;
        string resName = UnitSupport.TypeToString(shipID);
        
        if (UnitSupport.IsSingleSpawn(model) == true)
        {
            Ship ship = ObjectPoolManager.Instance.GetGameObejct(resName).GetComponent<Ship>();
            ship.kIsPlayer = true;
            ship.transform.forward = Vector3.forward;
            ship.transform.position = pos;
            ship.transform.parent = mUnitBoard.transform;
            //ship.EngineActive(false);

            Transform[] childs = ship.gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childs.Length; i++)
                childs[i].gameObject.layer = StageDef.LAYER_UNIT;
        }
        else
        {
            GameObject shipGroup = ObjectPoolManager.Instance.GetGameObejct(resName);
            shipGroup.transform.forward = Vector3.forward;
            shipGroup.transform.position = pos;
            shipGroup.transform.parent = mUnitBoard.transform;

            string childUnitResName = UnitSupport.ChildTypeToString(shipID);

            for (int n = 0; n < shipGroup.transform.childCount; n++)
            {                
                Ship childShip = ObjectPoolManager.Instance.GetGameObejct(childUnitResName.ToString()).GetComponent<Ship>();

                childShip.transform.forward = Vector3.forward;
                childShip.transform.position = shipGroup.transform.GetChild(n).position;
                childShip.transform.parent = mUnitBoard.transform;
                //childShip.EngineActive(false);

                shipGroup.transform.GetChild(n).gameObject.SetActive(false);

                Transform[] childs = childShip.gameObject.GetComponentsInChildren<Transform>();
                for (int i = 0; i < childs.Length; i++)
                    childs[i].gameObject.layer = StageDef.LAYER_UNIT;
            }

            ObjectPoolManager.Instance.Release(shipGroup.gameObject);
        }

        DT_UnitData_Info info = CDT_UnitData_Manager.Instance.GetInfo(shipID);
        mNameLabel.text = LocalizationManager.Instance.GetLocalValue(info.Name);
        mGradeLabel.text = LocalizationManager.Instance.GetLocalValue(1) + " : " + UnitSupport.GetGradeString((Grade)info.Grade);
        mCostLabel.text = LocalizationManager.Instance.GetLocalValue(4) + " : " + info.Cost.ToString();
        mFeatureLabel.text = LocalizationManager.Instance.GetLocalValue(info.Description);
        
        mArmorLabel.text = LocalizationManager.Instance.GetLocalValue(2) + " : " + info.ShieldAmount.ToString() + "/" + info.BodyAmount.ToString();
        mAttackLabel.text = LocalizationManager.Instance.GetLocalValue(3) + " : " + info.MinAttack.ToString() + " ~ " + info.MaxAttack.ToString();
        mAttackSpeedLabel.text = LocalizationManager.Instance.GetLocalValue(5) + " : " + info.AttackCoolTime.ToString();
        mAttackRangeLabel.text = LocalizationManager.Instance.GetLocalValue(6) + " : " + info.AttackRange.ToString();
        mMoveSpeedLabel.text = LocalizationManager.Instance.GetLocalValue(7) + " : " + info.MaxVelocity.ToString();
        mTurnSpeedLabel.text = LocalizationManager.Instance.GetLocalValue(8) + " : " + info.TurnForce.ToString();
        mUnitCountLabel.text = LocalizationManager.Instance.GetLocalValue(9) + " : " + info.UnitCount.ToString();
    }

    public void OnClickClose()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.UnitList);
    }

    public override void OnEnableAnimation()
    {
        mPanelTween.ResetToBeginning();
        mPanelTween.PlayForward();
    }

    bool mIsLeftRot = false;
    bool mIsRightRot = false;
    public void OnClickRotationRight()
    {
        mIsRightRot = true;
        mIsLeftRot = false;
    }
    
    public void OnClickRotationLeft()
    {
        mIsRightRot = false;
        mIsLeftRot = true;
    }
    
    public void OnClickRotationZero()
    {
        mIsRightRot = false;
        mIsLeftRot = false;
        LobbyManager.Instance.kUnitBoard.transform.eulerAngles = Vector3.zero;
    }
}
