using UnityEngine;
using System.Collections;

public class TransformAnimation : MonoBehaviour {

    public bool kIsTranslateX = false;
    public float kXvelocity = 0.1f;
    public bool kIsTranslateY = false;
    public float kYvelocity = 0.1f;    
    public bool kIsTranslateZ = false;
    public float kZvelocity = 0.1f;

    public bool kIsRotateX = false;
    public float kRotateX = 0.1f;
    public bool kIsRotateY = false;
    public float kRotateY = 0.1f;
    public bool kIsRotateZ = false;
    public float kRotateZ = 0.1f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        
        if (kIsTranslateX == true)
            transform.Translate(kXvelocity * Time.deltaTime, 0.0f, 0.0f);
        if (kIsTranslateY == true)
            transform.Translate(0.0f, kYvelocity * Time.deltaTime, 0.0f);
        if (kIsTranslateZ == true)
            transform.Translate(0.0f, 0.0f, kZvelocity * Time.deltaTime);        

        if (kIsRotateX == true)
            transform.Rotate(kRotateX * Time.deltaTime, 0.0f, 0.0f);
        if (kIsRotateY == true)
            transform.Rotate(0.0f, kRotateY * Time.deltaTime, 0.0f);
        if (kIsRotateZ == true)
            transform.Rotate(0.0f, 0.0f, kRotateZ * Time.deltaTime);
    }
}
