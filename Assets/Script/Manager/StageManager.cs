using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StageEnum;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour {
    public static StageManager Instance = null;
    List<SequenceController> mSequenceControllerList = new List<SequenceController>();

    [HideInInspector]
    public StageCamera kStageCamera;
    [HideInInspector]
    public ShipBoard kPlayerShipBoard;
    [HideInInspector]
    public ShipBoard kEnemyShipBoard;
    [HideInInspector]
    public GameObject kStageSpaceFog;

    void Awake()
    {
        Instance = this;
        CDataManagerNavigator.Instance.Load();
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start () {
        

        SequenceController controller = null;
        controller = StageUIRoot.Instance.GetComponent<SequenceController>();
        mSequenceControllerList.Add(controller);
        controller = StagePlayManager.Instance.GetComponent<SequenceController>();
        mSequenceControllerList.Add(controller);
        //controller = UIManager.Instance.GetComponent<SequenceController>();
        //mSequenceControllerList.Add(controller);

        StartCoroutine("SequenceControl");
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

        for ( int i = 0; i < mSequenceControllerList.Count; i++ )
        {
            controller = mSequenceControllerList[i];

            while ( controller.IsStartDone() == false )
                yield return new WaitForSeconds(0.1f);
        }

        CommonUIRoot.Instance.kLoading.SetPercent(60);
        yield return null;
        
        if(kStageCamera == null)
        {
            GameObject getGameObject = GameObject.Find("StageCamera");
            if (null != getGameObject)
                kStageCamera = getGameObject.transform.GetComponent<StageCamera>();
            else
                yield return null;
        }

        if (kPlayerShipBoard == null)
        {
            GameObject getGameObject = GameObject.Find("PlayerBoard");
            if (null != getGameObject)
                kPlayerShipBoard = getGameObject.transform.GetComponent<ShipBoard>();
            else
                yield return null;
        }

        if (kEnemyShipBoard == null)
        {
            GameObject getGameObject = GameObject.Find("EnemyBoard");
            if (null != getGameObject)
                kEnemyShipBoard = getGameObject.transform.GetComponent<ShipBoard>();
            else
                yield return null;
        }

        if(kStageSpaceFog == null)
        {
            GameObject getGameObject = GameObject.Find("Environment/Fog");
            if (null != getGameObject)
                kStageSpaceFog = getGameObject;
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

        CommonUIRoot.Instance.kLoading.SetPercent(90);
        yield return null;

        SetMode(Mode.Battle);

        CommonUIRoot.Instance.kLoading.SetPercent(100);
        yield return null;
    }

    void SetMode(Mode _selectMode)
    {        
        switch (_selectMode)
        {
            case Mode.Battle:
                {
                    StagePlayManager.Instance.kMode = Mode.Battle;
                    StagePlayManager.Instance.kCurStageNumber = 10;
                    StagePlayManager.Instance.GameStart();
                }
                break;
        }
        
        StageUIRoot.Instance.SetMenu(_selectMode);
    }
}
