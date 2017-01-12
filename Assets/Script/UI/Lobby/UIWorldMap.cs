using UnityEngine;
using System.Collections;

public class UIWorldMap : UIBase {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickMainMenu()
    {
        LobbyManager.Instance.SetMenu(LobbyEnum.MenuSelect.Main);
    }
}
