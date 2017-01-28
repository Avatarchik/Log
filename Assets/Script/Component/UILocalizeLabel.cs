using UnityEngine;
using System.Collections;

public class UILocalizeLabel : MonoBehaviour {

    public int Key = -1;

    // Use this for initialization
    void Start ()
    {
        OnLocalize();
    }

    public void OnLocalize()
    {
        if (Key == -1)
            return;
        
        var lable = GetComponent<UILabel>();
        if (lable)
        {
            LocalizationManager.Instance.SetLocalString(lable, Key);
        }
    }

    public void SetLocalizeText(string text)
    {
        var lable = GetComponent<UILabel>();
        if (lable)
        {
            lable.text = text;
        }
    }    
}
