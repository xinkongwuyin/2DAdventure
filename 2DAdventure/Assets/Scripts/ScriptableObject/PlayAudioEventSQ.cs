using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName="Event/PlayAudioEventSQ")]
public class PlayAudioEventSQ : ScriptableObject
{
    public UnityAction<AudioClip> OnEventRaised;

    public void RaiseEvent(AudioClip clip)
    {
        OnEventRaised?.Invoke(clip);
    }
}
