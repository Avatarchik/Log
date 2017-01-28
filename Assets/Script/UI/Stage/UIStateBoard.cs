using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIStateBoard : MonoBehaviour {    
    
    UILabel mStageNoticeLabel;

    TweenScale mStageNoticeTween;

    UISprite mPlayerGroupSpr;
    UISprite mEnemyGroupSpr;

    List<Ship> mPlayerShipList;
    List<Ship> mEnemyShipList;

    UILabel mRecordTimeLabel;
    UILabel mCurrentTimeLabel;
    UILabel mNewRecordTimeLabel;

    void Awake()
    {
        mPlayerGroupSpr = transform.Find("State/PlayerProgress/Foreground").GetComponent<UISprite>();
        mEnemyGroupSpr = transform.Find("State/EnemyProgress/Foreground").GetComponent<UISprite>();

        mRecordTimeLabel = transform.Find("State/Time/RecordTimeLabel").GetComponent<UILabel>();
        mCurrentTimeLabel = transform.Find("State/Time/CurrentTimeLabel").GetComponent<UILabel>();
        mNewRecordTimeLabel = transform.Find("State/Time/NewRecordLabel").GetComponent<UILabel>();
        mNewRecordTimeLabel.gameObject.SetActive(false);

        mStageNoticeLabel = transform.Find("StageNotice/Label").GetComponent<UILabel>();
        mStageNoticeTween = transform.Find("StageNotice/Label").GetComponent<TweenScale>();
    }

    public void OnPrepare()
    {
        mPlayerShipList = StagePlayManager.Instance.kPlayerShipList;
        mEnemyShipList = StagePlayManager.Instance.kEnemyShipList;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        TimeProgressUpdate();
        GroupProgressUpdate();
    }
    
    public void OnClickGameRestart()
    {
        StageUIRoot.Instance.StageClear();
        StagePlayManager.Instance.GameStart();
    }
    
    public void StageClearTimeUpdate(int _stageIndex)
    {
        float time = GameData.Local.GetClearTime(_stageIndex);

        if( time == 0.0f)
        {
            mRecordTimeLabel.text = "-' --\"";
        }
        else
        {
            float curTime = time;
            int overNum = (int)curTime;
            int underNum = (int)((curTime - (float)overNum) * 100.0f);

            string sec = StringUtil.TwoMix(overNum.ToString(), "' ");
            string ms = StringUtil.TwoMix(StringUtil.FloatTo2FrontString(underNum), "\"");
            
            mRecordTimeLabel.text = sec + ms;
        }
    }

    public void NewRecord()
    {
        mNewRecordTimeLabel.gameObject.SetActive(true);
    }

    public void Refresh()
    {
        mStageNoticeLabel.text = StringUtil.TwoMix("Stage ", StagePlayManager.Instance.kCurStageNumber.ToString());
        mStageNoticeTween.ResetToBeginning();
        mStageNoticeTween.PlayForward();

        mNewRecordTimeLabel.gameObject.SetActive(false);

        StageClearTimeUpdate(StagePlayManager.Instance.kCurStageNumber);
    }

    public void TimeProgressUpdate()
    {
        float curTime = StagePlayManager.Instance.kCurStagePlayTime;
        int overNum = (int)curTime;
        int underNum = (int)((curTime - (float)overNum) * 100.0f);

        string sec = StringUtil.TwoMix(overNum.ToString(), "' ");
        string ms = StringUtil.TwoMix(StringUtil.FloatTo2FrontString(underNum), "\"");

        mCurrentTimeLabel.text = sec + ms;
    }

    public void GroupProgressUpdate()
    {
        if (mPlayerShipList == null || mEnemyShipList == null)
            return;
                
        int curPlayer = 0;
        for (int i = 0; i < mPlayerShipList.Count; i++ )
            curPlayer += mPlayerShipList[i].kCurHealthPoint + mPlayerShipList[i].kCurShieldPoint;
                
        int curEnemy = 0;
        for (int i = 0; i < mEnemyShipList.Count; i++)
            curEnemy += mEnemyShipList[i].kCurHealthPoint + mEnemyShipList[i].kCurShieldPoint;

        mPlayerGroupSpr.fillAmount = (float)curPlayer / (float)StagePlayManager.Instance.kPlayerTotalArmor;
        mEnemyGroupSpr.fillAmount = (float)curEnemy / (float)StagePlayManager.Instance.kEnemyTotalArmor;
    }
}
