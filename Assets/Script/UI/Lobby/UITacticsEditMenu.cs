using UnityEngine;
using System.Collections;
using CommonEnum;

public class UITacticsEditMenu : UIBase
{
    TacticsBoard mTacticsBoard;

    TweenPosition mBottomTween;
    TweenPosition mTopTween;

    UILabel mAttacLabel;
    UILabel mArmorLabel;
    UILabel mSpeedLabel;

    int mTotalAttack = 0;
    int mTotalArmor = 0;
    float mAverSpeed = 0;

    void Awake()
    {
        mTopTween = transform.Find("Top").GetComponent<TweenPosition>();
        mBottomTween = transform.Find("Bottom").GetComponent<TweenPosition>();

        mAttacLabel = transform.Find("Top/AttackLabel").GetComponent<UILabel>();
        mArmorLabel = transform.Find("Top/ArmorLabel").GetComponent<UILabel>();
        mSpeedLabel = transform.Find("Top/SpeedLabel").GetComponent<UILabel>();
    }

    public void OnPrepare()
    {
        mTacticsBoard = LobbyManager.Instance.kTacticsBoard;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickClear()
    {
        mTacticsBoard.SelectPickUnitIndex(-1);
    }

    public void OnClickAllClear()
    {
        MessageBox.Open(3000002, ResetBoard, null);
    }

    public void ResetBoard()
    {
        mTacticsBoard.Reset();
    }

    public void OnClickEditComplet()
    {
        MessageBox.Open(3000001, TacticsEditSave, GoMainLobby);
    }

    public void TacticsEditSave()
    {
        for (int i = 0; i < mTacticsBoard.kEditShipList.Count; i++)
            GameData.Local.SetSlotData(i, mTacticsBoard.kEditShipList[i]);

        GoMainLobby();
    }

    public void GoMainLobby()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.Main);
    }

    public void OnClickUnitList()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.UnitList);
    }

    public void OnClickCloseButton()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.Main);
    }

    public override void OnEnableAnimation()
    {
        mTopTween.ResetToBeginning();
        mTopTween.PlayForward();
        mBottomTween.ResetToBeginning();
        mBottomTween.PlayForward();
    }

    public void PageAbilityUpdate()
    {
        mTotalAttack = 0;
        mTotalArmor = 0;
        float totalSpeed = 0.0f;

        int unitTotalCount = 0;

        for (int i = 0; i < StageDef.MAX_SHIP_GROUP_COUNT; i++)
        {
            if (GameData.Local.GetSlotData(i) == 0)
                continue;
                        
            int shipID = GameData.Local.GetSlotData(i);
            int shipLv = GameData.Local.GetShipLevel(shipID);
            DT_ShipData_Info info = CDT_ShipData_Manager.Instance.GetInfo(shipID);
            if (info == null)
                continue;

            int unitCount = info.UnitCount;
            if (ShipSupport.IsSingleSpawn((Model)shipID) == false)            
                info = CDT_ShipData_Manager.Instance.GetInfo(info.Reference);

            int minAttack = info.MinAttack + (int)(info.MinAttack * (shipLv - 1) * NumDef.TO_PERCENT_UNIT);
            int maxAttack = info.MaxAttack + (int)(info.MaxAttack * (shipLv - 1) * NumDef.TO_PERCENT_UNIT);
            mTotalAttack += ((int)((minAttack + maxAttack) * 0.5f)) * unitCount;

            int shieldAmount = info.ShieldAmount + (int)(info.ShieldAmount * (shipLv - 1) * NumDef.TO_PERCENT_UNIT);
            int bodyAmount = info.BodyAmount + (int)(info.BodyAmount * (shipLv - 1) * NumDef.TO_PERCENT_UNIT);
            mTotalArmor += (shieldAmount + bodyAmount) * unitCount;

            totalSpeed += (info.TurnForce + info.MaxVelocity) * unitCount;

            unitTotalCount += unitCount;
        }

        if (unitTotalCount == 0)
            mAverSpeed = 0.0f;
        else
            mAverSpeed = totalSpeed / unitTotalCount;
    }

    public void EditAbilityUpdate()
    {
        int totalAttack = 0;
        int totalArmor = 0;
        float totalSpeed = 0.0f;

        int unitTotalCount = 0;

        for (int i = 0; i < StageDef.MAX_SHIP_GROUP_COUNT; i++)
        {
            if (mTacticsBoard.kEditShipList[i] == 0)
                continue;

            int shipID = mTacticsBoard.kEditShipList[i];
            int shipLv = GameData.Local.GetShipLevel(shipID);
            
            DT_ShipData_Info info = CDT_ShipData_Manager.Instance.GetInfo(shipID);
            if (info == null)
                continue;

            int unitCount = info.UnitCount;
            if (ShipSupport.IsSingleSpawn((Model)shipID) == false)
                info = CDT_ShipData_Manager.Instance.GetInfo(info.Reference);

            int minAttack = info.MinAttack + (int)(info.MinAttack * (shipLv - 1) * NumDef.TO_PERCENT_UNIT);
            int maxAttack = info.MaxAttack + (int)(info.MaxAttack * (shipLv - 1) * NumDef.TO_PERCENT_UNIT);
            totalAttack += ((int)((minAttack + maxAttack) * 0.5f)) * unitCount;

            int shieldAmount = info.ShieldAmount + (int)(info.ShieldAmount * (shipLv - 1) * NumDef.TO_PERCENT_UNIT);
            int bodyAmount = info.BodyAmount + (int)(info.BodyAmount * (shipLv - 1) * NumDef.TO_PERCENT_UNIT);
            totalArmor += (shieldAmount + bodyAmount) * unitCount;

            totalSpeed += (info.TurnForce + info.MaxVelocity) * unitCount;

            unitTotalCount += unitCount;
        }

        float averSpeed = 0;
        if (unitTotalCount != 0)            
            averSpeed = totalSpeed / unitTotalCount;

        string comAttackStr = "(--)";
        if (totalAttack < mTotalAttack)
            comAttackStr = "[ff0000]" + StringUtil.ThreeMix("(-", (mTotalAttack - totalAttack).ToString(), ")") + "[-]";
        if (totalAttack > mTotalAttack)
            comAttackStr = "[00ff00]" + StringUtil.ThreeMix("(+", (totalAttack - mTotalAttack).ToString(), ")") + "[-]";

        string comArmorStr = "(--)";
        if (totalArmor < mTotalArmor)
            comArmorStr = "[ff0000]" + StringUtil.ThreeMix("(-", (mTotalArmor - totalArmor).ToString(), ")") + "[-]";
        if (totalArmor > mTotalArmor)
            comArmorStr = "[00ff00]" + StringUtil.ThreeMix("(+", (totalArmor - mTotalArmor).ToString(), ")") + "[-]";

        string comSpeedStr = "(--)";
        if (averSpeed < mAverSpeed)
            comSpeedStr = "[ff0000]" + StringUtil.ThreeMix("(-", StringUtil.FloatToString(mAverSpeed - averSpeed, 2), ")") + "[-]";
        if (averSpeed > mAverSpeed)
            comSpeedStr = "[00ff00]" + StringUtil.ThreeMix("(+", StringUtil.FloatToString(averSpeed - mAverSpeed, 2), ")") + "[-]";

        string averSpeedStr = StringUtil.FloatToString(averSpeed, 2);
        mAttacLabel.text = StringUtil.TwoMix(totalAttack.ToString(), comAttackStr);
        mArmorLabel.text = StringUtil.TwoMix(totalArmor.ToString(), comArmorStr);
        mSpeedLabel.text = StringUtil.TwoMix(averSpeedStr, comSpeedStr);
    }

}
