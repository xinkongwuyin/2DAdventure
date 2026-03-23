using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("组件")]
    public AudioSource BGMSource;
    public AudioSource FXSource;

    [Header("广播")]
    public FloatEvenetSQ syncVolumeEvent;

    [Header("事件监听")]
    public PlayAudioEventSQ FXEvent;
    public PlayAudioEventSQ BGMEvent;
    public FloatEvenetSQ volumeEvent;
    public VoidEventsSQ pasueGameEvent;


    public AudioMixer audioMixer;

    private void OnEnable()
    {
        BGMEvent.OnEventRaised += OnBGMEvent;
        FXEvent.OnEventRaised += OnFXEvent;
        volumeEvent.OnEventRaised += OnVolumeEvent;
        pasueGameEvent.OnEventRaised += OnPasueEvent;
    }

    private void OnDisable()
    {
        BGMEvent.OnEventRaised -= OnBGMEvent;
        FXEvent.OnEventRaised -= OnFXEvent;
        volumeEvent.OnEventRaised -= OnVolumeEvent;
        pasueGameEvent.OnEventRaised -= OnPasueEvent;
    }

    private void OnPasueEvent()
    {
        float amount;
        audioMixer.GetFloat("MasterVolume", out amount);
        syncVolumeEvent.RaiseEvent(amount);
    }

    private void OnVolumeEvent(float amount)
    {
         audioMixer.SetFloat("MasterVolume", amount*100-80);
    }

    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip=clip;
        FXSource.Play();
    }

    private void OnBGMEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }
}
