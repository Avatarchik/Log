using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class ObjectPoolManager : SequenceController {
    public static ObjectPoolManager Instance = null;

    Transform mObjectPoolTrans;

    public Dictionary<string, Stack<GameObject>> mObjectPoolDic = new Dictionary<string, Stack<GameObject>>();
    List<GameObject> mOutPoolList = new List<GameObject>();

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public override void OnPrepare()
    {
        mObjectPoolDic.Clear();
        mOutPoolList.Clear();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        GameObject pool = new GameObject();
        pool.name = "ObjectPool";
        mObjectPoolTrans = pool.transform;
        
        Load(StrDef.EFFECT_MUZZLE, 10);
        Load(StrDef.EFFECT_SHIELD, 10);
        Load(StrDef.UI_PLAYERMARK, 15);
        Load(StrDef.UI_ENEMYMARK, 15);
        Load(StrDef.UI_TYPOTEXT, 10);
    }

    public void Load(string _resPath, int _size)
    {
        UnityEngine.Object obj = Resources.Load(_resPath);
        if (obj == null)
        {
            Debug.Log(_resPath + "가 존재하지 않습니다.");
            return;
        }

        Stack<GameObject> objectList = null;
        if ( mObjectPoolDic.ContainsKey(_resPath) == true )
        {
            objectList = mObjectPoolDic[_resPath];
        }
        else
        {
            objectList = new Stack<GameObject>(_size);
            mObjectPoolDic.Add(_resPath, objectList);
        }

        for( int i = 0; i < _size; i++ )
        {            
            GameObject gameObj = Instantiate(obj, new Vector3(100, 100, 100), Quaternion.identity) as GameObject;
            
            gameObj.SetActive(false);            
            gameObj.transform.parent = mObjectPoolTrans;
            gameObj.name = _resPath;

            objectList.Push(gameObj);
        }
    }


    public GameObject GetGameObejct(string _resPath, Transform _parentTrans = null)
    {
        if(mObjectPoolDic.ContainsKey(_resPath) == false)
            Load(_resPath, 1);

        Stack<GameObject> objectList = mObjectPoolDic[_resPath];

        if (objectList.Count == 0)
            Load(_resPath, 1);

        GameObject gamoObj = objectList.Pop();
        mOutPoolList.Add(gamoObj);

        if (_parentTrans != null)
            gamoObj.transform.parent = _parentTrans;
        
        gamoObj.SetActive(true);

        return gamoObj;
    }

    public void Release(GameObject _gameObj)
    {
        mOutPoolList.Remove(_gameObj);
        Stack<GameObject> objects = mObjectPoolDic[_gameObj.name];
        _gameObj.SetActive(false);
        _gameObj.transform.parent = mObjectPoolTrans;

        objects.Push(_gameObj);
    }

    void Destroy()
    {
        mObjectPoolDic.Clear();
    }

    public void AllOutPoolCollect()
    {
        for(int i = 0; i < mOutPoolList.Count; i++)
        {
            GameObject obj = mOutPoolList[i];
            Release(obj);
            i--;
        }
    }
}
