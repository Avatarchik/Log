using UnityEngine;
using UnityEditor;
using System.Collections;
using CommonEnum;

[CustomEditor(typeof(LobbyManager), true)]
public class LobbyManagerEditor : Editor
{
    LobbyManager _this;

    void OnEnable()
    {
        // target은 Editor의 멤버 변수으로 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객처에 대한 
        // 레퍼런스 object형이므로 실제 클라스(타입)으로 캐스팅해서 명확하게 해서 사용하기 용이하게한다.

        _this = target as LobbyManager;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
    }    
}
