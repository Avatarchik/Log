using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LobbyEnum;
using UnityEngine.SceneManagement;

public class LobbyManager : SingletonG<LobbyManager>{
    enum WorldMenu
    {
        None,
        Empty,
        Main,
        Tactics,
    }

    List<SequenceController> mSequenceControllerList = new List<SequenceController>();

    [HideInInspector]
    public TacticsBoard kTacticsBoard = null;
    [HideInInspector]
    public LobbyBoard kLobbyBoard = null;
    [HideInInspector]
    public GameObject kUnitBoard = null;

    [HideInInspector]
    public LobbyCamera kLobbyCamera = null;
    [HideInInspector]
    public UnitCamera kUnitCamera = null;
    [HideInInspector]
    public TacticsCamera kTacticsCamera = null;
    [HideInInspector]
    public WorldCamera kWorldCamera = null;
    [HideInInspector]
    public GameObject kLobbySpaceFog = null;

    [HideInInspector]
    public MenuSelect kCurrentMenu = MenuSelect.None;
    WorldMenu kCurrentWorldMenu = WorldMenu.None;

    void Awake()
    {   
    }

	// Use this for initialization
	void Start () {
        SequenceController controller = null;
        controller = LobbyUIRoot.Instance.GetComponent<SequenceController>();
        mSequenceControllerList.Add(controller);
        controller = WorldUIRoot.Instance.GetComponent<SequenceController>();
        mSequenceControllerList.Add(controller);
        controller = ZoneManager.Instance.GetComponent<SequenceController>();
        mSequenceControllerList.Add(controller);

        StartCoroutine(SequenceControl());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator SequenceControl()
    {
        if (CommonManager.Loaded == false)
        {
            CommonManager.Title = false;
            SceneManager.LoadScene("Common", LoadSceneMode.Additive);
        }
        
        while (CommonManager.Loaded == false)
            yield return null;

        SequenceController controller = ObjectPoolManager.Instance.GetComponent<SequenceController>();
        mSequenceControllerList.Add(controller);

        for (int i = 0; i < mSequenceControllerList.Count; i++)
        {
            controller = mSequenceControllerList[i];

            while (controller.IsStartDone() == false)
                yield return null;
        }

        CommonUIRoot.Instance.SetTouchCamera();
        CommonUIRoot.Instance.kLoading.SetPercent(60);

        yield return null;

        while (kTacticsBoard == null)
        {
            GameObject getGameObject = GameObject.Find("TacticsBoard");
            if (null != getGameObject)
                kTacticsBoard = getGameObject.transform.GetComponent<TacticsBoard>();
            else
                yield return null;
        }
                
        while (kLobbyBoard == null)
        {
            GameObject getGameObject = GameObject.Find("LobbyBoard");
            if (null != getGameObject)
                kLobbyBoard = getGameObject.transform.GetComponent<LobbyBoard>();
            else
                yield return null;
        }

        while (kUnitBoard == null)
        {
            GameObject getGameObject = GameObject.Find("UnitBoard");
            if (null != getGameObject)
                kUnitBoard = getGameObject;
            else
                yield return null;
        }
                
        while (kLobbyCamera == null)
        {
            GameObject getGameObject = GameObject.Find("LobbyCamera");
            if (null != getGameObject)
                kLobbyCamera = getGameObject.transform.GetComponent<LobbyCamera>();
            else
                yield return null;
        }

        while (kWorldCamera == null)
        {
            GameObject getGameObject = WorldUIRoot.Instance.transform.Find("Camera").gameObject;
            if (null != getGameObject)
                kWorldCamera = getGameObject.transform.GetComponent<WorldCamera>();
            else
                yield return null;
        }
        
        while (kTacticsCamera == null)
        {
            GameObject getGameObject = GameObject.Find("TacticsCamera");
            if (null != getGameObject)
                kTacticsCamera = getGameObject.transform.GetComponent<TacticsCamera>();
            else
                yield return null;
        }

        while (kUnitCamera == null)
        {
            GameObject getGameObject = GameObject.Find("UnitCamera");
            if (null != getGameObject)
                kUnitCamera = getGameObject.transform.GetComponent<UnitCamera>();
            else
                yield return null;
        }

        while (kLobbySpaceFog == null)
        {
            GameObject getGameObject = GameObject.Find("Environment");
            if (null != getGameObject)
                kLobbySpaceFog = getGameObject;
            else
                yield return null;
        }

        CommonUIRoot.Instance.kLoading.SetPercent(70);
        yield return null;

        for (int i = 0; i < mSequenceControllerList.Count; i++)
        {
            controller = mSequenceControllerList[i];
            controller.Prepare();
        }

        SetMenu(GameData.Lobby.kSelectMenu);
        yield return null;

        CommonUIRoot.Instance.kLoading.SetPercent(100);
        yield return null;
    }
    
    public void SetMenu(MenuSelect _selectMenu)
    {        
        if (kCurrentMenu == _selectMenu)
            return;

        LobbyUIRoot.Instance.SetMenu(_selectMenu);
        
        WorldMenu selectWorld = GetWorld(_selectMenu);
        WorldMenu curWorld = GetWorld(kCurrentMenu);
        bool kIsChangeWorld = false;
        if (selectWorld != curWorld)
            kIsChangeWorld = true;

        if (kIsChangeWorld == true || selectWorld == WorldMenu.Empty)
        {
            kTacticsBoard.gameObject.SetActive(false);
            kTacticsCamera.gameObject.SetActive(false);            
            kLobbySpaceFog.gameObject.SetActive(false);
            kLobbyBoard.gameObject.SetActive(false);
            kLobbyCamera.gameObject.SetActive(false);
            
            ObjectPoolManager.Instance.AllOutPoolCollect();

            if(kCurrentMenu != MenuSelect.UnitList)
                LobbyUIRoot.Instance.kUnitList.SelectClear();
        }

        kWorldCamera.gameObject.SetActive(false);
        kUnitCamera.gameObject.SetActive(false);

        switch (_selectMenu)
        {
            case MenuSelect.Main:
                {
                    if (kIsChangeWorld == true)
                    { 
                        kLobbyBoard.OnPrepare();
                        kLobbyCamera.kIsCinemaView = true;
                        kLobbyCamera.gameObject.SetActive(true);
                        kLobbySpaceFog.gameObject.SetActive(true);
                    }
                    
                    SoundManager.Instance.BGMVolume(1.0f);
                    SoundManager.Instance.PlayBGM("shonan_outside");
                }
                break;
            case MenuSelect.UnitList:
                {
                    SoundManager.Instance.BGMVolume(0.3f);
                }
                break;
            case MenuSelect.UnitDetail:
                {
                    kUnitCamera.gameObject.SetActive(true);
                    LobbyUIRoot.Instance.kUnitDetailInfo.OnPrepare();
                }
                break;
            case MenuSelect.Tactics:
                {                    
                    if (kIsChangeWorld == true)
                    {
                        kTacticsBoard.OnPrepare();
                        kTacticsCamera.gameObject.SetActive(true);
                    }

                    SoundManager.Instance.BGMVolume(0.3f);
                }
                break;
            case MenuSelect.WorldMap:
                {
                    kLobbyCamera.kIsCinemaView = false;
                    kLobbyCamera.gameObject.SetActive(true);
                    kWorldCamera.gameObject.SetActive(true);
                    SoundManager.Instance.BGMVolume(0.3f);
                }
                break;
        }
        
        kCurrentMenu = _selectMenu;
        kCurrentWorldMenu = selectWorld;
    }

    WorldMenu GetWorld(MenuSelect _select)
    {
        switch(_select)
        {
            case MenuSelect.Main:
                return WorldMenu.Main;
            case MenuSelect.None:
                return WorldMenu.Empty;
            case MenuSelect.Tactics:
                return WorldMenu.Tactics;
            case MenuSelect.UnitList:
                return kCurrentWorldMenu;
            case MenuSelect.UnitDetail:
                return kCurrentWorldMenu;
            case MenuSelect.WorldMap:
                return WorldMenu.Empty;
        }

        return WorldMenu.None;
    }
}
