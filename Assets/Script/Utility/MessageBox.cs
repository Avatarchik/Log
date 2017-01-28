using UnityEngine;
using System.Collections;
using CommonEnum;

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

    public static void Open(string _msg, UIMessageBox.ButtonEventFunc _func)
    {
        CommonUIRoot.Instance.kMessageBox.SetConfirm(_msg, _func);
    }

    public static void Open(int _localID, UIMessageBox.ButtonEventFunc _yesFunc, UIMessageBox.ButtonEventFunc _noFunc)
    {
        CommonUIRoot.Instance.kMessageBox.SetYesNo(LocalizationManager.Instance.GetLocalValue(_localID), _yesFunc, _noFunc);
    }

    public static void Open(string _msg, UIMessageBox.ButtonEventFunc _yesFunc, UIMessageBox.ButtonEventFunc _noFunc)
    {
        CommonUIRoot.Instance.kMessageBox.SetYesNo(_msg, _yesFunc, _noFunc);
    }

    public static void NotEnoughResource(ResourceType _type)
    {
        switch(_type)
        {
            case ResourceType.Cristal:
                CommonUIRoot.Instance.kMessageBox.SetConfirm(LocalizationManager.Instance.GetLocalValue(3000027), null);
                break;
            case ResourceType.Gold:
                CommonUIRoot.Instance.kMessageBox.SetConfirm(LocalizationManager.Instance.GetLocalValue(3000025), null);
                break;
            case ResourceType.Material:
                CommonUIRoot.Instance.kMessageBox.SetConfirm(LocalizationManager.Instance.GetLocalValue(3000026), null);
                break;
        }
    }
}
