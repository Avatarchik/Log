using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonEnum;

public class Ship : MonoBehaviour
{
    public enum State
    {
        None,       //정지        
        Move,       //적을 탐색하며 움직입니다.
        Aim,        //범위안에 적이 들어왔고 조준중입니다.
        Attack,     //목표물을 공격 중입니다.
    }

    public enum Debuff
    {
        Electricity
    }

    class DebuffInfo
    {
        public Debuff debuffType;
        public ParticleSystem particle;
    }

    ////////////////////////////////////////////////////////
    //배 상세 정보
    [HideInInspector]
    public Model kModel = Model.None;
    [HideInInspector]
    public Grade kRarity = Grade.Common;
    [HideInInspector]
    public string kClassName = "";
    [HideInInspector]
    public int kClassLevel = 1;
    ////////////////////////////////////////////////////////

    [HideInInspector]
    public Weapon kWeapon;
            
    Renderer[] mRendererList;
    float mTotalDamageDpTime = 0.3f;
    float mCurDamageDpTime = 0.0f;

    ////////////////////////////////////////////////////////
    //생명력 관련
    //float mShieldRecoveryTime = 5.0f;
    //public float kShieldRecoveryAmount = 10;
    [HideInInspector]
    public int kCurHealthPoint = 0;
    [HideInInspector]
    public int kCurShieldPoint = 0;
    //내구력
    public int kTotalHealthPoint = 100;
    //방어력
    public int kTotalShieldPoint = 100;
    ////////////////////////////////////////////////////////    

    ////////////////////////////////////////////////////////
    //함선 움직임 관련    
    //선회력
    public float kTurningForce = 0.3f;
    //추가 선회력
    float mAddTurningForce = 0.0f;
    //현재 속력
    protected float mCurVelocity = 0.0f;
    //최대 속력
    public float kMaxVelocity = 0.05f;
    //추가 최대 속력
    float mAddMaxVelocity = 0.0f;
    ////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////
    //함선 AI 관련
    [HideInInspector]
    public State kState = State.None;
    
    protected Ship mAttackTarget = null;
    protected Ship mMoveTarget = null;

    [HideInInspector]
    public bool kIsDie = false;
    //전투 시작함
    [HideInInspector]
    public bool kIsEngage = false;
    ////////////////////////////////////////////////////////    

    ////////////////////////////////////////////////////////
    //영역 관련
    //쉴드 충돌 영역
    [HideInInspector]
    public SphereCollider kShieldCollider;
    //본체 충돌 영역
    [HideInInspector]
    public CapsuleCollider kBodyCollider;
    //공격범위
    [HideInInspector]
    public SphereCollider kAttackCollider;
    ////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////
    //그룹 정보
    [HideInInspector]
    public bool kIsPlayerGroup = true;
    [HideInInspector]    
    UIGroupInfo mGroupInfo;

    protected List<Ship> mTargetShipList = null;
    protected List<Ship> mOurShipList = null;
    ////////////////////////////////////////////////////////

    GameObject mUnitMark;

    ParticleSystem[] mEngineParticleSys;

    protected Vector3 mStartingPos;

    List<DebuffInfo> mDebuffList = new List<DebuffInfo>();

    //전투 로직을 수행하는 할당 아이디
    public int kAllocID = 0;
    
    void Awake()
    {
        kShieldCollider = GetComponent<SphereCollider>();
        kShieldCollider.enabled = true;
        kBodyCollider = GetComponent<CapsuleCollider>();
        kBodyCollider.enabled = false;
        kAttackCollider = transform.Find("Weapon").GetComponent<SphereCollider>();
        kAttackCollider.enabled = true;
        mEngineParticleSys = transform.Find("Engine").GetComponentsInChildren<ParticleSystem>();

        //데미지 받을 시 효과 부여하기 위한 랜더링 설정
        mRendererList = transform.GetComponentsInChildren<Renderer>();
        kWeapon = transform.Find("Weapon").GetComponent<Weapon>();
    }

    // Use this for initialization
    void Start()
    {
    }   

    void OnDisable()
    {
        StopAllCoroutines();
        
        kTurningForce = 0.3f;
        mAddTurningForce = 0.0f;
        mCurVelocity = 0.0f;
        kMaxVelocity = 0.05f;
        mAddMaxVelocity = 0.0f;

        kState = State.None;

        mAttackTarget = null;
        mMoveTarget = null;
        
        kIsEngage = false;

        mTargetShipList = null;
        mOurShipList = null;

        if (mDebuffList != null)
            mDebuffList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
    }

