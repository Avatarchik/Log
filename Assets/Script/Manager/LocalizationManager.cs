using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalizationManager : SingletonT<LocalizationManager>
{
    public enum LanguageT
    {
        KO = 0,
        JP,
        EN
    }
    
    private LanguageT mCurrentLanguageT = LanguageT.KO;
    
    public LanguageT GetSetLanguageType()
    {
        return mCurrentLanguageT;
    }

    public void SetLanguageType(LanguageT _language)
    {
        mCurrentLanguageT = _language;
    }


    private void init()
    {
        int iLocale = LocalData.Instance.language;
        if (iLocale == 0)
            mCurrentLanguageT = LanguageT.JP;
        else
            mCurrentLanguageT = LanguageT.KO;
    }
    /*
    public void SetLocalString(UILabel _label, int _ikey, DataType _type = DataType.General)
    {
        switch(_type)
        {
            case DataType.General:
                {
                    if (CDT_LocalizingData_Manager.Instance.GetInfo(_ikey) != null)
                    {
                        if (mCurrentLanguageT == LanguageT.KO)
                        {
                            _label.text = CDT_LocalizingData_Manager.Instance.GetInfo(_ikey).KO;
                        }

                        if (mCurrentLanguageT == LanguageT.JP)
                        {
                            _label.text = CDT_LocalizingData_Manager.Instance.GetInfo(_ikey).JP;
                        }

                        if (mCurrentLanguageT == LanguageT.EN)
                        {
                            _label.text = CDT_LocalizingData_Manager.Instance.GetInfo(_ikey).EN;
                        }
                    }
                }
                break;
            case DataType.Tutorial:
                {
                    if (CDT_TutorialDialog_Manager.Instance.GetInfo(_ikey) != null)
                    {
                        if (mCurrentLanguageT == LanguageT.KO)
                        {
                            _label.text = CDT_TutorialDialog_Manager.Instance.GetInfo(_ikey).KO;
                        }

                        if (mCurrentLanguageT == LanguageT.JP)
                        {
                            _label.text = CDT_TutorialDialog_Manager.Instance.GetInfo(_ikey).JP;
                        }

                        if (mCurrentLanguageT == LanguageT.EN)
                        {
                            _label.text = CDT_TutorialDialog_Manager.Instance.GetInfo(_ikey).EN;
                        }
                    }
                }
                break;
        }
    }
    */
    public string GetLocalValue(int _ikey)
    {
        string strValue = "";
        
        if (CDT_LocalizingData_Manager.Instance.GetInfo(_ikey) != null)
        {
            if (mCurrentLanguageT == LanguageT.KO)
            {
                return CDT_LocalizingData_Manager.Instance.GetInfo(_ikey).KO;
            }

            if (mCurrentLanguageT == LanguageT.JP)
            {
                return CDT_LocalizingData_Manager.Instance.GetInfo(_ikey).JP;
            }

            if (mCurrentLanguageT == LanguageT.EN)
            {
                return CDT_LocalizingData_Manager.Instance.GetInfo(_ikey).EN;
            }
        }
        
        return strValue;
    }
}
