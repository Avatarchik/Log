using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nation : MonoBehaviour {
    public enum Feature
    {
        None,
        Defensive,
        Careful,
        Usually,
        Aggressive,
        Max
    }
    
    public Feature kFeature = Feature.None;

    [HideInInspector]
    public float kArmmyPower;       //군사력    
    [HideInInspector]
    public float kGrowthPower;      //성장력

    [HideInInspector]
    public List<Cell> kConquestCellList = new List<Cell>();

    void Awake()
    {
        Cell cell = GetComponent<Cell>();
        cell.kNation = this;
        kConquestCellList.Add(cell);
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
        return kArmmyPower / kConquestCellList.Count;
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

            float cellUnitArmy = kArmmyPower / kConquestCellList.Count;
            
            switch(kFeature)
            {
                case Feature.Defensive:
                    {
                        if (cellUnitArmy > 1.1f)
                            AttackOtherCell();
                    }
                    break;
                case Feature.Careful:
                    {
                        if (cellUnitArmy > 0.9f)
                            AttackOtherCell();
                    }
                    break;
                case Feature.Usually:
                    {
                        if (cellUnitArmy > 0.7f)
                            AttackOtherCell();
                    }
                    break;
                case Feature.Aggressive:
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
        List<Cell> mAttackEnableList = new List<Cell>();
        for(int i = 0; i < kConquestCellList.Count; i++)
        {
            Cell cell = kConquestCellList[i];
            if (cell.kRowIndex - 1 >= 0 && cell.kColumnIndex - 1 >= 0)
            {
                Cell desCell = CellManager.Instance.Find(cell.kRowIndex - 1, cell.kColumnIndex - 1);
                if( desCell != null )
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex - 2 >= 0)
            {
                Cell desCell = CellManager.Instance.Find(cell.kRowIndex - 2, cell.kColumnIndex);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex - 1 >= 0 && cell.kColumnIndex + 1 < CellManager.Instance.kColumnCount)
            {
                Cell desCell = CellManager.Instance.Find(cell.kRowIndex - 1, cell.kColumnIndex + 1);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex + 1 < CellManager.Instance.kRowCount && cell.kColumnIndex - 1 >= 0)
            {
                Cell desCell = CellManager.Instance.Find(cell.kRowIndex + 1, cell.kColumnIndex - 1);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex + 2 < CellManager.Instance.kRowCount)
            {
                Cell desCell = CellManager.Instance.Find(cell.kRowIndex + 2, cell.kColumnIndex);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }

            if (cell.kRowIndex + 1 < CellManager.Instance.kRowCount && cell.kColumnIndex + 1 < CellManager.Instance.kColumnCount)
            {
                Cell desCell = CellManager.Instance.Find(cell.kRowIndex + 1, cell.kColumnIndex + 1);
                if (desCell != null)
                    if (desCell.kNation == null || desCell.kNation != this)
                        mAttackEnableList.Add(desCell);
            }
        }

        if (mAttackEnableList.Count == 0)
            return;

        int attackCellIndex = Random.Range(0, mAttackEnableList.Count);
        Cell attackCell = mAttackEnableList[attackCellIndex];

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
        float cellUnitArmy = kArmmyPower / kConquestCellList.Count;
        kArmmyPower -= cellUnitArmy;
    }
}