    void LateUpdate()
    {
        //데미지 받음을 표시
        DamageDisplayUpdate();
    }
    
    void DamageDisplayUpdate()
    {
        if (mCurDamageDpTime == 0.0f)
            return;
                
        mCurDamageDpTime -= Time.deltaTime;
        
        float ratio = 0.0f;
        if (mCurDamageDpTime <= 0.0f)
        {
            mCurDamageDpTime = 0.0f;
            ratio = 1.0f;
        }
        else
            ratio = (mTotalDamageDpTime - mCurDamageDpTime) / mTotalDamageDpTime;

        for (int i = 0; i < mRendererList.Length; i++)            
            mRendererList[i].material.SetFloat("_MainTexOpacity", ratio);
    }

    public virtual void OnPrepare()
    {
        mStartingPos = transform.position;

        mCurDamageDpTime = 0.0f;
        for (int i = 0; i < mRendererList.Length; i++)
            mRendererList[i].material.SetFloat("_MainTexOpacity", 1.0f);
        
        kWeapon.OnPrepare();
        kWeapon.gameObject.layer = gameObject.layer;

        //mGroupInfo = StageUIRoot.Instance.kGroupInfo;
        
        //아군 그룹 및 타겟 그룹 설정
        if (kIsPlayerGroup == true)
        {
            Transform[] childs = gameObject.GetComponentsInChildren<Transform>();
            for( int i = 0; i < childs.Length; i++)
                childs[i].gameObject.layer = StageDef.LAYER_PLAYER;
            kWeapon.gameObject.layer = 0;
            transform.forward = Vector3.forward;

            gameObject.layer = StageDef.LAYER_PLAYER;

            mTargetShipList = StagePlayManager.Instance.kEnemyShipList;
            mOurShipList = StagePlayManager.Instance.kPlayerShipList;
            
            mUnitMark = ObjectPoolManager.Instance.GetGameObejct(StrDef.UI_PLAYERMARK);
            mUnitMark.transform.parent = transform;
            mUnitMark.transform.localPosition = new Vector3(0.0f, -3.0f, 0.0f);
            mUnitMark.transform.localScale = Vector3.one * (kBodyCollider.height / 8.0f);
            mUnitMark.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Transform[] childs = gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childs.Length; i++)
                childs[i].gameObject.layer = StageDef.LAYER_ENEMY;
            kWeapon.gameObject.layer = 0;
            transform.forward = -Vector3.forward;

            gameObject.layer = StageDef.LAYER_ENEMY;

            mTargetShipList = StagePlayManager.Instance.kPlayerShipList;
            mOurShipList = StagePlayManager.Instance.kEnemyShipList;

            mUnitMark = ObjectPoolManager.Instance.GetGameObejct(StrDef.UI_ENEMYMARK);
            mUnitMark.transform.parent = transform;
            mUnitMark.transform.localPosition = new Vector3(0.0f, -3.0f, 0.0f);
            mUnitMark.transform.localScale = Vector3.one * (kBodyCollider.height / 8.0f);
            mUnitMark.transform.localRotation = Quaternion.identity;
        }

        if (mOurShipList.Contains(this) == false)
            mOurShipList.Add(this);

        kBodyCollider.enabled = false;
        kShieldCollider.enabled = true;

        kIsEngage = StagePlayManager.Instance.IsEngage(kIsPlayerGroup);

        mDebuffList.Clear();

        DT_ShipData_Info info = CDT_ShipData_Manager.Instance.GetInfo((int)kModel);
        kTurningForce = info.TurnForce;
        kMaxVelocity = info.MaxVelocity;
        kAttackCollider.radius = info.AttackRange;
        kTotalHealthPoint = info.BodyAmount;
        kTotalShieldPoint = info.ShieldAmount;
        kWeapon.kTotalChargeCoolTime = info.AttackCoolTime;
        kWeapon.kTotalFireCoolTime = info.LaunchDelay;
        kWeapon.kAttackMax = info.MaxAttack;
        kWeapon.kAttackMin = info.MinAttack;

        kCurHealthPoint = kTotalHealthPoint;
        kCurShieldPoint = kTotalShieldPoint;

        if( kIsPlayerGroup == true )
            StagePlayManager.Instance.kPlayerTotalArmor += kTotalHealthPoint + kTotalShieldPoint;
        else
            StagePlayManager.Instance.kEnemyTotalArmor += kTotalHealthPoint + kTotalShieldPoint;

