using UnityEngine;
using System.Collections;

public static class EditDef
{
    public const bool FPS_LOG = false;

    //카메라 줌인 줌아웃 거리
    public const float CAMERA_DISTANCE_MAX = 50.0f;
    public const float CAMERA_DISTANCE_MIN = -50.0f;
        
    //기본 사운드 볼륨 조절
    public const float SOUND_BGM_DEFAULT_VOLUME = 0.5f;
    public const float SOUND_EFFECT_DEFAULT_VOLUME = 0.25f;
    //같은 효과음 중첩 방지 시간
    public const float SOUND_EFFECT_MIN_OVERTIME = 0.15f;
}
