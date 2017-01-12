using UnityEngine;
using System.Collections;
using LobbyEnum;

public class UIMainMenu : UIBase {
    TweenPosition mBattleTween;
    TweenPosition mPlanetTween;
    TweenPosition mCrusaderTween;

    TweenPosition mTacticsTween;
    TweenPosition mListTween;
    TweenPosition mStoreTween;

    void Awake()
    {        
        mBattleTween = transform.Find("BattleButton").GetComponent<TweenPosition>();
        mPlanetTween = transform.Find("PlanetButton").GetComponent<TweenPosition>();
        mCrusaderTween = transform.Find("CrusaderButton").GetComponent<TweenPosition>();

        mTacticsTween = transform.Find("TacticsButton").GetComponent<TweenPosition>();
        mListTween = transform.Find("UnitListButton").GetComponent<TweenPosition>();
        mStoreTween = transform.Find("StoreButton").GetComponent<TweenPosition>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickTactics()
    {
        LobbyManager.Instance.SetMenu(MenuSelect.Tactics);
    }

    public void OnClickUnitList()
    {
        LobbyManager.Instance.SetMenu(MenuSelect.UnitList);
    }

    public void OnClickWorlMap()
    {
        LobbyManager.Instance.SetMenu(MenuSelect.WorldMap);
    }

    public void OnClickStore()
    {

    }

    public void OnClickOption()
    {
        CommonUIRoot.Instance.kOption.gameObject.SetActive(true);
    }

    public void OnClickBattle()
    {
        SceneLoadManager.Instance.SetLoadScene(CommonEnum.SceneState.Stage);
    }

    public override void OnEnableAnimation()
    {
        mBattleTween.ResetToBeginning();
        mBattleTween.PlayForward();
        mPlanetTween.ResetToBeginning();
        mPlanetTween.PlayForward();
        mCrusaderTween.ResetToBeginning();
        mCrusaderTween.PlayForward();
        mPlanetTween.ResetToBeginning();
        mPlanetTween.PlayForward();

        mTacticsTween.ResetToBeginning();
        mTacticsTween.PlayForward();
        mListTween.ResetToBeginning();
        mListTween.PlayForward();
        mStoreTween.ResetToBeginning();
        mStoreTween.PlayForward();
    }
}
