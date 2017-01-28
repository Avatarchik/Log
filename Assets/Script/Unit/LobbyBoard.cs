using UnityEngine;
using System.Collections;
using CommonEnum;

public class LobbyBoard : ShipBoard {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnPrepare()
    {
        gameObject.SetActive(true);
        
        for (int i = 0; i < CommonDef.MAX_SHIP_GROUP_COUNT; i++)
        {
            int shipID = GameData.Local.GetSlotData(i);
            if (shipID == 0)
                continue;

            Model model = (Model)shipID;

            if (model == Model.None)
                continue;
            
            SpawnShip(shipID, i);
        }
    }

    void SpawnShip(int _shipID, int _slotIndex)
    {
        Model model = (Model)_shipID;

        string resName = UnitSupport.TypeToString(_shipID);
        Transform slot = transform.Find("Slot" + _slotIndex.ToString());

        if (UnitSupport.IsSingleSpawn(model) == true)
        {
            Ship ship = ObjectPoolManager.Instance.GetGameObejct(resName).GetComponent<Ship>();
            ship.kModel = model;
            
            ship.transform.parent = slot;
            ship.kIsPlayer = kIsPlayerGroup;
            ship.transform.forward = slot.forward;

            Vector3 pos = slot.position;
            pos.z += Random.Range(-10.0f, 10.0f);
            pos.y += Random.Range(-20.0f, 20.0f);
            ship.transform.position = pos;
        }
        else
        {
            GameObject shipGroup = ObjectPoolManager.Instance.GetGameObejct(resName);
            shipGroup.transform.parent = slot;
            shipGroup.transform.forward = slot.forward;

            Vector3 pos = slot.position;
            pos.z += Random.Range(-10.0f, 10.0f);
            pos.y += Random.Range(-20.0f, 20.0f);
            shipGroup.transform.position = pos;

            string childUnitResName = UnitSupport.ChildTypeToString(_shipID);

            for (int i = 0; i < shipGroup.transform.childCount; i++)
            {
                Ship childShip = ObjectPoolManager.Instance.GetGameObejct(childUnitResName).GetComponent<Ship>();
                childShip.kIsPlayer = kIsPlayerGroup;
                childShip.transform.forward = transform.forward;
                childShip.transform.position = shipGroup.transform.GetChild(i).position;
                shipGroup.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
