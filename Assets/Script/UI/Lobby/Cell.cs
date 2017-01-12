using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

    public enum LandMark
    {
        None,
        BluePlanet,
        BrownPlanet,
        GrayPlanet,        
        MixBrownPlanet,
        PurplePlanet,
        RedPlanet,
        WhitePlanet,
        GreenSatellite,
        WhiteSatellite
    };

    public Nation kNation = null;
    public LandMark kLandMark = LandMark.None;

    public int kRowIndex = 0;
    public int kColumnIndex = 0;

    TweenAlpha mTweenAlpha = null;

    [HideInInspector]
    public UITexture mUITexture;

    void Awake()
    {
        /*
        mUITexture = GetComponent<UITexture>();
        mTweenAlpha = GetComponent<TweenAlpha>();
        mTweenAlpha.duration = 2.0f;
        mTweenAlpha.enabled = false;*/
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EventApprear()
    {
        mTweenAlpha.ResetToBeginning();
        mTweenAlpha.PlayForward();
        Invoke("ColorUpdate", mTweenAlpha.duration);
    }

    public void Conquest(Nation _nation)
    {
        if (kNation != null)
        {
            kNation.kConquestCellList.Remove(this);
            kNation.Lose();
        }

        if (_nation != null)
            _nation.kConquestCellList.Add(this);

        kNation = _nation;
    }

    public void ColorUpdate()
    {
        Color color = mUITexture.color;
        float alpha = color.a;

        if (kNation == null)
        {            
            color = Color.white;
            color.a = 25.0f / 255.0f;
            mUITexture.color = color;
            return;
        }

        switch(kNation.kFeature)
        {
            case Nation.Feature.Aggressive:
                {
                    color = Color.red;
                    //color.a = 1.0f;// 150.0f / 255.0f;
                }
                break;
            case Nation.Feature.Careful:
                {
                    color = Color.blue;
                    //color.a = 1.0f; //150.0f / 255.0f;
                }
                break;
            case Nation.Feature.Defensive:
                {
                    color = Color.black;
                    //color.a = 1.0f; //150.0f / 255.0f;
                }
                break;
            case Nation.Feature.Usually:
                {
                    color = Color.yellow;
                    //color.a = 1.0f; //150.0f / 255.0f;
                }
                break;
        }

        color.a = alpha;
        mUITexture.color = color;
    }

    public float CellUnitArmyPower()
    {
        if (kNation == null)
            return 0.0f;

        return kNation.CellUnitArmyPower();
    }
}
