using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonEnum;

public class AssetManager : SingletonT<AssetManager>
{
    [HideInInspector]
    public Dictionary<string, Object> kPreloadedObjectDic = new Dictionary<string, Object>();

    List<AssetBundle> mAllAssetBundlesList = new List<AssetBundle>();
    List<AssetBundle> mCommonAssetBundlesList = new List<AssetBundle>();
    List<AssetBundle> mTitleAssetBundlesList = new List<AssetBundle>();
    List<AssetBundle> mLobbyAssetBundlesList = new List<AssetBundle>();    
    List<AssetBundle> mStageAssetBundlesList = new List<AssetBundle>();
    
    public void Clear()
    {
        for (int i = 0; i < mAllAssetBundlesList.Count; i++)
            mAllAssetBundlesList[i].Unload(true);

        mAllAssetBundlesList.Clear();
        mCommonAssetBundlesList.Clear();
        mStageAssetBundlesList.Clear();
        mLobbyAssetBundlesList.Clear();        
        mTitleAssetBundlesList.Clear();

        Resources.UnloadUnusedAssets();
    }

    public void ClearType(ResType _type)
    {
        switch (_type)
        {
            case ResType.Lobby:
                {
                    for (int i = 0; i < mLobbyAssetBundlesList.Count; i++)
                    {
                        mAllAssetBundlesList.Remove(mLobbyAssetBundlesList[i]);
                        mLobbyAssetBundlesList[i].Unload(true);
                    }

                    mLobbyAssetBundlesList.Clear();
                }
                break;
            case ResType.Stage:
                {
                    for (int i = 0; i < mStageAssetBundlesList.Count; i++)
                    {
                        mAllAssetBundlesList.Remove(mStageAssetBundlesList[i]);
                        mStageAssetBundlesList[i].Unload(true);
                    }

                    mStageAssetBundlesList.Clear();
                }
                break;
            case ResType.Title:
                {
                    for (int i = 0; i < mTitleAssetBundlesList.Count; i++)
                    {
                        mAllAssetBundlesList.Remove(mTitleAssetBundlesList[i]);
                        mTitleAssetBundlesList[i].Unload(true);
                    }

                    mTitleAssetBundlesList.Clear();
                }
                break;
        }

        kPreloadedObjectDic.Clear();
        Resources.UnloadUnusedAssets();
    }

    Object Load(string _path, bool _isSprite = false)
    {
        Object resObject = null;

        //*이 부분 문제가 될 수 있음.(주의 요망)*//
        string key = _path.ToLower();

        if(_isSprite == true)
            key += "Sprite";

        resObject = FindPreloadObject(key);

        if (resObject != null)
            return resObject;

        resObject = FindAssetBundle(_path.ToLower(), _isSprite);

        if (resObject != null)
        {
            kPreloadedObjectDic[key] = resObject;
            return resObject;
        }

        if (_isSprite == true)
            resObject = Resources.Load(_path, typeof(Sprite));
        else
            resObject = Resources.Load(_path);

        if (resObject != null)
            kPreloadedObjectDic[key] = resObject;

        return resObject;
    }

    public Sprite GetSprite(string _path)
    {
        string key = _path.ToLower() + "Sprite";

        Object resObject = FindPreloadObject(key);
        if (resObject != null)
            return resObject as Sprite;

        return Load(_path, true) as Sprite;
    }

    public Object GetObject(string _path)
    {
        string key = _path.ToLower();

        Object resObject = FindPreloadObject(key);
        if (resObject != null)
            return resObject;

        return Load(_path);
    }

    public Object FindPreloadObject(string _key)
    {
        if (kPreloadedObjectDic.ContainsKey(_key) == true)
            return kPreloadedObjectDic[_key];

        return null;
    }

    public void SetAssetBundle(AssetBundle _assetBundle, ResType _type)
    {
        switch(_type)
        {
            case ResType.Common:
                mCommonAssetBundlesList.Add(_assetBundle);
                break;
            case ResType.Title:
                mTitleAssetBundlesList.Add(_assetBundle);
                break;
            case ResType.Lobby:
                mLobbyAssetBundlesList.Add(_assetBundle);
                break;
            case ResType.Stage:
                mStageAssetBundlesList.Add(_assetBundle);
                break;
        }

        mAllAssetBundlesList.Add(_assetBundle);
    }

    Object FindAssetBundle(string _key, bool _isSprite = false)
    {
        for(int i = 0; i < mAllAssetBundlesList.Count; i++)
        {
            AssetBundle asset = mAllAssetBundlesList[i];
            string[] assetNames = asset.GetAllAssetNames();

            for (int n = 0; n < assetNames.Length; n++)
            {
                //Debug.Log(assetNames[n]);
                string assetName = assetNames[n].Replace("assets/resources/", "");
                string key = assetName.Split('.')[0];

                if (_key.CompareTo(key) != 0)
                    continue;

                Object obj = null;

                if (_isSprite == true)
                {
                    obj = asset.LoadAsset(assetNames[n], typeof(Sprite));

                    if (kPreloadedObjectDic.ContainsKey(key + "Sprite") == true)
                        continue;

                    kPreloadedObjectDic.Add(key + "Sprite", obj);
                }
                else
                {
                    obj = asset.LoadAsset(assetNames[n]);

                    if (kPreloadedObjectDic.ContainsKey(key) == true)
                        continue;

                    kPreloadedObjectDic.Add(key, obj);
                }   

                return obj;
            }
        }

        return null;      
    }
}
