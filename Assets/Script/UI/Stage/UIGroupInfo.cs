using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class UIGroupInfo : MonoBehaviour {
    class ShipInfo
    {
        public Ship ship = null;
        public Transform AbilityTrans;
        public UISprite shieldAbilitySpr;
        public UISprite healthAbilitySpr;
        //UISprite[] mWeaponCoolTimeFill;
        //public UILabel energyLabel;
    }

    List<ShipInfo> mInfoList = new List<ShipInfo>();
    List<GameObject> mShipInfoSprPoolList = new List<GameObject>();

    GameObject mInfoGameObj;

    StringBuilder mEnergyBuilder = new StringBuilder();

    Camera mStageCamera;
    Camera mUICamera;

    void Awake()
    {
        mInfoGameObj = transform.Find("Info").gameObject;
        mInfoGameObj.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(DrawInfo());
    }
	
	// Update is called once per frame
	IEnumerator DrawInfo () {
        while(mStageCamera == null)
        {
            if(StageManager.Instance.kStageCamera != null)
                mStageCamera = StageManager.Instance.kStageCamera.GetComponent<Camera>();

            yield return null;
        }

        while(mUICamera == null)
        {
            if(StageUIRoot.Instance.kCamera != null)
                mUICamera = StageUIRoot.Instance.kCamera;

            yield return null;
        }
            
        while (true)
        {
            for (int i = 0; i < mInfoList.Count; i++)
            {
                ShipInfo info = mInfoList[i];
                if (info.ship == null)
                    continue;

                if (info.ship.kIsDie == true)
                {
                    info.ship = null;
                    info.AbilityTrans.gameObject.SetActive(false);
                    i--;
                    continue;
                }
                
                if (info.ship.kCurShieldPoint != info.ship.kTotalShieldPoint ||
                    info.ship.kCurHealthPoint != info.ship.kTotalHealthPoint)
                    info.AbilityTrans.Find("Energy").gameObject.SetActive(true);
                else
                    info.AbilityTrans.Find("Energy").gameObject.SetActive(false);

                Ship ship = info.ship;
                
                info.shieldAbilitySpr.fillAmount = (float)ship.kCurShieldPoint / (float)ship.kTotalShieldPoint;
                info.healthAbilitySpr.fillAmount = (float)ship.kCurHealthPoint / (float)ship.kTotalHealthPoint;

                mEnergyBuilder.Remove(0, mEnergyBuilder.Length);
                mEnergyBuilder.Append(ship.kCurShieldPoint.ToString());
                mEnergyBuilder.Append("/");
                mEnergyBuilder.Append(ship.kCurHealthPoint.ToString());
                
                //info.energyLabel.text = mEnergyBuilder.ToString();
                

                SetReposition(i);
            }

            yield return null;
        }
	}

    public void SetShip(Ship _ship)
    {
        for (int i = 0; i < mInfoList.Count; i++)
            if (mInfoList[i].ship == _ship)
                return;
        
        ShipInfo info = new ShipInfo();
        mInfoList.Add(info);
        GameObject obj = null;
        if (mShipInfoSprPoolList.Count == 0)
        {
            obj = Instantiate(mInfoGameObj);
        }
        else
        {
            obj = mShipInfoSprPoolList[0];
            mShipInfoSprPoolList.RemoveAt(0);
        }

        obj.gameObject.SetActive(true);
        obj.transform.Find("Energy").gameObject.SetActive(false);
        info.AbilityTrans = obj.transform;
        info.AbilityTrans.parent = mInfoGameObj.transform.parent;
        info.AbilityTrans.rotation = mInfoGameObj.transform.rotation;
        info.AbilityTrans.localScale = Vector3.one;
        info.AbilityTrans.Find("Energy").gameObject.SetActive(false);
        info.shieldAbilitySpr = info.AbilityTrans.Find("Energy/Shield/Foreground").GetComponent<UISprite>();
        info.healthAbilitySpr = info.AbilityTrans.Find("Energy/Health/Foreground").GetComponent<UISprite>();
        
        //info.energyLabel = info.AbilityTrans.Find("Energy/Label").GetComponent<UILabel>();
        info.ship = _ship;

        mInfoList.Add(info);
    }
        
    void SetReposition(int _index)
    {
        Vector3 shipPos = mInfoList[_index].ship.transform.position;

        float dist = Vector3.Distance(shipPos, mStageCamera.transform.position);
        float scale = (150 - Mathf.Clamp(dist, 30.0f, 100.0f)) / 75.0f;
        
        Vector3 screenPos = mStageCamera.WorldToScreenPoint(shipPos);
        Vector3 pos = mUICamera.ScreenToWorldPoint(screenPos);
        
        Transform abilityTrans = mInfoList[_index].AbilityTrans;
        abilityTrans.position = pos;
        Vector3 localPos = abilityTrans.localPosition;
        localPos.z = 0.0f;        
        localPos.y += 30.0f;

        abilityTrans.localScale     = Vector3.one * scale;
        abilityTrans.localPosition  = localPos;
    }

    public void Clear()
    {
        mInfoList.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            if (obj == mInfoGameObj)
                continue;

            obj.gameObject.SetActive(false);
            mShipInfoSprPoolList.Add(obj);
        }
    }
}
