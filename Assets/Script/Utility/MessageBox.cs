using UnityEngine;
using System.Collections;

public class MessageBox
{
    public enum Type
    {
        YES_NO,
        COMFRIM
    }

    public static void Open(int _localID, UIMessageBox.ButtonEventFunc _func)
    {
        CommonUIRoot.Instance.kMessageBox.SetConfirm(LocalizationManager.Instance.GetLocalValue(_localID), _func);
    }

    public static void Open(int _localID, UIMessageBox.ButtonEventFunc _yesFunc, UIMessageBox.ButtonEventFunc _noFunc)
    {
        CommonUIRoot.Instance.kMessageBox.SetYesNo(LocalizationManager.Instance.GetLocalValue(_localID), _yesFunc, _noFunc);
    }
}
