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
    public UIWorldMap kWorldMap;

    UIBase [] mUIListArray;

    void Awake()
    {
        Instance = this;

        mCamera = transform.Find("Camera").GetComponent<Camera>();

        kUnitList           = mCamera.transform.Find("CenterAnchor/UnitList").GetComponentInChildren<UIUnitList>(true);
        kMainMenu           = mCamera.transform.Find("CenterAnchor/MainMenu").GetComponentInChildren<UIMainMenu>(true);
        kTacticsEditMenu    = mCamera.transform.Find("CenterAnchor/TacticsEditMenu").GetComponentInChildren<UITacticsEditMenu>(true);
        kUnitDetailInfo     = mCamera.transform.Find("CenterAnchor/UnitDetailInfo").GetComponentInChildren<UIUnitDetailInfo>(true);
        kWorldMap           = mCamera.transform.Find("CenterAnchor/WorldMap").GetComponentInChildren<UIWorldMap>(true);

        kTacticsEditMenu.gameObject.SetActive(false);
        kUnitList.gameObject.SetActive(false);
        kUnitDetailInfo.gameObject.SetActive(false);

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
        for (int i = 0; i < mUIListArray.Length; i++)
            mUIListArray[i].gameObject.SetActive(false);
        
        switch(_selectMenu)
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
                kWorldMap.gameObject.SetActive(true);
                break;
        }
    }
}
