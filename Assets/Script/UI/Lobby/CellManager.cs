using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellManager : MonoBehaviour {
    public static CellManager Instance = null;

    [HideInInspector]
    public int kColumnCount = 1;
    [HideInInspector]
    public int kRowCount = 1;

    public GameObject kSampleTile;

    public List<Cell> kCellList = new List<Cell>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            Cell cell = transform.GetChild(i).GetComponent<Cell>();
            if (cell == null)
                continue;

            kCellList.Add(cell);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RefreshMapColor()
    {
        for( int i = 0; i < kCellList.Count; i++ )
        {
            Cell cell = kCellList[i];
            if(cell == null)
                continue;

            cell.ColorUpdate();
        }
    }

    public Cell Find(int _rowIndex, int _columnIndex)
    {
        for (int i = 0; i < kCellList.Count; i++)
        {
            Cell cell = kCellList[i];
            if (cell.kRowIndex == _rowIndex && cell.kColumnIndex == _columnIndex)
                return cell;
        }

        return null;
    }
}
