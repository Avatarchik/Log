using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonEnum;

public class Zone : MonoBehaviour
{
    [HideInInspector]
    public Nation kNation = null;

    ////////////////////////////////////////////////////////////////////////
    //자원 생성    
    public int kPlanetID = 0;
    [HideInInspector]
    public float kCurGoldAmount = 0;
    [HideInInspector]
    public float kTotalGoldAmount = 0;
    [HideInInspector]
    public float kProductGoldAmount = 0;
    [HideInInspector]
    public float kCurMaterialAmount = 0;
    [HideInInspector]
    public float kTotalMaterialAmount = 0;
    [HideInInspector]
    public float kProductMaterialAmount = 0;
    [HideInInspector]
    public float kCurCristalAmount = 0;
    [HideInInspector]
    public float kTotalCristalAmount = 0;
    [HideInInspector]
    public float kProductCristalAmount = 0;

    TweenScale mTextureTweenScale;
    
    ////////////////////////////////////////////////////////////////////////
    //유닛 배치
    [HideInInspector]
    public List<int> kUnitSlotList;
    [HideInInspector]
    public List<string> kUnitSlotNameList;

    public int kStageDataID = 0;
    [HideInInspector]
    public int kZoneMilitaryScore = 0;

    ////////////////////////////////////////////////////////////////////////
    //슬롯 정보    
    [HideInInspector]
    public int kRowIndex = 0;       //가로 인덱스
    [HideInInspector]
    public int kColumnIndex = 0;    //세로 인덱스

    [HideInInspector]
    public bool kIsSelectEnable = false;

    TweenAlpha mTweenAlpha = null;

    [HideInInspector]
    public UILabel kMilitaryScoreLabel;
    [HideInInspector]
    public UILabel kGoldLabel;
    [HideInInspector]
    public UILabel kMaterialLabel;
    [HideInInspector]
    public UILabel kCristalLabel;

    [HideInInspector]
    public UITexture mUITexture;

    void Awake()
    {
        GetComponent<UIButton>().tweenTarget = null;        
        mUITexture = transform.Find("Texture").GetComponent<UITexture>();

        mTextureTweenScale = mUITexture.GetComponent<TweenScale>();
        mTextureTweenScale.enabled = false;
        /*
        mTweenAlpha = GetComponent<TweenAlpha>();
        mTweenAlpha.duration = 2.0f;
        mTweenAlpha.enabled = false;*/

        Transform militaryScoreTrans = transform.Find("MilitaryLabel");
        if (militaryScoreTrans != null)
        {
            kMilitaryScoreLabel = militaryScoreTrans.GetComponent<UILabel>();
            kMilitaryScoreLabel.gameObject.SetActive(false);
        }

        Transform goldTrans = transform.Find("ResourceLabel/Gold");
        if (goldTrans != null)
        {
            kGoldLabel = goldTrans.GetComponent<UILabel>();
            kGoldLabel.gameObject.SetActive(false);
        }

        Transform materialTrans = transform.Find("ResourceLabel/Material");
        if(materialTrans != null)
        {
            kMaterialLabel = materialTrans.GetComponent<UILabel>();
            kMaterialLabel.gameObject.SetActive(false);
        }
        
        Transform cristalTrans = transform.Find("ResourceLabel/Cristal");
        if(cristalTrans != null)
        {
            kCristalLabel = cristalTrans.GetComponent<UILabel>();
            kCristalLabel.gameObject.SetActive(false);
        }
    }
    
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EventApprear()
    {
        mTweenAlpha.ResetToBeginning();
        mTweenAlpha.PlayForward();
        Invoke("ColorUpdate", mTweenAlpha.duration);
    }

    public void Conquest(Nation _nation)
    {
        if (kNation != null)
        {
            kNation.kConqueredZoneList.Remove(this);
            kNation.Lose();
        }

        if (_nation != null)
            _nation.kConqueredZoneList.Add(this);

        kNation = _nation;
    }

    public void ColorUpdate()
    {
        Color color = mUITexture.color;
        float alpha = color.a;
        
        switch (kNation.kName)
        {
            case Nation.Name.None:
                {
                    color = Color.white;
                    if (kIsSelectEnable == true)
                        color.a = 155.0f / 255.0f;
                    else
                        color.a = 25.0f / 255.0f;
                }
                break;
            case Nation.Name.User:
                {
                    color = Color.green;
                }
                break;
            case Nation.Name.Aggressive:
                {
                    color = Color.red;
                    //color.a = 1.0f;// 150.0f / 255.0f;
                }
                break;
            case Nation.Name.Careful:
                {
                    color = Color.blue;
                    //color.a = 1.0f; //150.0f / 255.0f;
                }
                break;
            case Nation.Name.Defensive:
                {
                    color = Color.black;
                    //color.a = 1.0f; //150.0f / 255.0f;
                }
                break;
            case Nation.Name.Usually:
                {
                    color = Color.yellow;
                    //color.a = 1.0f; //150.0f / 255.0f;
                }
                break;
        }

        //color.a = alpha;
        mUITexture.color = color;
    }

