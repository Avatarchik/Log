using UnityEngine;
using System.Collections;

public class ObjectPoolParticle : MonoBehaviour {
    protected ParticleSystem mParentParticleSystem;

    public bool kAutoRelease = true;

    // Use this for initialization
    void Awake () {
        mParentParticleSystem = GetComponent<ParticleSystem>();
        if (mParentParticleSystem != null)
        {
            mParentParticleSystem.playOnAwake = false;
            mParentParticleSystem.Stop(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {       
        if (mParentParticleSystem != null)
        {
            mParentParticleSystem.gameObject.SetActive(true);
            mParentParticleSystem.Stop(true);
            mParentParticleSystem.Play(true);

            if (mParentParticleSystem.loop == true)
                return;

            if (kAutoRelease == true)
                Invoke("Release", mParentParticleSystem.duration);
        }
    }

    public void SimulateClear()
    {
        if (mParentParticleSystem != null)
        {
            // 원점에 이펙트가 생성되는 버그 방지용...
            mParentParticleSystem.Clear();
            mParentParticleSystem.Simulate(0.0001f, true, true);
            mParentParticleSystem.Play(true);
        }
    }

    public void Release()
    {
        SimulateClear();
        ObjectPoolManager.Instance.Release(gameObject);
    }
}
