using UnityEngine;
using System.Collections;

namespace CommonEnum
{
    public enum Model
    {
        None,

        Skeleton            = 1000001,  //스켈레톤
        SkeletonSquad       = 1000002,  //스켈레톤 분대(3대)
        SkeletonArmy        = 1000003,  //스켈레톤 부대(6대)

        Archer              = 1000004,  //아처
        ArcherSquad         = 1000005,  //아처 분대(2대)
        ArcherArmy          = 1000006,  //아처 부대(4대)

        SpearGoblin         = 1000007,  //창 고블린
        SpearGoblinSquad    = 1000008,  //창 고블린 분대(3대)
        SpearGoblinArmy     = 1000009,  //창 고블린 부대(5대)

        Goblin              = 1000010,  //고블린
        GoblinSquad         = 1000011,  //고블린 분대(3대)
        GoblinArmy          = 1000012,  //고블린 부대(5대)
        
        Barbarian           = 1000013,  //바바리안
        BarbarianSquad      = 1000014,  //바바리안 분대(2대)
        BarbarianArmy       = 1000015,  //바바리안 부대(4대)

        Knight              = 1000016,  //기사
        KnightSquad         = 1000017,  //기사 분대(2대)
        KnightArmy          = 1000018,  //기사 부대(3대)

        Musketeer           = 1000019,  //포수
        MusketeerSquad      = 1000020,  //포수 분대(2대)
        MusketeerArmy       = 1000021,  //포수 분대(3대)

        Witch               = 1000022,  //마녀
        WitchSquad          = 1000023,  //마녀 분대(2대)

        SpearGoblinLord     = 1000024,  //창 고블린 군주
        SpearGoblinLordSquad= 1000025,  //창 고블린 군주 분대(2대)

        GoblinLord          = 1000026,  //고블린 군주
        GoblinLordSquad     = 1000027,  //고블린 군주 분대(2대)

        BarbarianLord       = 1000028,  //바바리안 군주
        BarbarianLordSquad  = 1000029,  //바바리안 군주 분대

        Prince              = 1000030,  //프린스
        PrinceSquad         = 1000031,  //프린스 분대(2대)
        PrinceArmy          = 1000032,  //프린스 군대(3대)

        Valkyrie            = 1000033,  //발키리
        ValkyrieSquad       = 1000034,  //발키리 분대(2대)
        ValkyrieArmy        = 1000035,  //발키리 부대(3대)

        Bowler              = 1000036,  //볼러
        BowlerSquad         = 1000037,  //볼러 분대(2대)
        BowlerArmy          = 1000038,  //볼러 부대(3대)

        Colossus            = 1000039,  //거신
        ColossusSquad       = 1000040,  //거신 분대(2대)
        ColossusArmy        = 1000041,  //거신 부대(3대)

        Xbow                = 1000042,  //엑스보우
        XbowSquad           = 1000043,  //엑스보우 분대(3대)
        XbowArmy            = 1000044,  //엑스보우 부대(3대)
        
        Giant               = 1000045,  //자이언트
        GiantSquad          = 1000046,  //자이언트 분대(2대)

        RoyalGiant          = 1000047,  //로얄 자이언트
        RoyalGiantSquad     = 1000048,  //로얄 자이언트 분대(2대)

        GiantSkeleton       = 1000049,  //자이언트 스켈레톤
        GiantSkeletonSquad  = 1000050,  //자이언트 스켈레톤 분대(2대)

        InfernoDragon       = 1000051,  //인페르노 드래곤
        InfernoDragonSquad  = 1000052,  //인페르노 드래곤 분대(2대)
        InfernoDragonArmy   = 1000053,  //인페르노 드래곤 부대(3대)

        Bomber              = 1000054,  //폭탄병
        BomberSquad         = 1000055,  //폭탄병 분대(2대)
        BomberArmy          = 1000056,  //폭탄병 부대(3대)

        ElectricWizard      = 1000057,  //전기 마법사
        ElectricWizardSquad = 1000058,  //전기 마법사 분대(2대)
        ElectricWizardArmy  = 1000059,  //전기 마법사 부대(3대)

        Guardian            = 1000060,  //가디언
        GuardianSquad       = 1000061,  //가디언 분대(2대)

        Paladins            = 1000062,  //팔라딘
        PaladinsSquad       = 1000063,  //팔라딘 분대(2대)

        BroodLord           = 1000064,  //무리군주(+수리선)
        BroodLordSquad      = 1000065,  //무리군주(+수리선) 분대(2대)

        ScienceVessle       = 1000066,  //사이언스 베슬
        ScienceVessleSquad  = 1000067,  //사이언스 베슬 분대(2대)

        Probe               = 1000068,  //프로브
        ProbeSquad          = 1000069,  //프로브 분대(2대)
        ProbeArmy           = 1000070,  //프로브 부대(3대)

        CommandCenter       = 1000071,  //커맨드 센터
        CommandCenterSquad  = 1000072,  //커맨드 센터 분대(2대)
        CommandCenterArmy   = 1000073,  //커맨드 센터 부대(3대)

        Commando            = 1000074,  //상륙함
        CommandoSquad       = 1000075,  //상륙함 분대(2대)
        CommandoArmy        = 1000076,  //상륙함 부대(3대) 

        Tester              = 9999999
    }

    public enum RandomRange
    {
        OverHund,           //100자리로 확률 뽑아냄
        OverHundUnderOne,   //소수점 한자리수까지 확률 뽑아냄
        OverHundUnderTwo    //소수점 두자리수까지 확률 뽑아냄
    }
    
    //PatchList.Xml과 연결되어 있어서 상수값을 바꾸면 안된다.
    public enum SceneState
    {
        None,
        Title,
        Lobby,
        Stage
    }

    //PatchList.Xml과 연결되어 있어서 상수값을 바꾸면 안된다.
    public enum ResType
    {
        Common,
        Title,
        Lobby,
        Stage
    }

    public enum Grade
    {        
        Common,         //흔한
        Uncommon,       //고급
        Rare,           //희귀
        Epic,           //영웅
        Legendary,      //전설
        Artifact        //유물
    }

    public enum ResourceType
    {
        Gold,
        Material,
        Cristal
    }
}
