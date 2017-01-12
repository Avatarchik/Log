using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour {
    public Transform kTarget;

    public enum RotateAxis
    {
        X,
        Y,
        Z
    }

    public RotateAxis kRotateAxis = RotateAxis.Y;
    public float kRotateSpeed = 10.0f;

    Vector3 mAxisVector;

    // Use this for initialization
    void Start () {
        switch (kRotateAxis)
        {
            case RotateAxis.X:
                mAxisVector = Vector3.right;
                break;
            case RotateAxis.Y:
                mAxisVector = Vector3.up;
                break;
            case RotateAxis.Z:
                mAxisVector = Vector3.forward;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(kTarget == null)
        {
            transform.Rotate(mAxisVector, Time.deltaTime * kRotateSpeed);
        }
        else
        {
            kTarget.Rotate(mAxisVector, Time.deltaTime * kRotateSpeed);
        }
	}
}
