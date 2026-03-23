using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private Data saveData;
    private GameObject obj;

    [Header("监听事件")]
        public VoidEventsSQ saveGameEvent;
        public VoidEventsSQ loadGameEvent;


    private List<ISaveable> saveables = new List<ISaveable>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
        saveData = new Data();
    }


    private void OnEnable()
    {
        saveGameEvent.OnEventRaised += Save;
        loadGameEvent.OnEventRaised += Load;
    }


    private void OnDisable()
    {
        saveGameEvent.OnEventRaised -= Save;
        loadGameEvent.OnEventRaised -= Load;
    }

    private void Update()
    {
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            Load();
        }
    }



    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveables.Contains(saveable))//保证接口
        {
            saveables.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        if(saveables.Contains(saveable))//保证接口
        {
            saveables.Remove(saveable);
        }
    }

    private void Save()
    {
        foreach (var saveable in saveables)
        {
            saveable.GetLoadData(saveData);
        }

        foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + " " + item.Value);
        }
    }

    public void Load()
    {
        foreach (var saveable in saveables)
        {
            saveable.LoadData(saveData);
        }

    }

}
