/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Example script that resizes the widget it's attached to in order to envelop the target content.
/// </summary>

[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Examples/Envelop Content")]
public class EnvelopContent : MonoBehaviour
{
	public Transform targetRoot;
	public int padLeft = 0;
	public int padRight = 0;
	public int padBottom = 0;
	public int padTop = 0;

	bool mStarted = false;

	void Start ()
	{
		mStarted = true;
		Execute();
	}

	void OnEnable () { if (mStarted) Execute(); }

	[ContextMenu("Execute")]
	public void Execute ()
	{
		if (targetRoot == transform)
		{
			Debug.LogError("Target Root object cannot be the same object that has Envelop Content. Make it a sibling instead.", this);
		}
		else if (NGUITools.IsChild(targetRoot, transform))
		{
			Debug.LogError("Target Root object should not be a parent of Envelop Content. Make it a sibling instead.", this);
		}
		else
		{
			Bounds b = NGUIMath.CalculateRelativeWidgetBounds(transform.parent, targetRoot, false);
			float x0 = b.min.x + padLeft;
			float y0 = b.min.y + padBottom;
			float x1 = b.max.x + padRight;
			float y1 = b.max.y + padTop;

			UIWidget w = GetComponent<UIWidget>();
			w.SetRect(x0, y0, x1 - x0, y1 - y0);
			BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
		}
	}
}