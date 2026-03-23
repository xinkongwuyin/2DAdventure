using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game Scene/GameSceneSQ")]
public class GameSceneSQ:ScriptableObject
{
    public SceneType SceneType;
    public AssetReference sceneReference;

}
