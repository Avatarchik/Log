using UnityEngine;
using System.Collections;

public class UIOption : UIBase {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }
}
