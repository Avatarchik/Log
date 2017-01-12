using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Photon : Projectile{
    public float kDuration = 5.0f;
    //투사체의 처음 크기
    public float kOriginScale = 6.0f;
    float mOriginDamage = 0.0f;

    //광자포의 데미지 주기
    public float mDamageCycleTime = 0.3f;
    //한프레임에 충돌하고 있는 적들
    List<Ship> mCollisionShipList = new List<Ship>();
    //처음으로 적들과 부딪힘
    bool mIsTrigger = false;
    //현재 데미지 주기 타이머
    float mCurDamageCycleTime = 0.0f;
    //투사체 전진 방향
    Vector3 mForwardDir;

    SphereCollider mCollider;
    
    protected override void OnTargetHit() { }
    protected override void MissingTargetUpdate() { }

    void Awake()
    {
        mCollider = GetComponent<SphereCollider>();
    }

    public override void SetLaunch(Ship _target, Ship _attacker, Transform _launcher)
    {
        base.SetLaunch(_target, _attacker, _launcher);

        mForwardDir = (mTargetShip.transform.position - _launcher.transform.position).normalized;
        
        transform.localScale = Vector3.one;
        mIsTrigger = false;
        mCollider.enabled = true;
        
        StartCoroutine(DurationUpdate());
    }
    
    IEnumerator DurationUpdate()
    {
        float curTime = 0.0f;
        while(curTime < kDuration)
        {
            curTime += Time.deltaTime;
            yield return null;
        }

        //소멸
        mCollider.enabled = false;
        curTime = 0.3f;
        float totalTime = 0.3f;
        while(curTime > 0.0f)
        {
            curTime -= Time.deltaTime;
            float distCustomRatio = curTime / totalTime;
            transform.localScale = Vector3.one * kOriginScale * distCustomRatio;
            yield return null;
        }
        
        Release();

        yield break;
    }

    // Update is called once per frame
    protected override void MoveUpdate()
    {
        transform.position += mForwardDir * Time.deltaTime * mVelocity;
    }
    
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.layer == mTargetShip.gameObject.layer)
        {
            mCollisionShipList.Add(col.GetComponent<Ship>());
            if (mIsTrigger == false)
                mCurDamageCycleTime = mDamageCycleTime;

            mIsTrigger = true;
        }
    }

    void LateUpdate()
    {
        if (mIsTrigger == false)
            return;

        mCurDamageCycleTime += Time.deltaTime;
        if( mCurDamageCycleTime >= mDamageCycleTime)
        {
            for (int i = 0; i < mCollisionShipList.Count; i++)
            {
                if (mCollisionShipList[i].kShieldCollider.enabled == true)
                {
                    Shield shield = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_SHIELD).GetComponent<Shield>();
                    shield.Play();
                    shield.transform.forward = -transform.forward;
                    shield.transform.position = transform.position - transform.forward;
                }
                int damage = Random.Range(mMinDamage, mMaxDamage + 1);
                mCollisionShipList[i].Damaged(damage);
            }
            mCurDamageCycleTime = 0.0f;
        }
        mCollisionShipList.Clear();
    }
}
