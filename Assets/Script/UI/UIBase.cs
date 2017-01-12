using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour {

    public virtual void OnEnableAnimation()   {}

    void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        OnEnableAnimation();
    }
}
