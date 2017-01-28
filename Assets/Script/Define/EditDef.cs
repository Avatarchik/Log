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

    //최대 행성 갯수
    public const int MAX_PLANET = 30;

    //행성 자원 업그레이드시 향상되는 능력치수치(%)
    public const int PLANET_UPGRAGE_ABILITY_PERCENT = 50;
    //행성 자원 업그레이드시 향상되는 지불 금액(코스트) 수치(%)
    public const int PLANET_UPGRAGE_COST_PERCENT = 100;

    //최대 전략 페이지 갯수
    public const int MAX_TACTICS_PAGE_COUNT = 5;

    //1레벨당 주둔 가능한 병력 점수
    public const int USER_MILITARY_SCORE = 2000;
    //주둔 병력 50점수당 소모되는 골드
    public const int USER_50_MILITARY_TO_GOLD = 1;
}
