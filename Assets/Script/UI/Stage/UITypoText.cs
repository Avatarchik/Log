using UnityEngine;
using System.Collections;
using System.Text;

public class UITypoText : MonoBehaviour {
    public enum Type
    {
        Damage,
        Recovery,
        Debuff,
        Effect
    }

    //TweenPosition mPositionTween;    
    TweenColor mColorTween;
    TweenPosition mPositionTween;

    UILabel mLabel;

    static StringBuilder mStringBuilder = new StringBuilder();

    // Use this for initialization
    void Awake () {
                
        mColorTween = GetComponent<TweenColor>();
        mColorTween.enabled = false;

        mPositionTween = GetComponent<TweenPosition>();
        mPositionTween.enabled = false;

        mLabel = GetComponent<UILabel>();

        //mToPos = mPositionTween.to;
        //mFromPos = mPositionTween.from;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play(string _message, Type _typo = Type.Damage)
    {
        Vector3 pos = transform.localPosition;
        pos.z = 0.0f;
        pos.y += Random.Range(-20.0f, 20.0f);
        pos.x += Random.Range(-40.0f, 40.0f);
        string colorTag = "";
        switch(_typo)
        {            
            case Type.Effect:          
            case Type.Damage:
                {
                    pos.y += 50.0f;
                    colorTag = StrDef.DAMAGE_COLOR;
                    if (_typo == Type.Effect)
                        pos.y += 30;
                                        
                    mColorTween.duration = 0.4f;
                    mColorTween.delay = 0.1f;
                    mPositionTween.duration = 0.5f;                    
                    mPositionTween.from = pos;
                    mPositionTween.to = pos + new Vector3(0.0f, 20.0f, 0.0f);

                    mLabel.fontSize = 40;
                }
                break;
            case Type.Recovery:
                {
                    colorTag = StrDef.SHIELD_RECOVERY_COLOR;
                    pos.y += 110.0f;
                    
                    mColorTween.duration = 0.5f;
                    mLabel.fontSize = 50;
                }
                break;
        }

        transform.localPosition = pos;
        
        float colorTime = mColorTween.delay + mColorTween.duration;        
        float posTime = mPositionTween.delay + mPositionTween.duration;

        float releaseTime = 0.0f;
        releaseTime = posTime;
        if (releaseTime < colorTime)
            releaseTime = colorTime;

        mStringBuilder.Remove(0, mStringBuilder.Length);
        mStringBuilder.Append(colorTag);
        mStringBuilder.Append(_message);
        mStringBuilder.Append("[-]");
        mLabel.text = mStringBuilder.ToString();

        switch (_typo)
        {
            case Type.Effect:
            case Type.Damage:
                {
                    mColorTween.ResetToBeginning();
                    mColorTween.PlayForward();
                    //mScaleTween.ResetToBeginning();
                    //mScaleTween.PlayForward();
                    mPositionTween.ResetToBeginning();
                    mPositionTween.PlayForward();
                }
                break;
            case Type.Recovery:
                {
                    mColorTween.ResetToBeginning();
                    mColorTween.PlayForward();
                }
                break;
            
            case Type.Debuff:
                {
                    mColorTween.ResetToBeginning();
                    mColorTween.PlayForward();
                }
                break;
        }

        Invoke("Release", releaseTime);
    }
    
    void Release()
    {
        ObjectPoolManager.Instance.Release(gameObject);
    }
}
