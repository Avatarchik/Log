using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Ship), true)]
public class ShipEditor : Editor
{
    Ship _this;

    void OnEnable()
    {
        // target은 Editor의 멤버 변수으로 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객처에 대한 
        // 레퍼런스 object형이므로 실제 클라스(타입)으로 캐스팅해서 명확하게 해서 사용하기 용이하게한다.

        _this = target as Ship;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        
        if(GUILayout.Button("Base Struct") == true )
        {
            if( _this.gameObject.GetComponent<SphereCollider>() == null )
            {
                SphereCollider sphere = _this.gameObject.AddComponent<SphereCollider>();
                sphere.radius = 50.0f;
            }
            if( _this.gameObject.GetComponent<CapsuleCollider>() == null )
            {
                CapsuleCollider capsule = _this.gameObject.AddComponent<CapsuleCollider>();
                capsule.direction = 2;
            }
            if( _this.gameObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rigid = _this.gameObject.AddComponent<Rigidbody>();
                rigid.useGravity = false;
                rigid.isKinematic = true;
            }

            GameObject weapon = null;
            if ( _this.transform.Find("Weapon") == null )
            {
                weapon = new GameObject();
                weapon.name = "Weapon";
                weapon.transform.parent = _this.transform;
            }
            else
            {
                weapon = _this.transform.Find("Weapon").gameObject;
            }
            if (weapon.GetComponent<Weapon>() == null)
            {
                weapon.AddComponent<Weapon>();
                weapon.AddComponent<SphereCollider>().radius = 100.0f;
            }
            weapon.transform.localPosition = Vector3.zero;

            GameObject model = null;
            if (_this.transform.Find("Model") == null)
            {
                model = new GameObject();
                model.name = "Model";
                model.transform.parent = _this.transform;
            }
            else
            {
                model = _this.transform.Find("Model").gameObject;
            }
            model.transform.localPosition = Vector3.zero;

            GameObject engine = null;
            if (_this.transform.Find("Engine") == null)
            {
                engine = new GameObject();
                engine.name = "Engine";
                engine.transform.parent = _this.transform;
            }
            else
            {
                engine = _this.transform.Find("Engine").gameObject;
            }
            if (engine.transform.Find("Thruster") == null)
            {
                Object thrusterPrefab = AssetDatabase.LoadAssetAtPath<Object>("Assets/Effect/Thruster.prefab");
                GameObject thrusterGameObject = Instantiate(thrusterPrefab, engine.transform) as GameObject;
                thrusterGameObject.name = "Thruster";
            }            
            engine.transform.localPosition = Vector3.zero;

            Debug.Log("Ship Base Struct Complete!");
        }
    }
}
