using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
    
    public enum State
    {
        None,
        Charing,
        ChargeComplete,
        Firing,
    }
    
    State mState = State.None;

    Ship mTargetShip = null;
    Ship mOwnerShip;

    List<Transform> mLauncherList = new List<Transform>();
    List<bool> mLockIndexList = new List<bool>();

    public float kTotalChargeCoolTime = 3.0f;
    [HideInInspector]
    public float kAddChargeCoolTime = 0.0f;
    
    public float kTotalFireCoolTime = 0.2f;
    [HideInInspector]
    public float kAddFireCoolTime = 0.0f;
        
    public int kAttackMin = 0;
    public int kAttackMax = 0;
    float mToShieldDamagPer = 0.0f;
    float mToBodyDamagePer = 0.0f;

    float mShipRadius = 0.0f;

    [HideInInspector]
    public int kLevel = 1;
    [HideInInspector]
    public int kPhysicalLevel = 1;
    
    public Projectile.Type kProjectileType;

    bool mIsRecallType = false;
    bool mIsLockType = false;
    
    void Awake()
    {
    }

    // Use this for initialization
    void Start() {        
        mShipRadius = TransformUtil.TopParentComponent(transform, "Ship").GetComponent<CapsuleCollider>().radius;
        mOwnerShip = TransformUtil.TopParentComponent(transform, "Ship").GetComponent<Ship>();
    }
	
    void OnDisable()
    {
        StopAllCoroutines();
    }

	// Update is called once per frame
	void Update () {
    }
    
    public void OnPrepare()
    {
        kAddChargeCoolTime = 0.0f;
        kAddFireCoolTime = 0.0f;
        mLauncherList.Clear();
        mLockIndexList.Clear();
        mState = State.None;

        Stop();
        for (int i = 0; i < transform.childCount; i++)
        {
            mLauncherList.Add(transform.Find("Launcher" + i.ToString()));
            mLockIndexList.Add(false);
        }

        if (kProjectileType == Projectile.Type.Skeleton)
            mIsRecallType = true;
        if (kProjectileType == Projectile.Type.InfernoRay)
            mIsLockType = true;
    }
    
    public void ChargeUpdate()
    {
        if (mState != State.None)
            return;
        if (kProjectileType == Projectile.Type.None)
            return;

        StartCoroutine(Charge());
    }
    
    IEnumerator Charge()
    {
        mState = State.Charing;
        float curChargetCoolTime = 0.0f;
        while (curChargetCoolTime < kTotalChargeCoolTime + kAddChargeCoolTime)
        {
            if (mState == State.None)
                yield break;

            curChargetCoolTime += Time.deltaTime;
            
            yield return null;
        }
        
        mState = State.ChargeComplete;
    }

    public void FireUpdate()
    {
        if (kProjectileType != Projectile.Type.Skeleton)
            if (mTargetShip == null)
                return;

        if (mState != State.ChargeComplete)
            return;

        StartCoroutine(Fire());
    }

    public void Stop()
    {
        mState = State.None;
        AllUnlockLauncher();
        StopCoroutine(Fire());
        StopCoroutine(Charge());
    }

    public void SetTarget(Transform _target)
    {
        mTargetShip = _target.GetComponent<Ship>();
    }

    public void UnlockLauncher(int _index)
    {
        mLockIndexList[_index] = false;
    }

    void AllUnlockLauncher()
    {
        for(int i = 0; i < mLockIndexList.Count; i++)
            mLockIndexList[i] = false;
    }

    IEnumerator Fire()
    {
        mState = State.Firing;

        int launchOrder = 0;
        while (launchOrder < mLauncherList.Count)
        {
            float curCoolTime = 0.0f;

            if (mLockIndexList[launchOrder] == true)
            {
                yield return null;
                continue;
            }

            while (curCoolTime < kTotalFireCoolTime + kAddFireCoolTime)
            {
                if (mState == State.None)
                    yield break;

                curCoolTime += Time.deltaTime;
                yield return null;
            }

            Transform launcherTrans = mLauncherList[launchOrder];
            if (mIsRecallType == false)
            {                
                ObjectPoolParticle muzzle = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_MUZZLE).GetComponent<ObjectPoolParticle>();
                muzzle.transform.position = launcherTrans.position;
                muzzle.Play();
            }
            
            Projectile projectile = null;
            switch (kProjectileType)
            {
                case Projectile.Type.Cannon:
                    {
                        projectile = ObjectPoolManager.Instance.GetGameObejct(StrDef.PROJECTILE_CANNON).GetComponent<Projectile>();
                        if( mOwnerShip.kIsPlayerGroup == true)
                            SoundManager.Instance.PlayEffect(2000001);
                    }
                    break;
                case Projectile.Type.Missile:
                    {
                        projectile = ObjectPoolManager.Instance.GetGameObejct(StrDef.PROJECTILE_MISSILE).GetComponent<Projectile>();
                        if (mOwnerShip.kIsPlayerGroup == true)
                            SoundManager.Instance.PlayEffect(2000002);
                    }
                    break;
                case Projectile.Type.Laser:
                    {
                        projectile = ObjectPoolManager.Instance.GetGameObejct(StrDef.PROJECTILE_LASER).GetComponent<Projectile>();
                        if (mOwnerShip.kIsPlayerGroup == true)
                            SoundManager.Instance.PlayEffect(2000003);
                    }
                    break;
                case Projectile.Type.ElectricityLaser:
                    {
                        projectile = ObjectPoolManager.Instance.GetGameObejct(StrDef.PROJECTILE_ELECTRICITYLASER).GetComponent<Projectile>();
                        if (mOwnerShip.kIsPlayerGroup == true)
                            SoundManager.Instance.PlayEffect(2000004);
                    }
                    break;
                case Projectile.Type.HitRay:
                    projectile = ObjectPoolManager.Instance.GetGameObejct(StrDef.PROJECTILE_HITRAY).GetComponent<Projectile>();
                    break;
                case Projectile.Type.Photon:
                    projectile = ObjectPoolManager.Instance.GetGameObejct(StrDef.PROJECTILE_PHOTON).GetComponent<Projectile>();
                    break;
                case Projectile.Type.InfernoRay:
                    projectile = ObjectPoolManager.Instance.GetGameObejct(StrDef.PROJECTILE_INFERNORAY).GetComponent<Projectile>();
                    break;
                case Projectile.Type.ClusterMissile:
                    {
                        projectile = ObjectPoolManager.Instance.GetGameObejct(StrDef.PROJECTILE_CLUSTERMISSILE).GetComponent<Projectile>();
                        if (mOwnerShip.kIsPlayerGroup == true)
                            SoundManager.Instance.PlayEffect(2000005);
                    }
                    break;
                case Projectile.Type.Skeleton:
                    {
                        Ship ship = ObjectPoolManager.Instance.GetGameObejct(StrDef.SHIP_SKELETON).GetComponent<Ship>();
                        ship.kModel = CommonEnum.Model.Skeleton;
                        ship.kIsPlayerGroup = mOwnerShip.kIsPlayerGroup;
                        ship.transform.forward = mOwnerShip.transform.forward;
                        ship.transform.position = launcherTrans.position;
                        ship.OnPrepare();
                        
                        Skeleton skeleton = ship.GetComponent<Skeleton>();
                        skeleton.WaitingForLaunch(launcherTrans.forward);
                    }
                    break;
                default:
                    Debug.Log("선택된 무기 타입이 없습니다.");
                    break;
            }

            if (mIsLockType == true)
                mLockIndexList[launchOrder] = true;

            launchOrder++;

            if (projectile == null)
                continue;
                        
            projectile.kWeaponModel = kProjectileType;

            ParticleSystem particle = projectile.GetComponent<ParticleSystem>();
            if (particle != null)
                particle.Play();
            ParticleSystem[] particles = projectile.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particles.Length; i++)
                particles[i].Play();

            projectile.transform.position = launcherTrans.position;

            int minDamage = (int)((float)kAttackMin / (float)mLauncherList.Count);
            int maxDamage = (int)((float)kAttackMax / (float)mLauncherList.Count);
            projectile.SetWeaponType(minDamage, maxDamage);
            projectile.SetLaunch(mTargetShip, mOwnerShip, launcherTrans);
        }

        mState = State.None;

        yield break;
    }
    
    void OnDrawGizmos()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawCube(transform.GetChild(i).position, Vector3.one);
        }
    }
}
