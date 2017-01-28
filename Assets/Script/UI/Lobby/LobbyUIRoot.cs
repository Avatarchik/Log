using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LobbyEnum;

public class LobbyUIRoot : SequenceController {
    public static LobbyUIRoot Instance;
    Camera mCamera;

    [HideInInspector]
    public UIUnitList kUnitList;
    [HideInInspector]
    public UIMainMenu kMainMenu;
    [HideInInspector]
    public UITacticsEditMenu kTacticsEditMenu;
    [HideInInspector]
    public UIUnitDetailInfo kUnitDetailInfo;
    [HideInInspector]
    public UIUnconqueredZone kUnconqueredZone;
    [HideInInspector]
    public UIConqueredZone kConqueredZone;
    [HideInInspector]
    public UIConqueredList kConqueredList;
    [HideInInspector]
    public UIResourceInfo kResourceInfo;
    [HideInInspector]
    public UIBattleLog kBattleLog;
    [HideInInspector]
    public UIUserInfo kUserInfo;

    [HideInInspector]
    public UIWorldMap kWorldMap;

    UIBase [] mUIListArray;

    public MenuSelect kCurSelectMenu = MenuSelect.None;

    void Awake()
    {
        Instance = this;

        mCamera = transform.Find("Camera").GetComponent<Camera>();

        kUnitList           = mCamera.transform.Find("CenterAnchor/UnitList").GetComponentInChildren<UIUnitList>(true);
        kMainMenu           = mCamera.transform.Find("CenterAnchor/MainMenu").GetComponentInChildren<UIMainMenu>(true);
        kTacticsEditMenu    = mCamera.transform.Find("CenterAnchor/TacticsEditMenu").GetComponentInChildren<UITacticsEditMenu>(true);
        kUnitDetailInfo     = mCamera.transform.Find("CenterAnchor/UnitDetailInfo").GetComponentInChildren<UIUnitDetailInfo>(true);
        kWorldMap           = mCamera.transform.Find("CenterAnchor/WorldMap").GetComponentInChildren<UIWorldMap>(true);
        kUnconqueredZone    = mCamera.transform.Find("CenterAnchor/UnconqueredZone").GetComponentInChildren<UIUnconqueredZone>(true);
        kConqueredZone      = mCamera.transform.Find("CenterAnchor/ConqueredZone").GetComponentInChildren<UIConqueredZone>(true);
        kConqueredList      = mCamera.transform.Find("CenterAnchor/ConqueredList").GetComponentInChildren<UIConqueredList>(true);
        kUserInfo           = mCamera.transform.Find("TopAnchor/UserInfo").GetComponentInChildren<UIUserInfo>(true);
        kResourceInfo       = mCamera.transform.Find("CenterAnchor/ResourceInfo").GetComponentInChildren<UIResourceInfo>(true);
        kBattleLog          = mCamera.transform.Find("CenterAnchor/BattleLog").GetComponentInChildren<UIBattleLog>(true);

        kTacticsEditMenu.gameObject.SetActive(false);
        kUnitList.gameObject.SetActive(false);
        kUnitDetailInfo.gameObject.SetActive(false);
        kUnconqueredZone.gameObject.SetActive(false);

        mUIListArray = GetComponentsInChildren<UIBase>(true);
    }

    public override void OnPrepare()
    {
        EasyTouch.instance.nGUICameras.Add(mCamera);
        kTacticsEditMenu.OnPrepare();
        kUnitList.OnPrepare();
    }

    // Use this for initialization
    public override void OnStart()
    {

    }

    // Update is called once per frame
    public override void OnUpdate()
    {

    }

    public void SetMenu(MenuSelect _selectMenu)
    {
        if (kCurSelectMenu == _selectMenu)
            return;

        for (int i = 0; i < mUIListArray.Length; i++)
            mUIListArray[i].gameObject.SetActive(false);

        kUserInfo.gameObject.SetActive(true);

        switch (_selectMenu)
        {
            case MenuSelect.UnitList:
                kUnitList.gameObject.SetActive(true);
                break;
            case MenuSelect.UnitDetail:
                kUnitDetailInfo.gameObject.SetActive(true);
                break;
            case MenuSelect.Main:
                kMainMenu.gameObject.SetActive(true);
                break;
            case MenuSelect.Tactics:
                kTacticsEditMenu.gameObject.SetActive(true);
                break;
            case MenuSelect.WorldMap:
                {
                    kWorldMap.gameObject.SetActive(true);
                    ZoneManager.Instance.zoneSelect = null;
                    
                    if ( WorldCamera.Instance.kIsSwipe == false)
                    {
                        if (kCurSelectMenu == MenuSelect.UnconqueredZone ||
                            kCurSelectMenu == MenuSelect.ConqueredZone ||
                            kCurSelectMenu == MenuSelect.ConqueredList ||
                            kCurSelectMenu == MenuSelect.ResourceInfo)
                                WorldCamera.Instance.CenterView();
                    }

                    WorldCamera.Instance.kIsSwipe = false;
                    WorldCamera.Instance.kCurrentMode = WorldCamera.Mode.Center;
                }
                break;
            case MenuSelect.UnconqueredZone:
                {
                    kWorldMap.gameObject.SetActive(true);
                    kWorldMap.SideView();
                    kUnconqueredZone.gameObject.SetActive(true);
                }
                break;
            case MenuSelect.ConqueredZone:
                {
                    kWorldMap.gameObject.SetActive(true);
                    kWorldMap.SideView();
                    kConqueredZone.gameObject.SetActive(true);
                    kConqueredZone.Refresh();
                }
                break;
            case MenuSelect.ConqueredList:
                {
                    ZoneManager.Instance.zoneSelect = null;

                    kWorldMap.gameObject.SetActive(true);
                    kWorldMap.ToResourceMode();
                    kWorldMap.SideView();

                    WorldCamera.Instance.SideView();                    
                    kConqueredList.gameObject.SetActive(true);
                }
                break;
        }

        kCurSelectMenu = _selectMenu;
    }
}
