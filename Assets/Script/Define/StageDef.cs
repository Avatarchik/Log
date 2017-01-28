using UnityEngine;
using System.Collections;

public static class StageDef
{
    public const int LAYER_PLAYER = 8;
    public const int LAYER_ENEMY = 9;
    public const int LAYER_UNIT = 10;

    //같은 효과음 중첩 방지 시간
    public const float SOUND_EFFECT_MIN_OVERTIME = 0.15f;
}
