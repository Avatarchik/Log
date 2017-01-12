using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Weapon), true)]
public class WeaponEditor : Editor
{
    Weapon _this;

    void OnEnable()
    {
        // target은 Editor의 멤버 변수으로 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객처에 대한 
        // 레퍼런스 object형이므로 실제 클라스(타입)으로 캐스팅해서 명확하게 해서 사용하기 용이하게한다.

        _this = target as Weapon;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        if (GUILayout.Button("Add Launcher") == true)
        {
            bool isNotAdd = true;
            int index = 0;
            while (isNotAdd)
            {
                if( _this.transform.Find("Launcher" + index.ToString()) == null )
                {
                    GameObject launcher = new GameObject();
                    launcher.transform.parent = _this.transform;
                    launcher.transform.localPosition = Vector3.zero;
                    launcher.name = "Launcher" + index.ToString();
                    isNotAdd = false;
                }

                index++;
            }                
        }
    }
}
