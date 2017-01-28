using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class GameData{
    static LocalData mLocal;
    static StageData mStage;
    static LobbyData mLobby;
    static UserData mUser;

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

    public static LobbyData Lobby
    {
        get
        {
            if (mLobby == null)
                mLobby = LobbyData.Instance;

            return mLobby;
        }
    }

    public static UserData User
    {
        get
        {
            if (mUser == null)
                mUser = UserData.Instance;

            return mUser;
        }
    }
}
