using UnityEngine;

public class TeleportPoint : MonoBehaviour, IIteractable
{
    public SceneLoadEventSQ loadEventSQ;
    public GameSceneSQ sceneToGo;
    public Vector3 positionToGo;

    public void triggerAction()
    {
        loadEventSQ.RaisesLoadRequestEvent(sceneToGo, positionToGo, true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