        mAddMaxVelocity = 0.0f;
        mAddTurningForce = 0.0f;
        mCurVelocity = 0.0f;

        kState = State.Move;        

        mAttackTarget = null;
        mMoveTarget = null;
        
        kIsDie = false;
        mIsSpeedUp = true;
        mOldSpeedState = false;
        /*
        if (StagePlayManager.Instance != null)
            mGroupInfo.SetShip(this);
        */

        ActionManager.Instance.AddQueue(MoveUpdate);
        //쉴드 재생
        //InvokeRepeating("ShieldRecovery", mShieldRecoveryTime, mShieldRecoveryTime);
    }

    public float TotalShieldHealth()
    {
        return kCurHealthPoint + kCurShieldPoint;
    }

    public void SumHealthPoint(int _sumHealthPoint)
    {
        int oldHealthPoint = kCurHealthPoint;
        kCurHealthPoint = oldHealthPoint + _sumHealthPoint;

        if (kCurHealthPoint > kTotalShieldPoint)
            kCurHealthPoint = kTotalShieldPoint;        
    }

    public bool IsEnergyFull()
    {
        if (kCurHealthPoint == kTotalHealthPoint &&
            kCurShieldPoint == kTotalShieldPoint)
            return true;

        return false;
    }

    public void SumShieldPoint(int _sumShieldPoint, bool _isTypo = false)
    {        
        kCurShieldPoint += _sumShieldPoint;

        if (kCurShieldPoint > kTotalShieldPoint)
            kCurShieldPoint = kTotalShieldPoint;
        if (kCurShieldPoint < 0)
            kCurShieldPoint = 0;

        if (kCurShieldPoint > 0)
        {
            kShieldCollider.enabled = true;
            kBodyCollider.enabled = false;
        }
        else
        {
            kShieldCollider.enabled = false;
            kBodyCollider.enabled = true;
        }
    }

    public void EngineActive(bool _isAct)
    {
        if (_isAct == true)
        {
            for (int i = 0; i < mEngineParticleSys.Length; i++)
                if (mEngineParticleSys[i].isPlaying == false)
                    mEngineParticleSys[i].Play();
        }
        else
        {
            for (int i = 0; i < mEngineParticleSys.Length; i++)
                if (mEngineParticleSys[i].isPlaying == true)
                    mEngineParticleSys[i].Stop();
        }
    }

    bool mIsSpeedUp = true;
    bool mOldSpeedState = false;
    public virtual void VelocityUpdate()
    {
        if (kState == State.None || kState == State.Aim || kState == State.Attack)
            mIsSpeedUp = false;

        if (kState == State.Move)
            mIsSpeedUp = true;

        if (mIsSpeedUp == mOldSpeedState)
            return;

        mOldSpeedState = mIsSpeedUp;
        if ( mIsSpeedUp == true )
        {
            StartCoroutine(Accelation());
            EngineActive(true);
        }
        else
        {
            StartCoroutine(Break());
            EngineActive(false);
        }
    }

    public void MoveForward()
    {
        Vector3 forwardDir = Vector3.forward;
        if (kIsPlayerGroup == false)
            forwardDir = -Vector3.forward;
        forwardDir = forwardDir * 10.0f;

        //계속 직진했다면 있어야할 원래의 위치값
        Vector3 groupPos = mStartingPos;
        groupPos.z = transform.position.z + forwardDir.z;
        Move(groupPos - transform.position);
    }

    public virtual void StateUpdate()
    {
        if (mTargetShipList == null)
            return;

        if (mTargetShipList.Count == 0)
            kState = State.Move;        

        switch (kState)
        {
            case State.None:
                EngineActive(false);
                break;            
            case State.Move:
                {
                    //MoveUpdate();
                                            
                    if (mMoveTarget == null)
                        MoveForward();
                    else
                        Move(mMoveTarget.transform.position - transform.position);
                }
                break;
            case State.Aim:
                {
                    AimUpdate();
                    WeaponCharge();

                    if (mAttackTarget != null)
                        Move(mAttackTarget.transform.position - transform.position);
                }
                break;
            case State.Attack:
                {
                    WeaponCharge();
                    WeaponFire();
                    AttackUpdate();

                    if(mAttackTarget != null)
                        Move(mAttackTarget.transform.position - transform.position);
                }
                break;
        }

        VelocityUpdate();
    }

    IEnumerator Accelation()
    {
        float maxVelocity = kMaxVelocity + mAddMaxVelocity;
        float toMaxVelocityTime = (maxVelocity - mCurVelocity) / maxVelocity;
        
        while(toMaxVelocityTime <= 1.0f)
        {
            if( mIsSpeedUp == false )
                yield break;

            toMaxVelocityTime += Time.deltaTime;            
            mCurVelocity = maxVelocity * toMaxVelocityTime;
            if (mCurVelocity > maxVelocity)
                mCurVelocity = maxVelocity;
            yield return null;
        }
        
        yield break;
    }

    IEnumerator Break()
    {
        float maxVelocity = kMaxVelocity + mAddMaxVelocity;
        float toZeroVelocityTime = mCurVelocity / maxVelocity;

        while (toZeroVelocityTime >= 0.0f)
        {
            if ( mIsSpeedUp == true )
                yield break;

            toZeroVelocityTime -= Time.deltaTime;
            mCurVelocity = maxVelocity * toZeroVelocityTime;
            if (mCurVelocity < 0.0f)
                mCurVelocity = 0.0f;

            yield return null;
        }

        yield break;
    }
    
    protected void Move(Vector3 _dir)
    {        
        transform.forward = Vector3.MoveTowards(transform.forward, _dir, Time.deltaTime * (kTurningForce + mAddTurningForce));

        if (StagePlayManager.Instance != null)
            transform.position += transform.forward * mCurVelocity * Time.deltaTime;
    }
    
    public void MoveUpdate()
    {
        if (kState != State.Move)
            return;

        if (mTargetShipList == null)
            return;

        Ship targetShip = null;
        mAttackTarget = null;

        float searchRange = Mathf.Infinity;
        if (kIsEngage == false)
            searchRange = kAttackCollider.radius;

        for (int i = 0; i < mTargetShipList.Count; i++)
        {
            Ship ship = mTargetShipList[i];

            if (ship.kIsDie == true)
                continue;

            float curDist = Vector3.Distance(transform.position, ship.transform.position);
            if (curDist > searchRange)
                continue;
            
            if (targetShip == null)
                targetShip = ship;

            float oldDist = Vector3.Distance(transform.position, targetShip.transform.position);
            if (curDist <= oldDist)
                targetShip = ship;
        }

        mMoveTarget = targetShip;

        if (targetShip == null)
            return;

        float targetShipDist = Vector3.Distance(transform.position, targetShip.transform.position);
        if (targetShipDist <= kAttackCollider.radius)
        {
            mAttackTarget = targetShip;
            kState = State.Aim;
        }

        if ( StagePlayManager.Instance.IsEngage(kIsPlayerGroup) == false )
            StagePlayManager.Instance.SeGroupEngage(this);
    }

    protected void AimUpdate()
    {
        if (mAttackTarget == null || mAttackTarget.kIsDie == true)
        {
            kState = State.Move;
            mAttackTarget = null;
            kWeapon.Stop();
            return;
        }

        Vector3 dirLook = mAttackTarget.transform.position - transform.position;
        transform.forward = Vector3.MoveTowards(transform.forward, dirLook, Time.deltaTime * kTurningForce);

        if (3.0f > Vector3.Angle(transform.forward, dirLook))
        {
            kState = State.Attack;
            kWeapon.SetTarget(mAttackTarget.transform);
        }
    }

    protected void AttackUpdate()
    {
        if (mAttackTarget == null || mAttackTarget.kIsDie == true)
        {
            kState = State.Aim;
            mAttackTarget = null;
            kWeapon.Stop();
            return;
        }            

        float curDist = Vector3.Distance(transform.position, mAttackTarget.transform.position);
        if (curDist > kAttackCollider.radius)
        {
            kState = State.Aim;
            mAttackTarget = null;            
            kWeapon.Stop();
            return;
        }        
    }

    public void WeaponCharge()
    {
        kWeapon.ChargeUpdate();
    }
         
    public void WeaponFire()
    {
        kWeapon.FireUpdate();
    }
            
    void ReturnWhiteColor()
    {
        for (int i = 0; i < mRendererList.Length; i++)
            mRendererList[i].material.SetColor("_Color", Color.white);
    }
    
    public void Damaged(int _damage)
    {
        if (kIsDie == true)
            return;
        
        int toShieldDamage = 0;
        int toBodyDamage = 0;

        if (kCurShieldPoint > _damage)
            toShieldDamage = _damage;
        else
        {
            toBodyDamage = _damage - kCurShieldPoint;
            toShieldDamage = kCurShieldPoint;
        }

        if(toShieldDamage > 0)
            SumShieldPoint(-_damage);

        if (toBodyDamage > 0)
            SumHealthPoint(-toBodyDamage);

        //StageUIRoot.Instance.TypoMessage(transform.position, _damage.ToString(), UITypoText.Type.Damage);

        //StageUIRoot.Instance.TypoMessage(transform.position, StringUtil.FloatToPlus(amount), UITypoText.Type.Recovery);

        mCurDamageDpTime = mTotalDamageDpTime;

        //나머지는 데미지는 함선의 체력에서 제외한다.
        if (kCurHealthPoint <= 0.0f)
            Die();        
    }

    public virtual void Die()
    {
        if (kIsPlayerGroup == true)
            SoundManager.Instance.BattleVoice(StageEnum.BattleSign.PlayerDestory); 
        else
            SoundManager.Instance.BattleVoice(StageEnum.BattleSign.EnemyDestroy);

        mOurShipList.Remove(this);

        ObjectPoolParticle explosion = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_EXPLOSIONBIG).GetComponent<ObjectPoolParticle>();
        explosion.transform.position = transform.position;

        int randomSound = Random.Range(2000006, 2000008);
        SoundManager.Instance.PlayEffect(randomSound);

        explosion.Play();
        kIsDie = true;

        ObjectPoolManager.Instance.Release(mUnitMark);

        for (int i = 0; i < mDebuffList.Count; i++)
            ObjectPoolManager.Instance.Release(mDebuffList[i].particle.gameObject);
        mDebuffList.Clear();

        kWeapon.Stop();
        ObjectPoolManager.Instance.Release(gameObject);
    }

    bool mIsResetElectric = false;

    public void AddDebuff(Debuff _debuff)
    {
        /*
        for( int i = 0; i < mDebuffList.Count; i++)
        {
            DebuffInfo info = mDebuffList[i];
            if( info.debuffType == _debuff )
            {
                switch (_debuff)
                {
                    case Debuff.Electricity:
                        mIsResetElectric = true;
                        return;
                }
            }
        }
        */
        DebuffInfo debuffinfo = new DebuffInfo();

        switch (_debuff)
        {
            case Debuff.Electricity:
                {   
                    StartCoroutine(ElectricityTime());
                    ParticleSystem hitParticle = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_ELECTRICITY).GetComponent<ParticleSystem>();                    
                    ParticleSystem.ShapeModule module = hitParticle.shape;

                    if ( kBodyCollider.direction == 2 )
                        module.box = new Vector3(kBodyCollider.radius, kBodyCollider.radius, kBodyCollider.height);
                    else
                        module.box = new Vector3(kBodyCollider.radius, kBodyCollider.height, kBodyCollider.radius);

                    hitParticle.transform.parent = transform;
                    hitParticle.transform.localPosition = Vector3.zero;
                    hitParticle.transform.localScale = Vector3.one;
                    hitParticle.transform.localRotation = Quaternion.identity;
                    debuffinfo.particle = hitParticle;
                }
                break;
        }

        debuffinfo.debuffType = _debuff;
        mDebuffList.Add(debuffinfo);
    }

    void RemoveDebuff(Debuff _debuff)
    {
        for (int i = 0; i < mDebuffList.Count; i++)
        {
            DebuffInfo info = mDebuffList[i];
            if (info.debuffType == Debuff.Electricity)
            {
                ObjectPoolManager.Instance.Release(info.particle.gameObject);
                mDebuffList.Remove(info);
                return;
            }
        }
    }

    IEnumerator ElectricityTime()
    {
        float curTime = 0.0f;

        float addChargeCoolTime = kWeapon.kTotalChargeCoolTime * 0.5f;
        kWeapon.kAddChargeCoolTime += addChargeCoolTime;
        float addFireCoolTime = kWeapon.kAddFireCoolTime * 0.5f;
        kWeapon.kAddFireCoolTime += addFireCoolTime;
        float addTurningForce = kTurningForce * 0.5f;
        mAddTurningForce -= addTurningForce;
        float addMaxVelocity = kMaxVelocity * 0.5f;
        mAddMaxVelocity -= addMaxVelocity;

        while (curTime < 5.0f)
        {
            curTime += Time.deltaTime;
            yield return null;
        }

        mAddTurningForce += addTurningForce;
        mAddMaxVelocity += addMaxVelocity;
        kWeapon.kAddChargeCoolTime -= addChargeCoolTime;        
        kWeapon.kAddFireCoolTime -= addFireCoolTime;
        RemoveDebuff(Debuff.Electricity);

        yield break;
    }
}
