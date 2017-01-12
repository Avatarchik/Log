using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Nation), true)]
public class NationEditor : Editor
{
    Nation _this;

    void OnEnable()
    {
        // target은 Editor의 멤버 변수으로 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객처에 대한 
        // 레퍼런스 object형이므로 실제 클라스(타입)으로 캐스팅해서 명확하게 해서 사용하기 용이하게한다.

        _this = target as Nation;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.BeginVertical();
        
    }
}
