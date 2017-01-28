using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConqueredList : UIBase {
    UILabel mLabel;

    TweenPosition mPosTween;

    public class ListItem
    {
        public GameObject itemObject;
        public UISprite planetSpr;
        public UILabel productLevelLabel;
        public UILabel storageLevelLabel;
        public UILabel nationLabel;
        public UIButton planetButton;
        public UIButton productUpButton;
        public UIButton storageUpButton;
        public UIButton detailButton;
    }

    List<ListItem> mListItemList = new List<ListItem>();

    void Awake()
    {
        mLabel = transform.Find("Label").GetComponent<UILabel>();

        Transform listTrans = transform.Find("List/Grid");
        for(int i = 0; i < listTrans.childCount; i++)
        {
            ListItem item = new ListItem();
            item.itemObject         = listTrans.Find("Item" + (i + 1).ToString()).gameObject;
            item.planetSpr          = listTrans.Find("Item" + (i + 1).ToString() + "/PlanetSprite").GetComponent<UISprite>();
            item.planetSpr.color = Color.gray;

            item.productLevelLabel  = listTrans.Find("Item" + (i + 1).ToString() + "/ProductLevelLabel").GetComponent<UILabel>();
            item.storageLevelLabel = listTrans.Find("Item" + (i + 1).ToString() + "/StorageLevelLabel").GetComponent<UILabel>();

            item.nationLabel        = listTrans.Find("Item" + (i + 1).ToString() + "/NationLabel").GetComponent<UILabel>();
            
            item.productUpButton    = listTrans.Find("Item" + (i + 1).ToString() + "/ProductUpButton").GetComponent<UIButton>();
            item.productUpButton.gameObject.SetActive(false);

            item.storageUpButton    = listTrans.Find("Item" + (i + 1).ToString() + "/StorageUpButton").GetComponent<UIButton>();
            item.storageUpButton.gameObject.SetActive(false);

            item.detailButton = listTrans.Find("Item" + (i + 1).ToString() + "/DetailInfoButton").GetComponent<UIButton>();
            item.detailButton.gameObject.SetActive(false);

            item.itemObject.SetActive(false);

            mListItemList.Add(item);
        }

        mPosTween = GetComponent<TweenPosition>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnEnableAnimation()
    {
        mPosTween.ResetToBeginning();
        mPosTween.PlayForward();
    }

    public void Refresh()
    {
        int conquredCount = ZoneManager.Instance.kNationList[(int)Nation.Name.User].kConqueredZoneList.Count;
        string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000009), EditDef.MAX_PLANET.ToString());
        msg = StringUtil.MacroString(msg, conquredCount.ToString());
        mLabel.text = msg;

        for (int i = 0; i < ZoneManager.Instance.kPlanetZoneList.Count; i++)
        {            
            Zone zone = ZoneManager.Instance.kPlanetZoneList[i];
            DT_PlanetData_Info info = CDT_PlanetData_Manager.Instance.GetInfo(zone.kPlanetID);
            
            ListItem item = mListItemList[i];
            item.itemObject.SetActive(true);
            if (zone.kNation.kName == Nation.Name.User)
            {
                item.planetSpr.color = Color.white;
                item.detailButton.gameObject.SetActive(true);
                item.storageUpButton.gameObject.SetActive(true);
                item.productUpButton.gameObject.SetActive(true);
            }
            else
            {
                item.planetSpr.color = Color.gray;
                item.detailButton.gameObject.SetActive(false);
                item.storageUpButton.gameObject.SetActive(false);
                item.productUpButton.gameObject.SetActive(false);
            }

            int productLevel = GameData.Local.GetPlanetProductLevel(zone.name);
            int storageLevel = GameData.Local.GetPlanetStorageLevel(zone.name);
            item.productLevelLabel.text = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000017), (productLevel + 1).ToString());
            item.storageLevelLabel.text = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000018), (storageLevel + 1).ToString());
            item.nationLabel.text = zone.kNation.kName.ToString();
        }
    }

    public void OnPlanetClick(GameObject _self)
    {
        int index = int.Parse(_self.name.Replace("Item", "")) - 1;
        if (index >= ZoneManager.Instance.kPlanetZoneList.Count)
            return;

        Zone zone = ZoneManager.Instance.kPlanetZoneList[index];
        ZoneManager.Instance.zoneSelect = zone;
    }

    public void OnProductUpgradeClick(GameObject _self)
    {
        int index = int.Parse(_self.name.Replace("Item", "")) - 1;
        if (index >= ZoneManager.Instance.kPlanetZoneList.Count)
            return;

        Zone zone = ZoneManager.Instance.kPlanetZoneList[index];
        ZoneManager.Instance.zoneSelect = zone;

        DT_PlanetData_Info info = CDT_PlanetData_Manager.Instance.GetInfo(zone.kPlanetID);

        string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000007), EditDef.PLANET_UPGRAGE_ABILITY_PERCENT.ToString());

        int level = GameData.Local.GetPlanetProductLevel(zone.name);
        float upgradePercent = 100 + (level + 1) * EditDef.PLANET_UPGRAGE_ABILITY_PERCENT;
        msg = StringUtil.MacroString(msg, upgradePercent.ToString());

        int cost = info.UpgradeCost + (int)(level * EditDef.PLANET_UPGRAGE_COST_PERCENT * CommonDef.TO_PERCENT_UNIT * info.UpgradeCost);
        msg = StringUtil.MacroString(msg, cost.ToString());
        MessageBox.Open(msg, ProductUpdrage, null);
    }

    public void ProductUpdrage()
    {
        int level = GameData.Local.GetPlanetProductLevel(ZoneManager.Instance.zoneSelect.name) + 1;
        GameData.Local.SetPlanetProductLevel(ZoneManager.Instance.zoneSelect.name, level);

        string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000019), (level + 1).ToString());
        MessageBox.Open(msg, null);

        for (int i = 0; i < ZoneManager.Instance.kPlanetZoneList.Count; i++)
        {
            Zone zone = ZoneManager.Instance.kPlanetZoneList[i];
            if (zone == ZoneManager.Instance.zoneSelect)
            {
                ListItem item = mListItemList[i];
                item.productLevelLabel.text = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000017), (level+1).ToString());
                break;
            }
        }

        ZoneManager.Instance.zoneSelect.SetResource();
    }

    public void OnStorageUpgradeClick(GameObject _self)
    {
        int index = int.Parse(_self.name.Replace("Item", "")) - 1;
        if (index >= ZoneManager.Instance.kPlanetZoneList.Count)
            return;

        Zone zone = ZoneManager.Instance.kPlanetZoneList[index];
        ZoneManager.Instance.zoneSelect = zone;

        DT_PlanetData_Info info = CDT_PlanetData_Manager.Instance.GetInfo(zone.kPlanetID);

        string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000008), EditDef.PLANET_UPGRAGE_ABILITY_PERCENT.ToString());

        int level = GameData.Local.GetPlanetStorageLevel(zone.name);
        float upgradePercent = 100 + (level + 1) * EditDef.PLANET_UPGRAGE_ABILITY_PERCENT;
        msg = StringUtil.MacroString(msg, upgradePercent.ToString());

        int cost = info.UpgradeCost + (int)(level * EditDef.PLANET_UPGRAGE_COST_PERCENT * CommonDef.TO_PERCENT_UNIT * info.UpgradeCost);
        msg = StringUtil.MacroString(msg, cost.ToString());
        MessageBox.Open(msg, StorageUpdrage, null);
    }

    public void StorageUpdrage()
    {
        int level = GameData.Local.GetPlanetStorageLevel(ZoneManager.Instance.zoneSelect.name) + 1;
        GameData.Local.SetPlanetStorageLevel(ZoneManager.Instance.zoneSelect.name, level);

        string msg = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000020), (level+1).ToString());
        MessageBox.Open(msg, null);

        for (int i = 0; i < ZoneManager.Instance.kPlanetZoneList.Count; i++)
        {
            Zone zone = ZoneManager.Instance.kPlanetZoneList[i];
            if (zone == ZoneManager.Instance.zoneSelect)
            {
                ListItem item = mListItemList[i];
                item.storageLevelLabel.text = StringUtil.MacroString(LocalizationManager.Instance.GetLocalValue(3000018), (level+1).ToString());
                break;
            }
        }

        ZoneManager.Instance.zoneSelect.SetResource();
    }

    public void OnDetailClick(GameObject _self)
    {
        int index = int.Parse(_self.name.Replace("Item", "")) - 1;
        if (index >= ZoneManager.Instance.kPlanetZoneList.Count)
            return;

        Zone zone = ZoneManager.Instance.kPlanetZoneList[index];
        ZoneManager.Instance.zoneSelect = zone;
    }

    public void OnClickClose()
    {
        LobbyUIRoot.Instance.SetMenu(LobbyEnum.MenuSelect.WorldMap);
    }
}
