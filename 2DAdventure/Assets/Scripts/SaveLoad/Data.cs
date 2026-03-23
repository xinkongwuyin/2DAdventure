using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public string sceneToSave;

    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();

    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    public void SaveGameScene(GameSceneSQ savedScene)
    {
        sceneToSave = JsonUtility.ToJson(savedScene);
        Debug.Log("保存场景数据：" + sceneToSave);
    }

    public GameSceneSQ GetSavedScene()
    {
        var newScene= ScriptableObject.CreateInstance<GameSceneSQ>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
}
