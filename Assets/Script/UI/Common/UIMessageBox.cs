using UnityEngine;
using System.Collections;

public class UIMessageBox : UIBase
{
    public delegate void ButtonEventFunc();

    ButtonEventFunc mYesButtonEvent;
    ButtonEventFunc mNoButtonEvent;
    ButtonEventFunc mConfirmButtonEvent;

    UIButton mYesButton;
    UIButton mNoButton;
    UIButton mConfirmButton;

    UILabel mContentLabel;
    void Awake()
    {
        mYesButton = transform.Find("YesButton").GetComponent<UIButton>();
        mNoButton = transform.Find("NoButton").GetComponent<UIButton>();
        mConfirmButton = transform.Find("ConfirmButton").GetComponent<UIButton>();
        mContentLabel = transform.Find("ContentLabel").GetComponent<UILabel>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetConfirm(string _msg, ButtonEventFunc _func)
    {
        gameObject.SetActive(true);
        Clear();
                
        mConfirmButton.gameObject.SetActive(true);

        mContentLabel.text = _msg;

        mConfirmButtonEvent = _func;
    }

    public void SetYesNo(string _msg, ButtonEventFunc _yesFunc, ButtonEventFunc _noFunc)
    {
        gameObject.SetActive(true);
        Clear();

        mYesButton.gameObject.SetActive(true);
        mNoButton.gameObject.SetActive(true);        

        mContentLabel.text = _msg;

        mYesButtonEvent = _yesFunc;
        mNoButtonEvent = _noFunc;
    }

    void Clear()
    {
        mConfirmButtonEvent = null;
        mNoButtonEvent = null;
        mYesButtonEvent = null;

        mYesButton.gameObject.SetActive(false);
        mNoButton.gameObject.SetActive(false);
        mConfirmButton.gameObject.SetActive(false);
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }
    
    public void OnClickComfirm()
    {
        gameObject.SetActive(false);

        if (mConfirmButtonEvent != null)
            mConfirmButtonEvent();
    }

    public void OnClickYes()
    {
        gameObject.SetActive(false);

        if (mYesButtonEvent != null)
            mYesButtonEvent();
    }

    public void OnClickNo()
    {
        gameObject.SetActive(false);

        if (mNoButtonEvent != null)
            mNoButtonEvent();
    }
}
