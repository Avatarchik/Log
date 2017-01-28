using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UILocalizeLabel), true)]
public class UILocalizeLabelEditor : Editor
{
    List<string> mKeys;
    List<string> mLang;

    void OnEnable()
    {
        UILocalizeLabel t = (UILocalizeLabel) target;

        int nkey = t.Key;

        mKeys = new List<string>();
        mLang = new List<string>();
        
        var dt = CDT_LocalizingData_Manager.Instance.GetInfo(nkey);
        if (dt != null)
        {
            mLang.Add("KO");
            mLang.Add("JP");
            mLang.Add("EN");

            mKeys.Add(dt.KO);
            mKeys.Add(dt.JP);
            mKeys.Add(dt.EN);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Space(6f);
        NGUIEditorTools.SetLabelWidth(80f);

        GUILayout.BeginHorizontal();
        // Key not found in the localization file -- draw it as a text field
        SerializedProperty sp = NGUIEditorTools.DrawProperty("Key", serializedObject, "Key");
        if (sp == null)
            return;


        //string myKey = sp.stringValue;
        string myKey = sp.intValue.ToString();

        bool isPresent = true;// (mKeys != null) && mKeys.Contains(myKey);
        GUI.color = isPresent ? Color.green : Color.red;
        GUILayout.BeginVertical(GUILayout.Width(22f));
        GUILayout.Space(2f);
        if( GUILayout.Button("적용") == true )
        {
            mKeys = new List<string>();
            mLang = new List<string>();
            UILocalizeLabel t = (UILocalizeLabel)target;
            var dt = CDT_LocalizingData_Manager.Instance.GetInfo(t.Key);
            if (dt != null)
            {
                mLang.Add("KO");
                mLang.Add("JP");
                mLang.Add("EN");

                mKeys.Add(dt.KO);
                mKeys.Add(dt.JP);
                mKeys.Add(dt.EN);
            }
        }

        //GUILayout.Label(isPresent ? "\u2714" : "\u2718", "TL SelectionButtonNew", GUILayout.Height(20f));
        GUILayout.EndVertical();
        GUI.color = Color.white;
        GUILayout.EndHorizontal();

        if (NGUIEditorTools.DrawHeader("Preview"))
        {
            NGUIEditorTools.BeginContents();

            for (int i = 0; i < mLang.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(mLang[i], GUILayout.Width(66f));

                if (GUILayout.Button(mKeys[i], "AS TextArea", GUILayout.MinWidth(80f), GUILayout.MaxWidth(Screen.width - 110f)))
                {
                    (target as UILocalizeLabel).SetLocalizeText(mKeys[i]);

                    GUIUtility.hotControl = 0;
                    GUIUtility.keyboardControl = 0;
                }
                GUILayout.EndHorizontal();
            }


            NGUIEditorTools.EndContents();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
