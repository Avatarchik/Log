using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonManager : SingletonG<CommonManager> {    
    List<SequenceController> mSequenceControllerList = new List<SequenceController>();

    public static bool Title = true;
    public static bool Loaded = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);        
        CDataManagerNavigator.Instance.Load();
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start () {
        
        SequenceController controller = null;
        controller = CommonUIRoot.Instance.GetComponent<SequenceController>();
        mSequenceControllerList.Add(controller);
        controller = SoundManager.Instance.GetComponent<SequenceController>();
        mSequenceControllerList.Add(controller);
        
        StartCoroutine(SequenceControl());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator SequenceControl()
    {        
        for ( int i = 0; i < mSequenceControllerList.Count; i++ )
        {
            SequenceController controller = mSequenceControllerList[i];

            while ( controller.IsStartDone() == false )
                yield return null;
        }

        if ( Title == true )
            CommonUIRoot.Instance.kTitle.SetLoadingPercent(0);
        yield return null;

        for (int i = 0; i < mSequenceControllerList.Count; i++)
        {
            SequenceController controller = mSequenceControllerList[i];

            controller.Prepare();

            if (Title == true)
                CommonUIRoot.Instance.kTitle.SetLoadingPercent(50);
            yield return null;
        }
                        
        if (Title == true)
            CommonUIRoot.Instance.kTitle.SetLoadingPercent(100);
        yield return null;

        Loaded = true;
    }
}
