using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using CommonEnum;

public class SceneLoadManager : SingletonG<SceneLoadManager>
{    
    public static float kStageSceneLoadingTime = 0.0f;
    public static bool kIsLoading = false;

    SceneState mNextSceneType = SceneState.None;

    [HideInInspector]
    public SceneState kCurrentSceneType = SceneState.None;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Update()
    {
    }
    
    public void SetLoadScene(SceneState _state)
    {
        Time.timeScale = 1.0f;

        CommonUIRoot.Instance.SetLoading();

        StartCoroutine(AsyncLoading(_state));
    }

    void CleanUp(SceneState _state)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
    
    //현재는 추가 후에 삭제를 하기 때문에 순간 (500M에 넘지 않는) 메모리 상승을 막을 수는 없다.
    //추후에 삭제 후 추가하는 방법으로 이 구간을 수정해야 한다.
    public IEnumerator AsyncLoading(SceneState _type)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_type.ToString());
        asyncOperation.allowSceneActivation = false;
        
        while (true)
        {
            CommonUIRoot.Instance.kLoading.SetPercent((int)(asyncOperation.progress * 50.0f));

            if (asyncOperation.progress < 0.9f)
            {
                yield return null;
                continue;
            }

            CommonUIRoot.Instance.kLoading.SetPercent(50);
            yield return null;
            
            CleanUp(kCurrentSceneType);
            kCurrentSceneType = _type;
            asyncOperation.allowSceneActivation = true;
            asyncOperation = null;

            yield break;
        }    
    }
}