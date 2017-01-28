using UnityEngine;
using UnityEditor;
using System.Collections;
using CommonEnum;

[CustomEditor(typeof(ZoneManager), true)]
public class ZoneManagerEditor : Editor
{
    ZoneManager _this;

    void OnEnable()
    {
        // target은 Editor의 멤버 변수으로 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객처에 대한 
        // 레퍼런스 object형이므로 실제 클라스(타입)으로 캐스팅해서 명확하게 해서 사용하기 용이하게한다.

        _this = target as ZoneManager;
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
        /*
        if (GUILayout.Button("Map Tile Creat") == true)
        {
            for (int rowIndex = 0; rowIndex < _this.kRowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _this.kColumnCount; columnIndex++)
                {
                    if (rowIndex % 2 == 0)
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
            UITexture[] textures = _this.transform.GetComponentsInChildren<UITexture>();
            for (int i = 0; i < textures.Length; i++)
                DestroyImmediate(textures[i].gameObject);
        }
        */
        if (GUILayout.Button("Map Check") == true)
        {
            Zone[] zones = _this.GetComponentsInChildren<Zone>();
            for(int i = 0; i < zones.Length; i++)
            {
                UIButton btn = zones[i].GetComponent<UIButton>();
                if (btn == null)
                    btn = zones[i].gameObject.AddComponent<UIButton>();

                EventDelegate onClick = new EventDelegate(zones[i], "OnClickButton");
                EventDelegate.Add(btn.onClick, onClick);

                BoxCollider col = zones[i].GetComponent<BoxCollider>();
                if (col == null)
                    col = zones[i].gameObject.AddComponent<BoxCollider>();

                col.size = new Vector3(50.0f, 100.0f, 1.0f);

                zones[i].kStageDataID = 1000001;

                UITexture texture = zones[i].GetComponent<UITexture>();
                TweenHeight tweenHeight = zones[i].GetComponent<TweenHeight>();
                TweenWidth tweenWidth = zones[i].GetComponent<TweenWidth>();
                if(tweenHeight != null)
                    DestroyImmediate(tweenHeight);
                if (tweenWidth != null)
                    DestroyImmediate(tweenWidth);
                if (texture != null)
                    DestroyImmediate(texture);

                UITexture [] childTextures = zones[i].transform.GetComponentsInChildren<UITexture>();
                for(int n = 0; n < childTextures.Length; n++)
                    DestroyImmediate(childTextures[n].gameObject);

                GameObject tileTexture = Instantiate(_this.kSampleTile) as GameObject;
                tileTexture.transform.parent = zones[i].transform;
                tileTexture.transform.localPosition = Vector3.zero; 
                tileTexture.transform.localScale = Vector3.one;
                tileTexture.transform.localRotation = Quaternion.identity;
                tileTexture.name = "Texture";

                TweenScale tween = tileTexture.AddComponent<TweenScale>();
                tween.from = new Vector3(1.0f, 1.0f, 1.0f);
                tween.to = new Vector3(0.85f, 0.85f, 0.85f);
                tween.style = UITweener.Style.PingPong;
                tween.duration = 0.3f;

                texture = tileTexture.GetComponent<UITexture>();
                texture.width = 100;
                texture.height = 100;
            }
        }

        GUILayout.Space(10.0f);

        if (GUILayout.Button("Zone Setting") == true)
        {
            Zone [] zones = _this.GetComponentsInChildren<Zone>();
            for (int i = 0; i < zones.Length; i++)
            {
                ZoneEditor.Clear(zones[i]);
                ZoneEditor.Setting(zones[i]);
            }
        }

        if (GUILayout.Button("Zone Clear") == true)
        {
            Zone[] zones = _this.GetComponentsInChildren<Zone>();
            for (int i = 0; i < zones.Length; i++)
            {
                ZoneEditor.Clear(zones[i]);
            }
        }

        GUILayout.Space(10.0f);

        if (GUILayout.Button("모든 데이터 초기화") == true)
        {
            PlayerPrefs.DeleteAll();
        }

        if (GUILayout.Button("점령지 데이터 초기화") == true)
        {
            PlayerPrefs.DeleteKey(UserData.Type.UserHaveMilitary.ToString());

            Zone[] zones = _this.GetComponentsInChildren<Zone>();
            for (int i = 0; i < zones.Length; i++)
                PlayerPrefs.DeleteKey(LocalData.Type.ConqueredZone.ToString() + "_" + zones[i].kColumnIndex + "_" + zones[i].kRowIndex);
        }

        if (GUILayout.Button("행성 자원 데이터 초기화") == true)
        {
            Zone[] zones = _this.GetComponentsInChildren<Zone>();
            for (int i = 0; i < zones.Length; i++)
            {
                if (zones[i].kPlanetID == 0)
                    continue;
                PlayerPrefs.DeleteKey(LocalData.Type.QuitTime.ToString());
                PlayerPrefs.DeleteKey(LocalData.Type.MinCheckTime.ToString());

                PlayerPrefs.DeleteKey(LocalData.Type.Resource.ToString() + zones[i].name + ResourceType.Gold.ToString());
                PlayerPrefs.DeleteKey(LocalData.Type.Resource.ToString() + zones[i].name + ResourceType.Material.ToString());
                PlayerPrefs.DeleteKey(LocalData.Type.Resource.ToString() + zones[i].name + ResourceType.Cristal.ToString());

                PlayerPrefs.DeleteKey(LocalData.Type.Product.ToString() + zones[i].name);
                PlayerPrefs.DeleteKey(LocalData.Type.Storage.ToString() + zones[i].name);
            }
        }

        if (GUILayout.Button("유저 데이터 초기화") == true)
        {
            PlayerPrefs.DeleteKey(UserData.Type.UserGold.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserMaterial.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserCristal.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserLevel.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserExp.ToString());

            PlayerPrefs.DeleteKey(UserData.Type.UserAttackAbility.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserMoveAbility.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserTurnAbility.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserShieldAbility.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserBodyAbility.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserIncomingsAbility.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserOutgoingsAbility.ToString());
            PlayerPrefs.DeleteKey(UserData.Type.UserResourceAbility.ToString());

            Zone[] zones = _this.GetComponentsInChildren<Zone>();
            for (int i = 0; i < zones.Length; i++)
                PlayerPrefs.DeleteKey(UserData.Type.UserHaveMilitary.ToString() + zones[i].name);

            for (int i = 0; i < 100; i++)
                PlayerPrefs.DeleteKey(UserData.Type.UserHaveUnit.ToString() + (CommonDef.UNIT_ID_NUMBERING + i));

            for (int i = 0; i < 100; i++)
                PlayerPrefs.DeleteKey(LocalData.Type.StageClearTime.ToString() + "_" + i);

            for (int i = 0; i < EditDef.MAX_TACTICS_PAGE_COUNT; i++)
                for (int n = 0; n < CommonDef.MAX_SHIP_GROUP_COUNT; i++)
                    PlayerPrefs.DeleteKey(LocalData.Type.UnitSlot.ToString() + "_" + i + "_" + n);
        }
    }
    /*
    UITexture CreateTextureTile(int _row, int _column)
    {
        GameObject obj = new GameObject();
        obj.transform.parent = _this.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;

        Zone zone = obj.AddComponent<Zone>();
        zone.kRowIndex      = _row;
        zone.kColumnIndex   = _column;

        GameObject tileTexture = Instantiate(_this.kSampleTile) as GameObject;
        obj.transform.parent = tileTexture.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity; 

        UITexture texture = tileTexture.GetComponent<UITexture>();
        texture.width = 100;
        texture.height = 100;

        return texture;
    }*/
}
