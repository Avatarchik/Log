using UnityEngine;
using System.Collections;

public class UIPausePopup : UIBase {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickOption()
    {
        CommonUIRoot.Instance.kOption.gameObject.SetActive(true);
    }

    public void OnClickReturnToLobby()
    {
        SceneLoadManager.Instance.SetLoadScene(CommonEnum.SceneState.Lobby);
    }

    public void OnClickScreenshot()
    {
        
    }

    public void OnClickReturnToPlay()
    {
        Time.timeScale = GameData.Local.gameSpeed;
        gameObject.SetActive(false);
    }
}
