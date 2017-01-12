using UnityEngine;
using System.Collections;

public class Missile : Projectile
{
    CapsuleCollider mCollider;
    TrailRenderer mTrail;
    GameObject mModel;

    bool mIsFollowTarget = false;

    void Awake()
    {
        mModel = transform.Find("Model").gameObject;
        mTrail = transform.Find("Trail").GetComponent<TrailRenderer>();
        mCollider = GetComponent<CapsuleCollider>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected override void MoveUpdate()
    {
        if (mIsFollowTarget == true)
            base.MoveUpdate();
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

    public override void SetLaunch(Ship _target, Ship _attacker, Transform _launcher)
    {
        base.SetLaunch(_target, _attacker, _launcher);

        mIsFollowTarget = false;
        mCollider.enabled = true;
        mModel.gameObject.SetActive(true);

        mTrail.Clear();
        StartCoroutine(LaunchMove(_launcher.forward));
    }

    IEnumerator LaunchMove(Vector3 _forward)
    {
        float curTime = 0.0f;
        float totalTime = 1.0f;
        while (curTime < totalTime)
        {
            curTime += Time.deltaTime;
            if (Time.timeScale == 0.0f)
                continue;

            Vector3 oldPos = transform.position;
            Vector3 targetFollowPos = Vector3.MoveTowards(transform.position, mTargetShip.transform.position, Time.deltaTime * mVelocity);
            Vector3 forwardMovePos = transform.position + (_forward * Time.deltaTime * mVelocity);
            transform.position = Vector3.Lerp(forwardMovePos, targetFollowPos, curTime / totalTime);
            transform.forward = transform.position - oldPos;

            yield return null;
        }

        mIsFollowTarget = true;
        yield break;
    }

    protected override void OnTargetHit()
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

        mCollider.enabled = false;
        mIsFollowTarget = false;
        mModel.gameObject.SetActive(false);

        Invoke("ReleaseReserve", mTrail.time);
    }

    void ReleaseReserve()
    {
        Release();
    }
}