    public float CellUnitArmyPower()
    {
        if (kNation == null)
            return 0.0f;

        return kNation.CellUnitArmyPower();
    }

    public void SetResource()
    {
        if (kPlanetID == 0)
            return;

        int lastMin = ZoneManager.Instance.kLastMinTime;

        DT_PlanetData_Info info = CDT_PlanetData_Manager.Instance.GetInfo(kPlanetID);

        float productUp = GameData.Local.GetPlanetProductLevel(name) * EditDef.PLANET_UPGRAGE_ABILITY_PERCENT * CommonDef.TO_PERCENT_UNIT;
        float storageUp = GameData.Local.GetPlanetStorageLevel(name) * EditDef.PLANET_UPGRAGE_ABILITY_PERCENT * CommonDef.TO_PERCENT_UNIT;

        kProductGoldAmount = info.GoldProduct + info.GoldProduct * productUp;
        kTotalGoldAmount = info.GoldStorage + info.GoldStorage * storageUp;

        kProductMaterialAmount = info.MaterialProduct + info.MaterialProduct * productUp;
        kTotalMaterialAmount = info.MaterialStorage + info.MaterialStorage * storageUp;

        kProductCristalAmount = info.CristalProduct + info.CristalProduct * productUp;
        kTotalCristalAmount = info.CristalStorage + info.CristalStorage * storageUp;

        if (kProductGoldAmount > 0)
        {
            kCurGoldAmount = GameData.Local.GetPlanetRes(name, ResourceType.Gold) + lastMin * kProductGoldAmount;
            if (kCurGoldAmount > kTotalGoldAmount)
                kCurGoldAmount = kTotalGoldAmount;
        }

        if (kProductMaterialAmount > 0)
        {
            kCurMaterialAmount = GameData.Local.GetPlanetRes(name, ResourceType.Material) + lastMin * kProductMaterialAmount;
            if (kCurMaterialAmount > kTotalMaterialAmount)
                kCurMaterialAmount = kTotalMaterialAmount;
        }

        if (kProductCristalAmount > 0)
        {
            kCurCristalAmount = GameData.Local.GetPlanetRes(name, ResourceType.Cristal) + lastMin * kProductCristalAmount;
            if (kCurCristalAmount > kTotalCristalAmount)
                kCurCristalAmount = kTotalCristalAmount;
        }

        RefreshResource();
    }

    public void ResourceProduct()
    {
        if (kProductGoldAmount > 0)
        {
            kCurGoldAmount += kProductGoldAmount;
            if (kCurGoldAmount > kTotalGoldAmount)
                kCurGoldAmount = kTotalGoldAmount;
        }

        if (kProductMaterialAmount > 0)
        {
            kCurMaterialAmount += kProductMaterialAmount;
            if (kCurMaterialAmount > kTotalMaterialAmount)
                kCurMaterialAmount = kTotalMaterialAmount;
        }

        if (kProductCristalAmount > 0)
        {
            kCurCristalAmount += kProductCristalAmount;
            if (kCurCristalAmount > kTotalCristalAmount)
                kCurCristalAmount = kTotalCristalAmount;
        }

        RefreshResource();
    }    

    public void RefreshResource()
    {
        if (kProductGoldAmount > 0)
        {
            if (kGoldLabel != null)
            {
                kGoldLabel.gameObject.SetActive(true);
                kGoldLabel.text = StringUtil.TwoMix("G +", ((int)kCurGoldAmount).ToString());
            }
        }
        if (kProductMaterialAmount > 0)
        {
            if (kMaterialLabel != null)
            {
                kMaterialLabel.gameObject.SetActive(true);
                kMaterialLabel.text = StringUtil.TwoMix("M +", ((int)kCurMaterialAmount).ToString());
            }
        }
        if (kProductCristalAmount > 0)
        {
            if (kCristalLabel != null)
            {
                kCristalLabel.gameObject.SetActive(true);
                kCristalLabel.text = StringUtil.TwoMix("C +", ((int)kCurCristalAmount).ToString());
            }
        }
    }

    public void SelectEnable()
    {
        kIsSelectEnable = true;
        ColorUpdate();
    }

