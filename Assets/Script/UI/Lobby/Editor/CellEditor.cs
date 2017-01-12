using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Cell), true)]
public class CellEditor : Editor
{
    string [] mCellObjectArr = {"BluePlanet", "BrownPlanet", "GrayPlanet", "GreenSatellite", "MixBrownPlanet", "PurplePlanet", "RedPlanet", "WhitePlanet", "WhiteSatellite"};

    Cell _this;

    void OnEnable()
    {
        // target은 Editor의 멤버 변수으로 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객처에 대한 
        // 레퍼런스 object형이므로 실제 클라스(타입)으로 캐스팅해서 명확하게 해서 사용하기 용이하게한다.

        _this = target as Cell;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        
        if (GUILayout.Button("Object Creat") == true)
        {
            if (_this.kLandMark == Cell.LandMark.None)
                return;
            string resourceName = "Prefabs/CellObject/" + _this.kLandMark.ToString() + ".prefab";
            Object obj = AssetDatabase.LoadAssetAtPath<Object>("Assets/Prefabs/CellObject/" + _this.kLandMark.ToString() + ".prefab");
            GameObject gameObj = Instantiate(obj) as GameObject;
            gameObj.transform.parent = _this.transform;
            gameObj.transform.localPosition = Vector3.zero;
            gameObj.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);
        }
    }
}
