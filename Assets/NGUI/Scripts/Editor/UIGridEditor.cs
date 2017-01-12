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
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIGrid), true)]
public class UIGridEditor : UIWidgetContainerEditor
{
    UIGrid _this;

    void OnEnable()
    {
        // target은 Editor의 멤버 변수으로 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객처에 대한 
        // 레퍼런스 object형이므로 실제 클라스(타입)으로 캐스팅해서 명확하게 해서 사용하기 용이하게한다.

        _this = target as UIGrid;
    }

    public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		SerializedProperty sp = NGUIEditorTools.DrawProperty("Arrangement", serializedObject, "arrangement");

		NGUIEditorTools.DrawProperty("  Cell Width", serializedObject, "cellWidth");
		NGUIEditorTools.DrawProperty("  Cell Height", serializedObject, "cellHeight");

		if (sp.intValue < 2)
		{
			bool columns = (sp.hasMultipleDifferentValues || (UIGrid.Arrangement)sp.intValue == UIGrid.Arrangement.Horizontal);

			GUILayout.BeginHorizontal();
			{
				sp = NGUIEditorTools.DrawProperty(columns ? "  Column Limit" : "  Row Limit", serializedObject, "maxPerLine");
				if (sp.intValue < 0) sp.intValue = 0;
				if (sp.intValue == 0) GUILayout.Label("Unlimited");
			}
			GUILayout.EndHorizontal();

			UIGrid.Sorting sort = (UIGrid.Sorting)NGUIEditorTools.DrawProperty("Sorting", serializedObject, "sorting").intValue;

			if (sp.intValue != 0 && (sort == UIGrid.Sorting.Horizontal || sort == UIGrid.Sorting.Vertical))
			{
				EditorGUILayout.HelpBox("Horizontal and Vertical sortinig only works if the number of rows/columns remains at 0.", MessageType.Warning);
			}
		}

		NGUIEditorTools.DrawProperty("Pivot", serializedObject, "pivot");
		NGUIEditorTools.DrawProperty("Smooth Tween", serializedObject, "animateSmoothly");
		NGUIEditorTools.DrawProperty("Hide Inactive", serializedObject, "hideInactive");
		NGUIEditorTools.DrawProperty("Constrain to Panel", serializedObject, "keepWithinPanel");
        NGUIEditorTools.DrawProperty("Sort Name", serializedObject, "sortName");
        serializedObject.ApplyModifiedProperties();
        
        if( GUILayout.Button("NameSort") == true )
        {
            for(int i = 0; i < _this.transform.childCount; i++)
            {
                Transform child = _this.GetChild(i);
                child.name = _this.sortName + (i + 1).ToString();
            }
        }
	}
}
