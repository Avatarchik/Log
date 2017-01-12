//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        Custom,
        OnEnable,
        OnDisable,
    }

    public AudioClip audioClip;
    public Trigger trigger = Trigger.OnClick;
    public int kID  = 0;

    bool mIsOver = false;

    bool canPlay
	{
		get
		{
			if (!enabled) return false;
			UIButton btn = GetComponent<UIButton>();
			return (btn == null || btn.isEnabled);
		}
	}

    void Awake()
    { 
    }

	void OnEnable ()
	{
        if (trigger == Trigger.OnEnable)
        {
            SoundManager.Instance.PlayEffect(kID);//NGUITools.PlaySound(audioClip, volume, pitch);
        }
    }

	void OnDisable ()
	{
		if (trigger == Trigger.OnDisable)
        {
            SoundManager.Instance.PlayEffect(kID);
        }
    }

    void OnHover (bool isOver)
	{
		if (trigger == Trigger.OnMouseOver)
		{
			if (mIsOver == isOver) return;
			mIsOver = isOver;
		}

		if (canPlay && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
        {
            SoundManager.Instance.PlayEffect(kID);
        }
    }

    void OnPress (bool isPressed)
	{
		if (trigger == Trigger.OnPress)
		{
			if (mIsOver == isPressed) return;
			mIsOver = isPressed;
		}

		if (canPlay && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
        {
            SoundManager.Instance.PlayEffect(kID);
        }

    }

    void OnClick ()
	{
		if (canPlay && trigger == Trigger.OnClick)
        {
            SoundManager.Instance.PlayEffect(kID);
        }
    }

    void OnSelect (bool isSelected)
	{
		if (canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
			OnHover(isSelected);
	}

	public void Play ()
    {
        SoundManager.Instance.PlayEffect(kID);
    }
}
