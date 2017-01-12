using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        Invoke("Release", 1.0f);
    }

    void Release()
    {
        ObjectPoolManager.Instance.Release(gameObject);
    }
}
