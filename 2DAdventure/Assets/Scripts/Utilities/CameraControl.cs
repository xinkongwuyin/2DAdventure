using UnityEngine;
using Cinemachine;
using System.Diagnostics.Eventing.Reader;
using System;
public class CameraControl : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventsSQ afterSceneLoadedEvent;
    private CinemachineConfiner2D confiner;
    public CinemachineImpulseSource impulseSource;
    public VoidEventsSQ cameraShakeEvent;
    private void Awake()
    {
        confiner = GetComponent<CinemachineConfiner2D>();
    }

    //private void Start()
    //{
    //    GetNewCameraBounds();
    //}
    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
        {
            Debug.LogError("未找到Bounds对象，无法设置相机边界。请确保场景中存在一个带有Collider2D组件的对象，并且该对象的标签为'Bounds'。");
            return;
        }
        confiner.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner.InvalidateCache();
    }

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += onCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= onCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBounds();
    }

    private void onCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }



}
