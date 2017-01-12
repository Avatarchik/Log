using UnityEngine;
using System.Collections;

public class UIControlMenu : UIBase {
    bool mIsGamePause = false;
    //
    UILabel mGameSpeedLabel;

    void Awake()
    {
        mGameSpeedLabel = transform.Find("SpeedChangeButton/Label").GetComponent<UILabel>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickPause()
    {
        StageUIRoot.Instance.kPausePopup.gameObject.SetActive(true);
        Time.timeScale = 0.0f;        
    }

    public void OnClickGameSpeedChange()
    {
        GameData.Local.gameSpeed += 0.5f;
        if (GameData.Local.gameSpeed > 2.0f)
            GameData.Local.gameSpeed = 1.0f;

        GameSpeedUpdate();
    }

    public void GameSpeedUpdate()
    {
        Time.timeScale = GameData.Local.gameSpeed;
        string pow = StringUtil.FloatToString(GameData.Local.gameSpeed, 1);
        mGameSpeedLabel.text = StringUtil.TwoMix("x ", pow);
    }

    public void Skill1Active()
    {

    }

    public void Skill2Active()
    {

    }

    public void Skill3Active()
    {

    }
}
