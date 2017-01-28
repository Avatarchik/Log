using LobbyEnum;
using StageEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyData : SingletonT<LobbyData>
{
    [HideInInspector]
    public int kSelectStageDataID = 0;

    [HideInInspector]
    public int kSelectZoneRow = 0;
    [HideInInspector]
    public int kSelectZoneColumn = 0;

    [HideInInspector]
    public Mode kSelectMode = Mode.None;

    [HideInInspector]
    public MenuSelect kSelectMenu = MenuSelect.Main;
}
