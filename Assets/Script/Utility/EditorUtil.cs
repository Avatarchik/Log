#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;

public class EditorUtil {
    static int oldVersionNum = 0;

    public static Color SelectColor(Color _color, bool _bSelect, float _a = 0.0f)
    {
        if (_bSelect == true)
            _color.a = 1.0f;
        else
            _color.a = 0.2f + _a;
        return _color;
    }
    
    public static void HandleLabelEx(Vector3 _pos, string _text, int _fontSize, Color _fontColor)
    {
        GUIStyle style          = new GUIStyle();
        
        style.fontSize          = _fontSize;
        style.normal.textColor  = _fontColor;

        Handles.BeginGUI();
        Handles.Label(_pos, _text, style);
        Handles.EndGUI();
    }

    public static Camera GetZeroSceneViewCam()
    {
        Camera[] cams = SceneView.GetAllSceneCameras();
        bool sceneHasCamera = cams.Length > 0;
        Camera sceneCamera = null;        
        if (sceneHasCamera){
            sceneCamera = cams[0];
        }

        return sceneCamera;
    }
}
#endif