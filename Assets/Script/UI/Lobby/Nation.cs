using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nation {
    public enum Name
    {
        None,
        User,       //유저가 차지한 영역
        Defensive,  //방어가 차지한 영역
        Careful,    //조심이가 차지한 영역
        Usually,    //보통이가 차지한 영역
        Aggressive, //공격이가 차지한 영역
        Max
    }
    
    public Name kName = Name.None;

    [HideInInspector]
    public float kArmmyPower;       //군사력    
    [HideInInspector]
    public float kGrowthPower;      //성장력

    [HideInInspector]
    public List<Zone> kConqueredZoneList = new List<Zone>();

    void Awake()
    {
        //Zone cell = GetComponent<Zone>();
        //cell.kNation = this;
        //kConquestCellList.Add(cell);
    }

    // Use this for initialization
    void Start() {
        //CellManager.Instance.RefreshMapColor();
        //kGrowthPower = Random.Range(0.4f, 0.8f);
        //StartCoroutine(GrowUp(Random.Range(1.0f, 2.0f)));
    }

    // Update is called once per frame
    void Update() {

    }
    
    public float CellUnitArmyPower()
    {
        return kArmmyPower / kConqueredZoneList.Count;
    }

    IEnumerator GrowUp(float _delayTime)
    {
        float curTime = 0.0f;

        while (true)
        {
            curTime += Time.deltaTime;
            if (_delayTime > curTime)
            {
                yield return null;
                continue;
            }

            curTime -= _delayTime;
            kArmmyPower += kGrowthPower;

            float cellUnitArmy = kArmmyPower / kConqueredZoneList.Count;
            
            switch(kName)
            {
                case Name.Defensive:
                    {
                        if (cellUnitArmy > 1.1f)
                            AttackOtherCell();
                    }
                    break;
                case Name.Careful:
                    {
                        if (cellUnitArmy > 0.9f)
                            AttackOtherCell();
                    }
                    break;
                case Name.Usually:
                    {
                        if (cellUnitArmy > 0.7f)
                            AttackOtherCell();
                    }
                    break;
                case Name.Aggressive:
                    {
                        if (cellUnitArmy > 0.5f)
                            AttackOtherCell();
                    }
                    break;
            }
        }
    }

    void AttackOtherCell()
    {
        List<Zone> mAttackEnableList = new List<Zone>();
        for(int i = 0; i < kConqueredZoneList.Count; i++)
        {
            Zone cell = kConqueredZoneList[i];
            if (cell.kRowIndex - 1 >= 0 && cell.kColumnIndex - 1 >= 0)
            {
                Zone desCell = ZoneManager.Instance.Find(cell.kRowIndex - 1, cell.kColumnIndex - 1);
                if( desCell != null )
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex - 2 >= 0)
            {
                Zone desCell = ZoneManager.Instance.Find(cell.kRowIndex - 2, cell.kColumnIndex);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex - 1 >= 0 && cell.kColumnIndex + 1 < ZoneManager.Instance.kColumnCount)
            {
                Zone desCell = ZoneManager.Instance.Find(cell.kRowIndex - 1, cell.kColumnIndex + 1);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex + 1 < ZoneManager.Instance.kRowCount && cell.kColumnIndex - 1 >= 0)
            {
                Zone desCell = ZoneManager.Instance.Find(cell.kRowIndex + 1, cell.kColumnIndex - 1);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex + 2 < ZoneManager.Instance.kRowCount)
            {
                Zone desCell = ZoneManager.Instance.Find(cell.kRowIndex + 2, cell.kColumnIndex);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex + 1 < ZoneManager.Instance.kRowCount && cell.kColumnIndex + 1 < ZoneManager.Instance.kColumnCount)
            {
                Zone desCell = ZoneManager.Instance.Find(cell.kRowIndex + 1, cell.kColumnIndex + 1);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }
        }

        if (mAttackEnableList.Count == 0)
            return;

        int attackCellIndex = Random.Range(0, mAttackEnableList.Count);
        Zone attackCell = mAttackEnableList[attackCellIndex];

        //점령 성공
        if (CellUnitArmyPower() > attackCell.CellUnitArmyPower())
        {
            attackCell.Conquest(this);
        }
        //무승부
        else if (CellUnitArmyPower() == attackCell.CellUnitArmyPower())
        {
            Lose();
            attackCell.Conquest(null);
        }
        //패배
        else
        {
            Lose();
        }

        attackCell.EventApprear();
    }

    public void Lose()
    {
        float cellUnitArmy = kArmmyPower / kConqueredZoneList.Count;
        kArmmyPower -= cellUnitArmy;
    }

    public void AddZone(Zone _zone)
    {
        kConqueredZoneList.Add(_zone);
    }

    public void RemoveZone(Zone _zone)
    {
        kConqueredZoneList.Remove(_zone);
    }
}
