using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/SceneLoadEventSQ")]
public class SceneLoadEventSQ:ScriptableObject
{
    public UnityAction<GameSceneSQ,Vector3,bool> LoadRequestEvent;
    ///<summary>
    ///场景加载需求
    ///</summary>
    ///<param name="locationToLoad">
    ///<param name="posToGo">
    ///<param name="fadeScreen">
    public void RaisesLoadRequestEvent(GameSceneSQ locationToGo, Vector3 posToGo, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToGo, posToGo, fadeScreen);
    }

}
