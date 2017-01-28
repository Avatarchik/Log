using UnityEngine;
using System.Collections;

public class HitRay : Projectile {
    
    Ray mTargetRay = new Ray();
    int mMaskLayer;
    
    LineRenderer mLineRenderer;
    
    enum State
    {
        Move,
        Hit,
        Missing
    }

    State mState = State.Move;

    // Use this for initialization
    void Awake() {        
        mLineRenderer = GetComponent<LineRenderer>();
    }

    Vector3 mForwardPos;
    Vector3 mBackwardPos;
    Vector3 mHitPosition;
    Vector3 mMissingPosition;

    ObjectPoolParticle mLauncherImpact;
    ObjectPoolParticle mHitImpact;

    bool mIsFollowRay = false;

    protected override void OnTargetHit() { }
    protected override void MissingTargetUpdate() { }

    void MissingUpdate()
    {
        if (mAttacker.kIsDie == true || mTargetShip.kIsDie == true)
        {
            mState = State.Missing;
            mMissingPosition = mTargetShip.transform.position;
            return;
        }
    }

    // Update is called once per frame
    void Update() {

        MissingUpdate();

        switch (mState)
        {
            case State.Move:
                {
                    if(mLauncherImpact != null)
                        mLauncherImpact.transform.position = mLauncher.position;

                    mTargetRay.origin = mLauncher.position;
                    mTargetRay.direction = mTargetShip.transform.position - mLauncher.position;

                    RaycastHit[] hits = Physics.RaycastAll(mTargetRay, Mathf.Infinity, mMaskLayer);
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform == mTargetShip.transform)
                        {
                            mHitPosition = hits[i].point;
                            break;
                        }
                    }

                    mForwardPos = Vector3.MoveTowards(mForwardPos, mHitPosition, Time.deltaTime * mVelocity);
                    mLineRenderer.SetPosition(1, mForwardPos);

                    if (mForwardPos == mHitPosition)
                    {
                        mState = State.Hit;

                        if (mTargetShip.kShieldCollider.enabled == true)
                        {
                            Shield shield = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_SHIELD).GetComponent<Shield>();
                            shield.Play();
                            shield.transform.forward = -transform.forward;
                            shield.transform.position = mHitPosition - transform.forward * mTargetShip.kShieldCollider.radius;
                        }

                        int damage = Random.Range(mMinDamage, mMaxDamage);
                        mTargetShip.Damaged(damage);
                    }
                }
                break;
            case State.Hit:
                {
                    mTargetRay.origin = mLauncher.position;
                    mTargetRay.direction = mTargetShip.transform.position - mLauncher.position;

                    RaycastHit[] hits = Physics.RaycastAll(mTargetRay, Mathf.Infinity, mMaskLayer);
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform == mTargetShip.transform)
                        {
                            mHitPosition = hits[i].point;
                            break;
                        }
                    }

                    if (mHitImpact == null)
                    {
                        mHitImpact = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_RAYIMPACT).GetComponent<ObjectPoolParticle>();
                        mHitImpact.Play();
                    }
                    mHitImpact.transform.position = mHitPosition;
                }
                break;
            case State.Missing:
                {
                    mForwardPos = Vector3.MoveTowards(mForwardPos, mHitPosition, Time.deltaTime * mVelocity);
                    mLineRenderer.SetPosition(1, mForwardPos);
                    CancelInvoke();
                    mIsFollowRay = true;
                }
                break;
        }

        mLineRenderer.SetPosition(0, mBackwardPos);
        mLineRenderer.SetPosition(1, mForwardPos);

        if (mIsFollowRay == true)
        {
            if (mLauncherImpact != null)
            {
                mLauncherImpact.Release();
                mLauncherImpact = null;
            }

            mBackwardPos = Vector3.MoveTowards(mBackwardPos, mForwardPos, Time.deltaTime * mVelocity);
            if (mBackwardPos == mForwardPos)
            {
                if (mHitImpact != null)
                {
                    mHitImpact.Release();
                    mHitImpact = null;
                }
                Release();
            }
        }
    }    
        
    public override void SetLaunch(Ship _target, Ship _attacker, Transform _launcher)
    {
        base.SetLaunch(_target, _attacker, _launcher);

        mState = State.Move;

        mLauncherImpact = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_RAYIMPACT).GetComponent<ObjectPoolParticle>();
        mLauncherImpact.transform.position = _launcher.position;
        mLauncherImpact.Play();
        mHitImpact = null;

        mHitPosition = _target.transform.position;
        mBackwardPos = _launcher.position;
        mForwardPos = _launcher.position;
        transform.forward = _target.transform.position - _launcher.position;

        if (_target.kIsPlayer == true)
            mMaskLayer = 1 << StageDef.LAYER_PLAYER;
        else
            mMaskLayer = 1 << StageDef.LAYER_ENEMY;

        mIsFollowRay = false;
        CancelInvoke();
        Invoke("FollowRay", 1.0f);
    }

    void FollowRay()
    {
        mIsFollowRay = true;
    }
}
