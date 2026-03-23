using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public PlayStatBar playStatBar;
    [Header("事件监听")]
    public CharacterEventSQ healthEvent;
    public SceneLoadEventSQ unloadedSceneEvent;
    public VoidEventsSQ loadDataEvent;
    public VoidEventsSQ gameOverEvent;
    public VoidEventsSQ backToMenuEvent;
    public FloatEvenetSQ syncVolumeEvent;

    [Header("广播")]
    
    public VoidEventsSQ pauseEvent;


    [Header("组件")]
    public GameObject gameOverPanel;
    public GameObject restarBtn;
    public GameObject mobileTouch;
    public Button settingsBtr;
    public GameObject pausePanel;
    public Slider volumeSlider;

    private void Awake()
    {
        #if UNITY_STANDALONE
        mobileTouch.SetActive(false);
#endif

        settingsBtr.onClick.AddListener(TogglePausePanel);
    }



    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

  

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent; 
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100f;
    }

    private void TogglePausePanel()
    {
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pauseEvent.RaiseEvent();
            pausePanel.SetActive(true);
            Time.timeScale = 0f;    
        }
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restarBtn);
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.currentHealth / character.maxHealth;
        playStatBar.OnHealthChange(percentage);


    }


    private void OnUnLoadedSceneEvent(GameSceneSQ sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.SceneType == SceneType.Menu;
        
            playStatBar.gameObject.SetActive(!isMenu);
        
    }
}
