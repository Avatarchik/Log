using UnityEngine;
using System.Collections;

public class TransformUtil {    
    public static Transform TopParent(Transform _tf)
    {   
        while(_tf.parent != null)
        {
            _tf = _tf.parent;
        }

        return _tf;
    }

	public static Transform TopParentComponent(Transform _tf, string _component)
	{
		while (_tf.parent != null)
		{
			_tf = _tf.parent;
			
			if (_tf.GetComponent(_component) != null)
			{
				return _tf;
			}
		}
		
		return null;
	}

    public static Transform TopParentName(Transform _tf, string _name)
    {
        while (_tf.parent != null)
        {
            _tf = _tf.parent;

            if (_tf.name == _name)
            {
                return _tf;
            }
        }

        return null;
    }

    public static Transform TopParentTag(Transform _tf, string _tag)
    {
        while (_tf.parent != null)
        {
            _tf = _tf.parent;

            if (_tf.tag == _tag) {
                return _tf;
            }
        }

        return null;
    }

    public static string PathToRoot(Transform _tf)
    {
        string path = "";
        path = _tf.name;

        while (_tf.parent != null)
        {
            path = _tf.parent.name + "/" + path;
            _tf = _tf.parent;            
        }

        return path;
    }
}
