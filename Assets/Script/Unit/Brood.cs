using UnityEngine;
using System.Collections;

public class Brood : MonoBehaviour {
    public enum State
    {
        Storage,    //부모 기체에 대기 중
        Move,       //타겟을 향해 이동 중
        Repair,     //타겟을 수리 중
        Return      //부모 기체에 복귀 중
    }

    public State kState = State.Storage;

    ParticleSystem mEngineParticle;

    BroodLord mParentShip;
    Ship mTargetShip;

    ////////////////////////////////////////////////////////
    //함선 움직임 관련    
    //선회력
    public float kTurningForce = 0.3f;    
    //현재 속력
    protected float mCurVelocity = 0.0f;
    //최대 속력
    public float kMaxVelocity = 0.05f;

    bool mIsRepair = false;
    bool mIsSpeedUp = false;
    ////////////////////////////////////////////////////////

    Vector3 mDockPosition;

    Vector3 mOldTargetPos;
    Quaternion mOldTargetRot;

    void Awake()
    {
        mEngineParticle = transform.Find("Thruster").GetComponent<ParticleSystem>();
    }
    
    public void OnPrepare()
    {
        mIsRepair = false;
        mIsSpeedUp = false;
        mOldSpeedState = false;
        kState = State.Storage;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetParent(BroodLord _lord)
    {
        mParentShip = _lord;
        mDockPosition = transform.localPosition;
    }
    
    public void SetTarget(Ship _ship)
    {
        mTargetShip = _ship;
        kState = State.Move;
        mParentShip.kAvailableBroodList.Remove(this);
        transform.parent = null;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (mParentShip == null)
            return;

        VelocityUpdate();        

        switch (kState)
        {
            case State.Storage:
                {
                    if (mParentShip.kState == Ship.State.Move)
                    {
                        if (mEngineParticle.isPlaying == false)
                            mEngineParticle.Play();
                    }
                    else
                    {
                        mEngineParticle.Stop();
                    }

                    transform.forward = Vector3.MoveTowards(transform.forward, mParentShip.transform.forward, Time.deltaTime * kTurningForce);
                }
                break;
            case State.Move:
                {
                    mIsSpeedUp = true;
                    Vector3 movePos = mTargetShip.transform.position + Vector3.up * 10.0f;
                    Move(movePos - transform.position);

                    if (Vector3.Distance(movePos, transform.position) < 10.0f)
                    {
                        kState = State.Repair;
                        transform.parent = mTargetShip.transform;
                    }

                    if (mEngineParticle.isPlaying == false)
                        mEngineParticle.Play();
                }
                break;
            case State.Repair:
                {
                    if (mTargetShip.kState == Ship.State.Move)
                    {
                        if (mEngineParticle.isPlaying == false)
                            mEngineParticle.Play();
                    }
                    else
                    {
                        mEngineParticle.Stop();
                    }

                    transform.forward = Vector3.MoveTowards(transform.forward, mTargetShip.transform.forward, Time.deltaTime * kTurningForce);

                    if (mIsRepair == false)
                    {
                        mIsRepair = true;
                        StartCoroutine(TargetRepair());
                    }

                    if (mEngineParticle.isPlaying == false)
                        mEngineParticle.Play();
                }
                break;
            case State.Return:
                {
                    mIsSpeedUp = true;                    
                    Vector3 returnPos = mParentShip.transform.position + mDockPosition;
                    Move(returnPos - transform.position);
                    
                    if (mEngineParticle.isPlaying == false)
                        mEngineParticle.Play();

                    if (Vector3.Distance(returnPos, transform.position) < 2.0f)
                    {
                        kState = State.Storage;
                        transform.parent = mParentShip.transform;
                        mIsSpeedUp = false;
                        mOldSpeedState = false;
                        StartCoroutine(Break());
                    }
                }
                break;
        }
	}

    public void TargetLostUpdate()
    {
        if (mTargetShip == null)
            return;

        if (kState == State.Return)
            return;

        if (mTargetShip.kIsDie == true)
            ReturnToBase();
    }

    void ReturnToBase()
    {
        mParentShip.WorkComplete(this, mTargetShip);
        kState = State.Return;        
        transform.parent = null;
        mTargetShip = null;
        mIsRepair = false;
    }

    protected void Move(Vector3 _dir)
    {
        transform.forward = Vector3.MoveTowards(transform.forward, _dir, Time.deltaTime * kTurningForce);
        transform.position += transform.forward * mCurVelocity * Time.deltaTime;
    }

    IEnumerator TargetRepair()
    {        
        float curTime = 0.0f;
        
        while (true)
        {
            if (mTargetShip.kIsDie == true)
            {
                ReturnToBase();
                yield break;
            }

            if (mTargetShip.IsEnergyFull() == true)
            {
                kState = State.Return;
                ReturnToBase();
                yield break;
            }

            curTime += Time.deltaTime;
            if (curTime >= 1.0f)
            {
                curTime -= 1.0f;
                mTargetShip.SumHealthPoint(50);
                mTargetShip.SumShieldPoint(50);
            }

            yield return null;
        }
    }

    IEnumerator Accelation()
    {
        float maxVelocity = kMaxVelocity;
        float toMaxVelocityTime = (maxVelocity - mCurVelocity) / maxVelocity;

        while (toMaxVelocityTime <= 1.0f)
        {
            if (mIsSpeedUp == false)
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
        float maxVelocity = kMaxVelocity;
        float toZeroVelocityTime = mCurVelocity / maxVelocity;

        while (toZeroVelocityTime >= 0.0f)
        {
            if (mIsSpeedUp == true)
                yield break;

            toZeroVelocityTime -= Time.deltaTime;
            mCurVelocity = maxVelocity * toZeroVelocityTime;
            if (mCurVelocity < 0.0f)
                mCurVelocity = 0.0f;

            yield return null;
        }

        yield break;
    }

    bool mOldSpeedState = false;
    public virtual void VelocityUpdate()
    {
        if (kState == State.Move || kState == State.Return)
            mIsSpeedUp = true;
        else
            mIsSpeedUp = false;

        if (mIsSpeedUp == mOldSpeedState)
            return;

        mOldSpeedState = mIsSpeedUp;
        if (mIsSpeedUp == true)
            StartCoroutine(Accelation());
        else
            StartCoroutine(Break());
    }
}
