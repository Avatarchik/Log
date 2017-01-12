using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public enum Type
    {
        None = -1,
        Cannon,
        Laser,
        ElectricityLaser,
        Missile,
        HitRay,
        InfernoRay,
        Photon,
        Skeleton,
        ClusterMissile
    }

    public enum SpeedType
    {
        VerySlow,
        Slow,
        Normal,
        Fast,
        VeryFast
    }

    public Type kWeaponModel;

    protected Ship mTargetShip;
    protected Ship mAttacker;
    protected Transform mLauncher;

    protected int mMinDamage = 0;
    protected int mMaxDamage = 0;

    public SpeedType kSpeedType = SpeedType.Normal;

    protected float mVelocity = 10.0f; 

    protected bool mIsMissingTarget = false;
    protected Vector3 mMissingTargetPos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        MissingTargetUpdate();
        MoveUpdate();
    }

    protected virtual void MissingTargetUpdate()
    {
        if (mIsMissingTarget == false && mTargetShip.kIsDie == true)
        {
            mIsMissingTarget = true;
            mMissingTargetPos = mTargetShip.transform.position;
        }

        if (mIsMissingTarget == true)
        {
            if (mMissingTargetPos == transform.position)
            {
                Hit();
                Release();
                return;
            }
        }
    }

    protected virtual void MoveUpdate()
    {
        Vector3 oldPos = transform.position;

        if (mIsMissingTarget == false)
            transform.position = Vector3.MoveTowards(transform.position, mTargetShip.transform.position, Time.deltaTime * mVelocity);
        else
            transform.position = Vector3.MoveTowards(transform.position, mMissingTargetPos, Time.deltaTime * mVelocity);

        if (transform.position == oldPos)
            return;

        transform.forward = transform.position - oldPos;
    }
    
    public virtual void SetLaunch(Ship _target, Ship _attacker, Transform _launcher)
    {
        switch(kSpeedType)
        {
            case SpeedType.VerySlow:
                mVelocity = 40.0f;
                break;
            case SpeedType.Slow:
                mVelocity = 50.0f;
                break;
            case SpeedType.Normal:
                mVelocity = 60.0f;
                break;
            case SpeedType.Fast:
                mVelocity = 70.0f;
                break;
            case SpeedType.VeryFast:
                mVelocity = 80.0f;
                break;
        }
        
        mLauncher = _launcher;
        mTargetShip = _target;
        mAttacker = _attacker;
        mIsMissingTarget = false;

        transform.position = _launcher.position;
    }

    public void SetWeaponType(int _minDamage, int _maxDamage)
    {
        mMinDamage = _minDamage;
        mMaxDamage = _maxDamage;
    }

    protected virtual void OnTargetHit()
    {
        Hit();

        if (mTargetShip.kShieldCollider.enabled == true)
        {
            Shield shield = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_SHIELD).GetComponent<Shield>();
            shield.Play();
            shield.transform.forward = -transform.forward;
            shield.transform.position = transform.position - transform.forward;
        }

        if (mTargetShip.kIsDie == false)
        {
            int damage = Random.Range(mMinDamage, mMaxDamage + 1);
            mTargetShip.Damaged(damage);
        }

        Release();
    }

    void OnTriggerEnter(Collider col)
    {
        if (mTargetShip == null)
            return;

        if (col.transform == mTargetShip.transform)
        {
            OnTargetHit();
        }
    }
    
    protected void Hit()
    {
        ObjectPoolParticle hitParticle = null;

        switch (kWeaponModel)
        {
            case Type.Laser:
                hitParticle = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_EXPLOSIONSMALL).GetComponent<ObjectPoolParticle>();
                break;
            case Type.Cannon:
                hitParticle = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_SPARKSMALL).GetComponent<ObjectPoolParticle>();
                break;
            case Type.Missile:
                hitParticle = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_EXPLOSIONMEDIUM).GetComponent<ObjectPoolParticle>();
                break;
            case Type.ElectricityLaser:
                hitParticle = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_EXPLOSIONMEDIUM).GetComponent<ObjectPoolParticle>();
                break;
            case Type.ClusterMissile:
                hitParticle = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_EXPLOSIONFRAGMENT).GetComponent<ObjectPoolParticle>();
                break;
            default:
                return;
        }

        hitParticle.Play();
        hitParticle.transform.position = transform.position;
    }

    public void Release()
    {
        ObjectPoolManager.Instance.Release(gameObject);
    }
}