    /// <summary> 해당 지역의 병력 점수 갱신 ( 주변 갱신 여부 )</summary>    
    public void RefreshMilitary(bool _isWithAround = true)
    {
        kMilitaryScoreLabel.gameObject.SetActive(true);
        kMilitaryScoreLabel.text = GameData.User.GetHaveZoneMilitary(name).ToString();
        SelectEnable();

        //주변 셀의 전투력을 보여준다.
        if (_isWithAround == true)
        {            
            if (kRowIndex - 1 >= 0 && kColumnIndex - 1 >= 0)
            {
                Zone desZone = ZoneManager.Instance.Find(kRowIndex - 1, kColumnIndex - 1);
                if (desZone != null && desZone.kNation.kName != Nation.Name.User)
                {
                    desZone.kMilitaryScoreLabel.gameObject.SetActive(true);
                    desZone.kMilitaryScoreLabel.text = desZone.kZoneMilitaryScore.ToString();
                    desZone.SelectEnable();
                }
            }
            if (kRowIndex - 2 >= 0)
            {
                Zone desZone = ZoneManager.Instance.Find(kRowIndex - 2, kColumnIndex);
                if (desZone != null && desZone.kNation.kName != Nation.Name.User)
                {
                    desZone.kMilitaryScoreLabel.gameObject.SetActive(true);
                    desZone.kMilitaryScoreLabel.text = desZone.kZoneMilitaryScore.ToString();
                    desZone.SelectEnable();
                }
            }

            if (kRowIndex - 1 >= 0 && kColumnIndex + 1 < ZoneManager.Instance.kColumnCount)
            {
                Zone desZone = ZoneManager.Instance.Find(kRowIndex - 1, kColumnIndex + 1);
                if (desZone != null && desZone.kNation.kName != Nation.Name.User)
                {
                    desZone.kMilitaryScoreLabel.gameObject.SetActive(true);
                    desZone.kMilitaryScoreLabel.text = desZone.kZoneMilitaryScore.ToString();
                    desZone.SelectEnable();
                }
            }

            if (kRowIndex + 1 < ZoneManager.Instance.kRowCount && kColumnIndex - 1 >= 0)
            {
                Zone desZone = ZoneManager.Instance.Find(kRowIndex + 1, kColumnIndex - 1);
                if (desZone != null && desZone.kNation.kName != Nation.Name.User)
                {
                    desZone.kMilitaryScoreLabel.gameObject.SetActive(true);
                    desZone.kMilitaryScoreLabel.text = desZone.kZoneMilitaryScore.ToString();
                    desZone.SelectEnable();
                }
            }

            if (kRowIndex + 2 < ZoneManager.Instance.kRowCount)
            {
                Zone desZone = ZoneManager.Instance.Find(kRowIndex + 2, kColumnIndex);
                if (desZone != null && desZone.kNation.kName != Nation.Name.User)
                {
                    desZone.kMilitaryScoreLabel.gameObject.SetActive(true);
                    desZone.kMilitaryScoreLabel.text = desZone.kZoneMilitaryScore.ToString();
                    desZone.SelectEnable();
                }
            }

            if (kRowIndex + 1 < ZoneManager.Instance.kRowCount && kColumnIndex + 1 < ZoneManager.Instance.kColumnCount)
            {
                Zone desZone = ZoneManager.Instance.Find(kRowIndex + 1, kColumnIndex + 1);
                if (desZone != null && desZone.kNation.kName != Nation.Name.User)
                {
                    desZone.kMilitaryScoreLabel.gameObject.SetActive(true);
                    desZone.kMilitaryScoreLabel.text = desZone.kZoneMilitaryScore.ToString();
                    desZone.SelectEnable();
                }
            }
        }
    }

    public void OnClickButton()
    {
        if(kIsSelectEnable == false)
        {
            MessageBox.Open(3000028, null);
            LobbyUIRoot.Instance.SetMenu(LobbyEnum.MenuSelect.WorldMap);
            return;
        }

        ZoneManager.Instance.zoneSelect = this;

        if (kNation.kName == Nation.Name.User)
            LobbyUIRoot.Instance.SetMenu(LobbyEnum.MenuSelect.ConqueredZone);
        else
            LobbyUIRoot.Instance.SetMenu(LobbyEnum.MenuSelect.UnconqueredZone);

        GameData.Lobby.kSelectStageDataID = kStageDataID;
    }

    public void FocusOff()
    {
        mTextureTweenScale.enabled = false;
        mUITexture.transform.localScale = Vector3.one;
    }

    public void FocusOn()
    {
        mTextureTweenScale.enabled = true;
    }
}