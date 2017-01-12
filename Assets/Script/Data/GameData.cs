using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class GameData{
    static LocalData mLocal;
    static StageData mStage;
    
    public static LocalData Local
    {
        get
        {
            if (mLocal == null)
                mLocal = LocalData.Instance;

            return mLocal;
        }
    }
    
    public static StageData Stage
    {
        get
        {
            if (mStage == null)
                mStage = StageData.Instance;

            return mStage;
        }
    }    
}
