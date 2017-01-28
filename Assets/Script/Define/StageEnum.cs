using UnityEngine;
using System.Collections;

namespace StageEnum
{
    public enum BattleSign
    {
        EnemyFind,
        EnemyDestroy,
        PlayerDestory,
        Order
    }

    public enum Mode
    {
        None,
        Battle,
        Conquer
    }
}