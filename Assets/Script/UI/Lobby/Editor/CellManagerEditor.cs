using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CellManager), true)]
public class CellManagerEditor : Editor
{
    CellManager _this;

    void OnEnable()
    {
        // target은 Editor의 멤버 변수으로 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객처에 대한 
        // 레퍼런스 object형이므로 실제 클라스(타입)으로 캐스팅해서 명확하게 해서 사용하기 용이하게한다.

        _this = target as CellManager;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.BeginVertical();

        _this.kRowCount = EditorGUILayout.IntField("Row Count", _this.kRowCount);
        _this.kColumnCount = EditorGUILayout.IntField("Column Count", _this.kColumnCount);

        EditorGUILayout.IntField("Cell Count ", _this.transform.childCount);

        EditorGUILayout.EndVertical();
        
        if (GUILayout.Button("Map Tile Creat") == true)
        {
            for (int rowIndex = 0; rowIndex < _this.kRowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _this.kColumnCount; columnIndex++)
                {
                    if( rowIndex % 2 == 0)
                    {
                        if (columnIndex % 2 == 0)
                        {
                            UITexture texture = CreateTextureTile(rowIndex, columnIndex);
                            texture.transform.localPosition = new Vector3(columnIndex * 75.0f, (rowIndex / 2) * -100.0f, 0.0f);
                            texture.name = rowIndex.ToString() + "_" + columnIndex.ToString();
                        }
                    }                    
                    else
                    {
                        if (columnIndex % 2 == 1)
                        {
                            UITexture texture = CreateTextureTile(rowIndex, columnIndex);
                            texture.transform.localPosition = new Vector3(columnIndex * 75.0f, (rowIndex / 2) * -100.0f - 50.0f, 0.0f);
                            texture.name = rowIndex.ToString() + "_" + columnIndex.ToString();
                        }
                    }
                }
            }
        }

        if (GUILayout.Button("Map Tile Clear") == true)
        {
            UITexture [] textures = _this.transform.GetComponentsInChildren<UITexture>();
            for (int i = 0; i < textures.Length; i++)
                DestroyImmediate(textures[i].gameObject);
        }
    }

    UITexture CreateTextureTile(int _row, int _column)
    {
        GameObject obj = Instantiate(_this.kSampleTile) as GameObject;
        obj.transform.parent = _this.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;

        Cell cell = obj.AddComponent<Cell>();
        cell.kRowIndex      = _row;
        cell.kColumnIndex   = _column;

        UITexture texture = obj.GetComponent<UITexture>();
        texture.width = 100;
        texture.height = 100;

        return texture;
    }
}
