using UnityEngine;
using System.Collections;

public class SequenceController : MonoBehaviour {
    protected bool mStartDone = false;
    protected bool mPrepareDone = false;

    // Use this for initialization
    public void Start () {
        OnStart();

        mStartDone = true;
    }

    public void Update(){
        if (IsStandBy() == false)
            return;

        OnUpdate();
    }

    public void LateUpdate(){
        if (IsStandBy() == false)
            return;

        OnLateUpdate();
    }

    public void FixedUpdate() {
        if (IsStandBy() == false)
            return;

        OnFixedUpdate();
    }

    public virtual void Prepare() {
        OnPrepare();

        mPrepareDone = true;
    }

    public bool IsPrepareDone()
    {
        return mPrepareDone;
    }

    public bool IsStartDone()
    {
        return mStartDone;
    }

    bool IsStandBy()
    {
        if (mStartDone == true && mPrepareDone == true)
            return true;

        return false;
    }

    public virtual void OnPrepare()     { }
    public virtual void OnStart()       { }    
    public virtual void OnUpdate()      { }
    public virtual void OnLateUpdate()  { }
    public virtual void OnFixedUpdate() { }
}
