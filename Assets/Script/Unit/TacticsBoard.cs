using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonEnum;

public class TacticsBoard : ShipBoard {
    int mSelectUnitIndex = 0;

    [HideInInspector]
    public List<int> kEditShipList = new List<int>();

    void Awake()
    {        
    }

    void OnEnable()
    {
        EasyTouch.On_SimpleTap += OnClickSpotArea;
    }

    void OnDisable()
    {
        EasyTouch.On_SimpleTap -= OnClickSpotArea;
    }

    public void SelectPickUnitIndex(int _index)
    {
        //mSelectUnitIndex가 -1이라면 제거
        //mSelectUnitIndex가 0이라면 선택되지 않음
        mSelectUnitIndex = _index;
    }

    public override void OnPrepare()
    {
        kEditShipList.Clear();
        for (int i = 0; i < CommonDef.MAX_SHIP_GROUP_COUNT; i++)
            kEditShipList.Add(0);

        gameObject.SetActive(true);

        for (int i = 0; i < CommonDef.MAX_SHIP_GROUP_COUNT; i++)
        {
            int shipID = GameData.Local.GetSlotData(i);
            if (shipID == 0)
            {
                kEditShipList.Add(0);
                continue;
            }

            Model model = (Model)shipID;

            if (model == Model.None)
                continue;
            
            InsertShip(shipID, i);
        }

        LobbyUIRoot.Instance.kTacticsEditMenu.PageAbilityUpdate();
        LobbyUIRoot.Instance.kTacticsEditMenu.EditAbilityUpdate();
    }

    public void RemoveShip(int _slotIndex)
    {
        Transform slot = transform.Find("Slot" + _slotIndex.ToString());
        Ship [] ships = slot.GetComponentsInChildren<Ship>();
        slot.Find("Mark").gameObject.SetActive(true);

        kEditShipList[_slotIndex] = 0;

        for (int i = 0; i < ships.Length; i++)
            ObjectPoolManager.Instance.Release(ships[i].gameObject);        
    }

    void InsertShip(int _shipID, int _slotIndex)
    {
        Model model = (Model)_shipID;

        string resName = UnitSupport.TypeToString(_shipID);
        Transform slot = transform.Find("Slot" + _slotIndex.ToString());
        slot.Find("Mark").gameObject.SetActive(false);

        if (UnitSupport.IsSingleSpawn(model) == true)
        {
            Ship ship = ObjectPoolManager.Instance.GetGameObejct(resName).GetComponent<Ship>();
            ship.kModel = model;
            
            ship.transform.parent = slot;
            ship.kIsPlayer = kIsPlayerGroup;
            ship.transform.forward = slot.forward;
            ship.transform.position = slot.position;
        }
        else
        {            
            GameObject shipGroup = ObjectPoolManager.Instance.GetGameObejct(resName);            
            shipGroup.transform.forward = slot.forward;
            shipGroup.transform.position = slot.position;

            string childUnitResName = UnitSupport.ChildTypeToString(_shipID);

            for (int i = 0; i < shipGroup.transform.childCount; i++)
            {
                Ship childShip = ObjectPoolManager.Instance.GetGameObejct(childUnitResName).GetComponent<Ship>();
                childShip.transform.parent = slot;
                childShip.kIsPlayer = kIsPlayerGroup;
                childShip.transform.forward = transform.forward;
                childShip.transform.position = shipGroup.transform.GetChild(i).position;
                shipGroup.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        kEditShipList[_slotIndex] = _shipID;
    }
    
    void OnClickSpotArea(Gesture gesture)
    {
        if (gesture.pickObject == null)
            return;

        int selectSlotIndex = int.Parse(gesture.pickObject.name.Replace("Slot", ""));

        if (mSelectUnitIndex == 0)
        {
            MessageBox.Open(3000003, null);
            return;
        }

        //제거
        if ( mSelectUnitIndex == -1 )
        {
            RemoveShip(selectSlotIndex);
        }
        //추가
        else
        {
            int selectShipID = CommonDef.UNIT_ID_NUMBERING + mSelectUnitIndex;
            if( kEditShipList[selectSlotIndex] != 0 )
                RemoveShip(selectSlotIndex);
                        
            InsertShip(selectShipID, selectSlotIndex);
        }

        LobbyUIRoot.Instance.kTacticsEditMenu.EditAbilityUpdate();
    }

    public void Reset()
    {
        for( int i = 0; i < kEditShipList.Count; i++)
        {
            if (kEditShipList[i] == 0)
                continue;

            RemoveShip(i);
        }

        OnPrepare();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
