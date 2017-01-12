﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StageEnum;

public class StagePlayManager : SequenceController {
    public static StagePlayManager Instance = null;

    [HideInInspector]
    public List<Ship> kEnemyShipList = new List<Ship>();
    [HideInInspector]
    public List<Ship> kPlayerShipList = new List<Ship>();

    [HideInInspector]
    public int kEnemyTotalArmor = 0;
    [HideInInspector]
    public int kPlayerTotalArmor = 0;

    bool mIsEnemyEngage = false;
    bool mIsPlayerEngage = false;

    [HideInInspector]
    public int kCurStageNumber = 10;
    
    [HideInInspector]
    public float kCurStagePlayTime = 0.0f;

    public Mode kMode;
    // Use this for initialization
    void Awake () {
        Instance = this;
    }

    public void GameStart()
    {
        kEnemyShipList.Clear();
        kPlayerShipList.Clear();

        Time.timeScale = GameData.Local.gameSpeed;
        StageUIRoot.Instance.kContorlMenu.GameSpeedUpdate();

        switch (kMode)
        {
            case Mode.Battle:
                {
                    StageManager.Instance.kPlayerShipBoard.OnPrepare();
                    StageManager.Instance.kEnemyShipBoard.OnPrepare();

                    for (int i = 0; i < kPlayerShipList.Count; i++)
                        kPlayerShipList[i].OnPrepare();
                    for (int i = 0; i < kEnemyShipList.Count; i++)
                        kEnemyShipList[i].OnPrepare();

                    StageUIRoot.Instance.kStateBoard.Refresh();
                    SoundManager.Instance.BattleVoice(StageEnum.BattleSign.EnemyFind);
                    SoundManager.Instance.PlayBGM("JinglePunks");
                }
                break;            
        }

        kCurStagePlayTime = 0.0f;
    }

    // Update is called once per frame
    public override void OnStart()
    {

    }
    // Update is called once per frame
    public override void OnPrepare()
    {

    }

    //임시
    bool mIsCheckContinu = true;
    // Update is called once per frame
    public override void OnUpdate ()
    {
        if (kEnemyShipList.Count != 0)
            kCurStagePlayTime += Time.deltaTime;

        if (mIsCheckContinu == false)
            return;

        if( kMode == Mode.Battle )
        {
            if( kEnemyShipList.Count == 0 )
            {
                float clearTime = GameData.Local.GetClearTime(kCurStageNumber);
                if (clearTime == 0 || clearTime > kCurStagePlayTime)
                {
                    StageUIRoot.Instance.kStateBoard.NewRecord();
                    GameData.Local.SetClearTime(kCurStageNumber, kCurStagePlayTime);
                }

                kCurStageNumber++;
                Invoke("StageContinue", 2.0f);
                mIsCheckContinu = false;
            }
            if (kPlayerShipList.Count == 0)
            {
                Invoke("StageContinue", 2.0f);
                mIsCheckContinu = false;
            }
        }
    }

    void StageContinue()
    {
        kCurStagePlayTime       = 0.0f;

        mIsCheckContinu = true;
        StageUIRoot.Instance.StageClear();

        kEnemyShipList.Clear();
        kPlayerShipList.Clear();
        ObjectPoolManager.Instance.AllOutPoolCollect();

        StageManager.Instance.kPlayerShipBoard.OnPrepare();
        StageManager.Instance.kEnemyShipBoard.OnPrepare();

        kPlayerTotalArmor = 0;
        kEnemyTotalArmor = 0;

        for (int i = 0; i < kPlayerShipList.Count; i++)
            kPlayerShipList[i].OnPrepare();
        for (int i = 0; i < kEnemyShipList.Count; i++)
            kEnemyShipList[i].OnPrepare();

        StageUIRoot.Instance.kStateBoard.Refresh();
        SoundManager.Instance.BattleVoice(StageEnum.BattleSign.EnemyFind);
    }

    public bool IsEngage(bool _isPlayer)
    {
        if (_isPlayer == true)
            return mIsPlayerEngage;
        else
            return mIsEnemyEngage;
    }

    public void SeGroupEngage(Ship _ship)
    {
        if( kPlayerShipList.Contains(_ship) == true )
        {
            if (mIsPlayerEngage == true)
                return;

            SoundManager.Instance.BattleVoice(StageEnum.BattleSign.EnemyFind);

            mIsPlayerEngage = true;
            for (int i = 0; i < kPlayerShipList.Count; i++)
                kPlayerShipList[i].kIsEngage = true;
        }
        else
        {
            if (mIsEnemyEngage == true)
                return;

            mIsEnemyEngage = true;
            for (int i = 0; i < kEnemyShipList.Count; i++)
                kEnemyShipList[i].kIsEngage = true;
        }
    }    
}
