using UnityEngine;
using System.Collections;

public class InfernoRay : Projectile
{
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
    void Awake()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        ren = mLineRenderer.material;
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
            mTargetShip.kWeapon.UnlockLauncher(mLockIndex);
            return;
        }
    }
    
    IEnumerator HitUpdate()
    {
        float curDurationTime = 0.0f;
        float damageCurCycleTime = 0.0f;
        while(mState == State.Hit)
        {
            curDurationTime += Time.deltaTime;
            damageCurCycleTime += Time.deltaTime;

            float ratio = curDurationTime / 7.0f;
            ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);

            float width = Mathf.Clamp(ratio * 10.0f, 0.2f, 10.0f);

            if (mLauncherImpact != null)
                mLauncherImpact.transform.localScale = Vector3.one * width;
            if (mHitImpact != null)
                mHitImpact.transform.localScale = Vector3.one * width;

            mLineRenderer.SetWidth(width, width);
            if (damageCurCycleTime >= 1.0f)
            {
                damageCurCycleTime = 0.0f;
                if (mTargetShip.kShieldCollider.enabled == true)
                {
                    Shield shield = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_SHIELD).GetComponent<Shield>();
                    shield.Play();
                    shield.transform.forward = -transform.forward;
                    shield.transform.position = mHitPosition - transform.forward * mTargetShip.kShieldCollider.radius;
                }

                int damage = Mathf.Clamp((int)(ratio * mMaxDamage), mMinDamage, mMaxDamage);
                mTargetShip.Damaged(damage);
            }
            
            yield return null;
        }        

        yield break;
    }

    float mCurHitTime = 0.0f;
    Material ren;
    // Update is called once per frame
    void Update()
    {
        MissingUpdate();

        Vector2 texOffset = ren.mainTextureOffset;
        texOffset.x -= Time.deltaTime * 2.0f;
        ren.mainTextureOffset = texOffset;

        switch (mState)
        {
            case State.Move:
                {
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
                        StartCoroutine(HitUpdate());
                    }
                }
                break;
            case State.Hit:
                {
                    mCurHitTime += Time.deltaTime;

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
                mLauncherImpact.transform.localScale = Vector3.one;
                mLauncherImpact.Release();
                mLauncherImpact = null;
            }

            mBackwardPos = Vector3.MoveTowards(mBackwardPos, mForwardPos, Time.deltaTime * mVelocity);
            if (mBackwardPos == mForwardPos)
            {
                if (mHitImpact != null)
                {
                    mHitImpact.transform.localScale = Vector3.one;
                    mHitImpact.Release();
                    mHitImpact = null;
                }
                Release();
            }
        }
    }

    int mLockIndex = 0;
    public void SetLauncherLockIndex(int _index)
    {
        mLockIndex = _index;
    }

    public override void SetLaunch(Ship _target, Ship _attacker, Transform _launcher)
    {
        base.SetLaunch(_target, _attacker, _launcher);

        if (mState == State.Hit)
            return;

        mState = State.Move;
        mLineRenderer.SetWidth(0.5f, 0.5f);
        mLauncherImpact = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_RAYIMPACT).GetComponent<ObjectPoolParticle>();
        mLauncherImpact.transform.position = _launcher.position;
        mLauncherImpact.Play();
        mHitImpact = null;

        mHitPosition = _target.transform.position;
        mBackwardPos = _launcher.position;
        mForwardPos = _launcher.position;
        transform.forward = _target.transform.position - _launcher.position;

        if (_target.kIsPlayerGroup == true)
            mMaskLayer = 1 << StageDef.LAYER_PLAYER;
        else
            mMaskLayer = 1 << StageDef.LAYER_ENEMY;

        mIsFollowRay = false;
    }    
}
