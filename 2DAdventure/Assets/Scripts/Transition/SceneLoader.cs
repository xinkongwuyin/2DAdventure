using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable
{
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 MenuPosition;

    [Header("事件监听")]
    public SceneLoadEventSQ loadEventSQ;
    public VoidEventsSQ newGameEvent;
    public VoidEventsSQ backToMenuEvent;

    [Header("广播")]
    public VoidEventsSQ afterSceneLoadedEvent;
    public FadeEventSQ fadeEventSQ;
    public SceneLoadEventSQ sceneUnloadedEvent;
    public VoidEventsSQ SaveGameEvent;

    [Header("场景")]
    public GameSceneSQ MenuScene;
    public GameSceneSQ firstLoadScene;
    [SerializeField ]private GameSceneSQ currentLoadScene;
    private GameSceneSQ sceneToLoad;
    private Vector3 positionToGo;
    private bool isLoading;
    private bool fadeScreen;
   

    public float fadeDuration; 
    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference,UnityEngine.SceneManagement.LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    


    }

    private void Start()
    {
        //newGame();
        loadEventSQ.RaisesLoadRequestEvent(MenuScene, MenuPosition, true);


    }

    private void OnEnable()
    {
        loadEventSQ.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += newGame;
        ISaveable saveable= this;
        saveable.RegisterSaveData();

        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;

    }

    private void OnDisable()
    {
        loadEventSQ.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= newGame;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();

        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;


    }

    private void OnBackToMenuEvent()
    {
        sceneToLoad = MenuScene;
        loadEventSQ.RaisesLoadRequestEvent(sceneToLoad, MenuPosition, true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="locationToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScene"></param>
    private void newGame()
    {
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad, firstPosition,true);
        loadEventSQ.RaisesLoadRequestEvent(sceneToLoad, firstPosition,true);

        StartCoroutine(SaveAfterLoadRoutine());
    }
    private IEnumerator SaveAfterLoadRoutine()
    {
        // 等待异步加载彻底完成（isLoading 是你在 OnLoadCompleted 里设为 false 的）
        while (isLoading)
        {
            yield return null;
        }

        // 【关键】再额外等一帧！
        // 这一帧是为了确保新场景里所有物体的 OnEnable/Start 都执行完毕
        // 并且都已经在 DataManager 里 RegisterSaveData 了
        yield return new WaitForEndOfFrame();

        // 此时 saveables 列表已经填满了当前场景的所有物体
        // 执行保存，这就会成为你“死亡后回到的那个点”
        SaveGameEvent.RaiseEvent();
        Debug.Log("初始存档点已记录：第一关起点");
    }



    private void OnLoadRequestEvent(GameSceneSQ locationToLoad, Vector3 posToGo, bool fadeScene)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad= locationToLoad;
        positionToGo= posToGo;
        this.fadeScreen = fadeScene;

        Debug.Log(sceneToLoad.sceneReference.SubObjectName);

        if (currentLoadScene != null)
            StartCoroutine(UnLoadPreviousScene());
        else
            LoadNewScene();
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            fadeEventSQ.FadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);

        sceneUnloadedEvent.RaisesLoadRequestEvent(sceneToLoad, positionToGo, true);


        yield return currentLoadScene.sceneReference.UnLoadScene();

        playerTrans.gameObject.SetActive(false);

        LoadNewScene();
    }


    private void LoadNewScene()
    {
        var loadingOption= sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true  );

        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadScene= sceneToLoad;

        playerTrans.position = positionToGo; 

        playerTrans.gameObject.SetActive(true);

        if (fadeScreen)
        {
             fadeEventSQ.FadeOut(fadeDuration);
        }

        isLoading = false;

        if (currentLoadScene.SceneType != SceneType.Menu)
        {
            afterSceneLoadedEvent?.RaiseEvent();
        }

    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetLoadData(Data data)
    {
        data.SaveGameScene(currentLoadScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefination>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            positionToGo = data.characterPosDict[playerID];
            sceneToLoad = data.GetSavedScene();

            OnLoadRequestEvent(sceneToLoad,positionToGo, true);
        }
    }
}
