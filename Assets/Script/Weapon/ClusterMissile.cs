using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClusterMissile : Projectile {    
    
    int mMaskLayer;
    public float kDamageRadius = 15.0f;

    List<Ship> mTargetShipList;
    Ray mHitTestRay = new Ray();

    CapsuleCollider mCollider;
    TrailRenderer mTrail;
    GameObject mModel;

    void Awake()
    {
        mModel = transform.Find("Model").gameObject;
        mTrail = transform.Find("Trail").GetComponent<TrailRenderer>();
        mCollider = GetComponent<CapsuleCollider>();
    }

    public override void SetLaunch(Ship _target, Ship _attacker, Transform _launcher)
    {
        base.SetLaunch(_target, _attacker, _launcher);

        mCollider.enabled = true;
        mTrail.Clear();
        mModel.gameObject.SetActive(true);

        if (mTargetShip.kIsPlayer == true)
        {
            mTargetShipList = StagePlayManager.Instance.kPlayerShipList;
            mMaskLayer = 1 << StageDef.LAYER_PLAYER;
        }
        else
        {
            mTargetShipList = StagePlayManager.Instance.kEnemyShipList;
            mMaskLayer = 1 << StageDef.LAYER_ENEMY;
        }   
    }

    protected override void MissingTargetUpdate()
    {
        if (mCollider.enabled == false)
            return;

        if (mIsMissingTarget == false && mTargetShip.kIsDie == true)
        {
            mIsMissingTarget = true;
            mMissingTargetPos = mTargetShip.transform.position;
        }

        //대상을 잃어버린 경우에도 범위공격으로 판단한다.
        if (mIsMissingTarget == true)
        {
            if (mMissingTargetPos == transform.position)
            {
                OnTargetHit();
                return;
            }
        }
    }

    protected override void OnTargetHit()
    {
        Hit();
        
        mHitTestRay.origin = transform.position;

        for ( int i = 0; i < mTargetShipList.Count; i++ )
        {
            Ship targetShip = mTargetShipList[i];

            mHitTestRay.direction = targetShip.transform.position - transform.position;
            RaycastHit[] hits = Physics.RaycastAll(mHitTestRay, kDamageRadius, mMaskLayer);

            for( int n = 0; n < hits.Length; n++)
            {
                if (hits[n].transform == targetShip.transform)
                {
                    ObjectPoolParticle hitParticle = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_EXPLOSIONMEDIUM).GetComponent<ObjectPoolParticle>();
                    hitParticle.transform.position = hits[n].point;
                    hitParticle.Play();

                    if (targetShip.kShieldCollider.enabled == true)
                    {
                        Shield shield = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_SHIELD).GetComponent<Shield>();
                        shield.Play();
                        shield.transform.forward = -transform.forward;
                        shield.transform.position = transform.position - transform.forward;
                    }

                    if (targetShip.kIsDie == false)
                    {
                        int damage = Random.Range(mMinDamage, mMaxDamage + 1);
                        targetShip.Damaged(damage);
                    }
                }
            }
        }

        mModel.gameObject.SetActive(false);
        mCollider.enabled = false;
        Invoke("ReleaseReserve", mTrail.time);
    }

    void ReleaseReserve()
    {
        Release();
    }
}
