using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    public delegate void UpdateFunc();

    public int kUpdateCountByFrame = 10;

    int mCurUpdateIndex = 0;
    List<UpdateFunc> mUpdateList = new List<UpdateFunc>();

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mUpdateList.Count <= 0)
            return;

        for (int i = 0; i < kUpdateCountByFrame; i++)
        {   
            mUpdateList[mCurUpdateIndex]();

            mCurUpdateIndex++;
            if (mCurUpdateIndex >= mUpdateList.Count)
                mCurUpdateIndex = 0;
        }
    }

    public void AddQueue(UpdateFunc _func)
    {
        mUpdateList.Add(_func);
    }
}
