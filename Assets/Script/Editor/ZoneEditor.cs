using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Zone), true)]
public class ZoneEditor : Editor
{
    string [] mCellObjectArr = {"BluePlanet", "BrownPlanet", "GrayPlanet", "GreenSatellite", "MixBrownPlanet", "PurplePlanet", "RedPlanet", "WhitePlanet", "WhiteSatellite"};

    Zone _this;

    void OnEnable()
    {
        // target은 Editor의 멤버 변수으로 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객처에 대한 
        // 레퍼런스 object형이므로 실제 클라스(타입)으로 캐스팅해서 명확하게 해서 사용하기 용이하게한다.

        _this = target as Zone;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("골드 저장량", _this.kTotalGoldAmount.ToString());
        EditorGUILayout.LabelField("골드 생산량", _this.kProductGoldAmount.ToString());
        EditorGUILayout.LabelField("매터리얼 저장량", _this.kTotalMaterialAmount.ToString());
        EditorGUILayout.LabelField("매터리얼 생산량", _this.kProductMaterialAmount.ToString());
        EditorGUILayout.LabelField("크리스탈 저장량", _this.kTotalCristalAmount.ToString());
        EditorGUILayout.LabelField("크리스탈 생산량", _this.kProductCristalAmount.ToString());

        for (int i = 0; i < _this.kUnitSlotNameList.Count; i++)
        {            
            EditorGUILayout.LabelField((i + 1).ToString() + "슬롯 유닛", _this.kUnitSlotNameList[i]);
        }

        EditorGUILayout.LabelField("군사력", _this.kZoneMilitaryScore.ToString());

        if (GUILayout.Button("Zone Setting") == true)
        {
            Clear(_this);
            Setting(_this);
        }

        if (GUILayout.Button("Zone Clear") == true)
        {
            Clear(_this);
        }
    }

    /// <summary>유닛 리스트로부터 병력 점수를 도출 : 유닛 리스트 </summary>
    public static int GetMilitaryScore(List<int> _units)
    {
        int totalScore = 0;
        for (int i = 0; i < _units.Count; i++)
        {
            DT_UnitData_Info info = CDT_UnitData_Manager.Instance.GetInfo(_units[i]);
            //평균 공격력
            totalScore += (int)((info.MinAttack + info.MaxAttack) * 0.5f * info.UnitCount);
            //총 방어력
            totalScore += (info.ShieldAmount + info.BodyAmount) * info.UnitCount;
        }
        return totalScore;
    }

    public static void Clear(Zone _zone)
    {
        while (_zone.transform.childCount > 0)
            DestroyImmediate(_zone.transform.GetChild(0).gameObject);

        _zone.kProductGoldAmount = 0;
        _zone.kTotalGoldAmount = 0;
        _zone.kProductMaterialAmount = 0;
        _zone.kTotalMaterialAmount = 0;
        _zone.kProductCristalAmount = 0;
        _zone.kTotalCristalAmount = 0;

        _zone.kUnitSlotList.Clear();
        _zone.kUnitSlotNameList.Clear();
        
        _zone.kZoneMilitaryScore = 0;
    }

    public static void Setting(Zone _zone)
    {
        if (_zone.kStageDataID != 0)
        {
            DT_StageData_Info info1 = CDT_StageData_Manager.Instance.GetInfo(_zone.kStageDataID);
            if(_zone.kUnitSlotList == null)
                _zone.kUnitSlotList = new List<int>();
            _zone.kUnitSlotList.Add(info1.Spot1);
            _zone.kUnitSlotList.Add(info1.Spot2);
            _zone.kUnitSlotList.Add(info1.Spot3);
            _zone.kUnitSlotList.Add(info1.Spot4);
            _zone.kUnitSlotList.Add(info1.Spot5);
            _zone.kUnitSlotList.Add(info1.Spot6);
            _zone.kUnitSlotList.Add(info1.Spot7);
            _zone.kUnitSlotList.Add(info1.Spot8);
            _zone.kUnitSlotList.Add(info1.Spot9);
            _zone.kUnitSlotList.Add(info1.Spot10);
            _zone.kUnitSlotList.Add(info1.Spot11);
            _zone.kUnitSlotList.Add(info1.Spot12);
            _zone.kUnitSlotList.Add(info1.Spot13);
            _zone.kUnitSlotList.Add(info1.Spot14);
            _zone.kUnitSlotList.Add(info1.Spot15);

            if (_zone.kUnitSlotNameList == null)
                _zone.kUnitSlotNameList = new List<string>();
            for (int i = 0; i < _zone.kUnitSlotList.Count; i++)
            {                
                DT_UnitData_Info info2 = CDT_UnitData_Manager.Instance.GetInfo(_zone.kUnitSlotList[i]);
                DT_LocalizingData_Info info3 = CDT_LocalizingData_Manager.Instance.GetInfo(info2.Name);
                _zone.kUnitSlotNameList.Add(info3.KO);
            }

            _zone.kZoneMilitaryScore = GetMilitaryScore(_zone.kUnitSlotList);

            GameObject military = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/MilitaryLabel.prefab");
            GameObject militaryObj = Instantiate(military) as GameObject;
            militaryObj.transform.parent = _zone.transform;
            militaryObj.transform.localPosition = Vector3.zero;
            militaryObj.transform.localRotation = Quaternion.identity;
            militaryObj.transform.localScale = Vector3.one;
            militaryObj.name = "MilitaryLabel";
            militaryObj.GetComponent<UILabel>().text = _zone.kZoneMilitaryScore.ToString();
            militaryObj.GetComponent<UILabel>().depth = 2;
        }

        if (_zone.kPlanetID != 0)
        {
            DT_PlanetData_Info info = CDT_PlanetData_Manager.Instance.GetInfo(_zone.kPlanetID);
            GameObject planet = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Planet/" + info.PlanetName + ".prefab");
            GameObject planetObj = Instantiate(planet) as GameObject;
            planetObj.transform.parent = _zone.transform;
            planetObj.transform.localPosition = Vector3.zero;
            planetObj.transform.localRotation = Quaternion.identity;
            planetObj.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);
            planetObj.name = info.PlanetName;

            _zone.kProductGoldAmount = info.GoldProduct;
            _zone.kTotalGoldAmount = info.GoldStorage;
            _zone.kProductMaterialAmount = info.MaterialProduct;
            _zone.kTotalMaterialAmount = info.MaterialStorage;
            _zone.kProductCristalAmount = info.CristalProduct;
            _zone.kTotalCristalAmount = info.CristalStorage;

            GameObject resource = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/ResourceLabel.prefab");
            GameObject resourceObj = Instantiate(resource) as GameObject;
            resourceObj.transform.parent = _zone.transform;
            resourceObj.transform.localPosition = Vector3.zero;
            resourceObj.transform.localRotation = Quaternion.identity;
            resourceObj.transform.localScale = Vector3.one;
            resourceObj.name = "ResourceLabel";

            UILabel[] labels = resourceObj.GetComponentsInChildren<UILabel>();
            for (int i = 0; i < labels.Length; i++)
                labels[i].depth = 2;

            Transform militaryTrans = _zone.transform.Find("MilitaryLabel");
            if (militaryTrans != null)
            {
                Vector3 pos = militaryTrans.localPosition;
                pos.y = -37.0f;
                militaryTrans.localPosition = pos;
            }
        }
    }
}
