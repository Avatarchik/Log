using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonEnum;

public class ZoneManager : SequenceController
{
    public static ZoneManager Instance = null;

    [HideInInspector]
    public int kColumnCount = 1;
    [HideInInspector]
    public int kRowCount = 1;

    [HideInInspector]
    public int kSelectZoneColumn = 0;
    [HideInInspector]
    public int kSelectZoneRow = 0;

    public GameObject kSampleTile;

    [HideInInspector]
    public List<Zone> kZoneList = new List<Zone>();
    [HideInInspector]
    public List<Nation> kNationList = new List<Nation>();
    [HideInInspector]
    public List<Zone> kPlanetZoneList = new List<Zone>();

    public int kLastMinTime = 0;
    public float mCurMinCheckTime = 0.0f;

    void Awake()
    {
        Instance = this;
        for(int i = (int)Nation.Name.None; i < (int)Nation.Name.Max; i++)
        {
            Nation nation = new Nation();
            nation.kName = (Nation.Name)i;
            kNationList.Add(nation);
        }
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Zone zone = transform.GetChild(i).GetComponent<Zone>();
            if (zone == null)
                continue;
            
            Nation.Name name = GameData.Local.GetConqueredZone(zone.kRowIndex, zone.kColumnIndex);
            SetNationZone(name, zone);
            kZoneList.Add(zone);

            if (zone.kPlanetID != 0)
                kPlanetZoneList.Add(zone);
        }
    }
    
    public override void OnPrepare()
    {
        //유저 정렴 리스트가 최상위로 정렬
        kPlanetZoneList.Sort(delegate (Zone _a, Zone _b)
        {
            if (_a.kNation.kName == Nation.Name.User &&
                _b.kNation.kName == Nation.Name.User)
                return 0;
            if (_a.kNation.kName == Nation.Name.User &&
                _b.kNation.kName != Nation.Name.User)
                return -1;
            if (_a.kNation.kName != Nation.Name.User && 
                _b.kNation.kName == Nation.Name.User)
                return 1;

            return 0;
        });

        LobbyUIRoot.Instance.kConqueredList.Refresh();
        RefreshMapColor();

        for (int i = 0; i < kPlanetZoneList.Count; i++)
            kPlanetZoneList[i].RefreshResource();

        List<Zone> userConqueredZoneList = kNationList[(int)Nation.Name.User].kConqueredZoneList;
        for (int i = 0; i < userConqueredZoneList.Count; i++)
            userConqueredZoneList[i].RefreshMilitary();
    }

    void OnEnable()
    {
        System.DateTime quitSysTime = System.Convert.ToDateTime(GameData.Local.quitTime);
        System.DateTime curSysTime = System.DateTime.Now;

        System.TimeSpan timecal = curSysTime - quitSysTime;
        if (int.MaxValue < timecal.TotalMinutes)
            kLastMinTime = int.MaxValue;
        else
            kLastMinTime = (int)timecal.TotalMinutes;

        for (int i = 0; i < kPlanetZoneList.Count; i++)
            kPlanetZoneList[i].SetResource();

        StartCoroutine(ResourceUpdate(timecal.Seconds + GameData.Local.minCheckTime));
    }

    void OnDisable()
    {
        long check = System.DateTime.Now.ToBinary();
        GameData.Local.quitTime = System.DateTime.Now.ToString();
        GameData.Local.minCheckTime = (int)mCurMinCheckTime;

        for (int i = 0; i < kPlanetZoneList.Count; i++)
        {
            Zone zone = kPlanetZoneList[i];
            GameData.Local.SetPlanetRes(zone.transform.name, CommonEnum.ResourceType.Gold, zone.kCurGoldAmount);
            GameData.Local.SetPlanetRes(zone.transform.name, CommonEnum.ResourceType.Material, zone.kCurMaterialAmount);
            GameData.Local.SetPlanetRes(zone.transform.name, CommonEnum.ResourceType.Cristal, zone.kCurCristalAmount);
        }
    }

    IEnumerator ResourceUpdate(int _lastSecTime)
    {
        mCurMinCheckTime = _lastSecTime;
        while (true)
        {
            mCurMinCheckTime += Time.unscaledDeltaTime;
            if (mCurMinCheckTime >= 60.0f)
            {
                mCurMinCheckTime -= 60.0f;
                for (int i = 0; i < kPlanetZoneList.Count; i++)
                    kPlanetZoneList[i].ResourceProduct();
            }

            yield return null;
        }
    }

    public void RefreshMapColor()
    {
        for( int i = 0; i < kZoneList.Count; i++ )
        {
            Zone zone = kZoneList[i];
            if(zone == null)
                continue;

            zone.ColorUpdate();
        }
    }

    public Zone Find(int _rowIndex, int _columnIndex)
    {
        for (int i = 0; i < kZoneList.Count; i++)
        {
            Zone cell = kZoneList[i];
            if (cell.kRowIndex == _rowIndex && cell.kColumnIndex == _columnIndex)
                return cell;
        }

        return null;
    }

    public Nation Find(Nation.Name _name)
    {
        for(int i = 0;i < kNationList.Count; i++)
        {
            Nation nation = kNationList[i];
            if (nation.kName == _name)
                return nation;
        }

        return null;
    }

    Zone mSelectedZone = null;
    public Zone zoneSelect
    {
        get {
            return mSelectedZone;
        }
        set
        {
            WorldCamera.Instance.ZoneFocus(value);

            if (mSelectedZone != null)
                mSelectedZone.FocusOff();

            if (value != null)
            {
                GameData.Lobby.kSelectZoneRow = value.kRowIndex;
                GameData.Lobby.kSelectZoneColumn = value.kColumnIndex;
                value.FocusOn();
            }

            mSelectedZone = value;
        }
    }

    /// <summary> 국가가 점령지를 접수 : 점령 국가, 점령 지역 </summary>
    public void SetNationZone(Nation.Name _nationName, Zone _zone)
    {
        if(_zone.kNation != null)
        {
            Nation oldNation = _zone.kNation;
            oldNation.RemoveZone(_zone);
        }        

        Nation newNation = Find(_nationName);
        _zone.kNation = newNation;
        newNation.AddZone(_zone);
    }

    /// <summary> 회수 가능한 자원의 양 : 회수할 자원 타입 </summary>
    public int GetTotalResource(ResourceType _type)
    {
        List<Zone> zoneList = kNationList[(int)Nation.Name.User].kConqueredZoneList;

        float totalAmount = 0;

        for(int i = 0; i < zoneList.Count; i++)
        {
            switch(_type)
            {
                case ResourceType.Gold:
                    totalAmount += zoneList[i].kCurGoldAmount;
                    break;
                case ResourceType.Material:
                    totalAmount += zoneList[i].kCurMaterialAmount;
                    break;
                case ResourceType.Cristal:
                    totalAmount += zoneList[i].kCurCristalAmount;
                    break;
            }
        }

        return (int)totalAmount;
    }

    /// <summary> 자원을 회수 후 계산 : 회수할 자원 타입 </summary>
    public void CollectResource(ResourceType _type)
    {
        List<Zone> zoneList = kNationList[(int)Nation.Name.User].kConqueredZoneList;
        
        for (int i = 0; i < zoneList.Count; i++)
        {
            switch (_type)
            {
                case ResourceType.Gold:
                    zoneList[i].kCurGoldAmount      -= (int)zoneList[i].kCurGoldAmount;
                    break;
                case ResourceType.Material:
                    zoneList[i].kCurMaterialAmount  -= (int)zoneList[i].kCurMaterialAmount;
                    break;
                case ResourceType.Cristal:
                    zoneList[i].kCurCristalAmount   -= (int)zoneList[i].kCurCristalAmount;
                    break;
            }

            zoneList[i].RefreshResource();
        }
    }
}
