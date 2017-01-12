using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonEnum;

public class StageData : SingletonT<StageData>
{
    public bool kIsFromLobby = false;
    public bool kIsFromTitle = false;
    
    [HideInInspector]
    public bool kTestModeNoDamage = false;    
    [HideInInspector]
    public bool kTestModeAutoPlay = false;
    [HideInInspector]
    public bool kTestModeLogView = false;
}