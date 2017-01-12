using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonEnum;

public class ShipBoard : MonoBehaviour {
    
    [HideInInspector]
    public bool kIsPlayerGroup = true;
    
    void Awake(){
        if (gameObject.layer == StageDef.LAYER_PLAYER)
            kIsPlayerGroup = true;
        else
            kIsPlayerGroup = false;
    }

    // Use this for initialization
    void Start () {
    }
        
    public virtual void OnPrepare()
    {
        gameObject.SetActive(true);

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].enabled = false;

        int stageID = NumDef.STAGE_ID_NUMBERING + StagePlayManager.Instance.kCurStageNumber;

        for (int i = 0; i < StageDef.MAX_SHIP_GROUP_COUNT; i++)
        {
            int shipID = 0;

            if( kIsPlayerGroup == true )
                shipID = GameData.Local.GetSlotData(i);
            else
                shipID = GetStageEnemyShipID(stageID, i);

            if (shipID == 0)
                continue;
            
            Model model = (Model)shipID;

            if (model == Model.None )
                continue;

            string resName = ShipSupport.TypeToString(shipID);
            Transform slot = transform.Find("Slot" + i.ToString());

            if ( ShipSupport.IsSingleSpawn(model) == true )
            {                
                Ship ship = ObjectPoolManager.Instance.GetGameObejct(resName).GetComponent<Ship>();
                ship.kModel = model;
                ship.kAllocID = StagePlayManager.Instance.kPlayerShipList.Count + StagePlayManager.Instance.kEnemyShipList.Count;

                if (kIsPlayerGroup == true)
                    StagePlayManager.Instance.kPlayerShipList.Add(ship);
                else
                    StagePlayManager.Instance.kEnemyShipList.Add(ship);
                
                ship.kIsPlayerGroup = kIsPlayerGroup;
                ship.transform.forward = slot.forward;
                ship.transform.position = slot.position;
            }
            else
            {
                GameObject shipGroup = ObjectPoolManager.Instance.GetGameObejct(resName);
                shipGroup.transform.forward = slot.forward;
                shipGroup.transform.position = slot.position;

                string refResName = ShipSupport.RefTypeToString(shipID);

                for (int n = 0; n < shipGroup.transform.childCount; n++)
                {
                    DT_ShipData_Info info = CDT_ShipData_Manager.Instance.GetInfo(shipID);
                    Ship childShip = ObjectPoolManager.Instance.GetGameObejct(refResName).GetComponent<Ship>();
                    childShip.kModel = (Model)info.Reference;
                    childShip.kAllocID = StagePlayManager.Instance.kPlayerShipList.Count + StagePlayManager.Instance.kEnemyShipList.Count;
                    childShip.kIsPlayerGroup = kIsPlayerGroup;
                    childShip.transform.forward = transform.forward;
                    childShip.transform.position = shipGroup.transform.GetChild(n).position;

                    if (kIsPlayerGroup == true)
                        StagePlayManager.Instance.kPlayerShipList.Add(childShip);
                    else
                        StagePlayManager.Instance.kEnemyShipList.Add(childShip);

                    shipGroup.transform.GetChild(n).gameObject.SetActive(false);
                }
            }            
        }
    }
    
	// Update is called once per frame
	void Update () {
	}

    int GetStageEnemyShipID(int _curStageID, int _slotIndex)
    {
        DT_StageData_Info info = CDT_StageData_Manager.Instance.GetInfo(_curStageID);

        switch(_slotIndex)
        {
            case 0:     return info.Spot1;
            case 1:     return info.Spot2;
            case 2:     return info.Spot3;
            case 3:     return info.Spot4;
            case 4:     return info.Spot5;
            case 5:     return info.Spot6;
            case 6:     return info.Spot7;
            case 7:     return info.Spot8;
            case 8:     return info.Spot9;
            case 9:     return info.Spot10;
            case 10:    return info.Spot11;
            case 11:    return info.Spot12;
            case 12:    return info.Spot13;
            case 13:    return info.Spot14;
            case 14:    return info.Spot15;
        }

        return 0;
    }
}
