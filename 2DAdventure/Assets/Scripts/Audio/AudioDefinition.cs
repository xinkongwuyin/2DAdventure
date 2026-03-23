using UnityEngine;

public class AudioDefinition : MonoBehaviour
{
    public PlayAudioEventSQ playAudioEventSQ;
    public AudioClip audioClip;
    public bool playOnEnable;

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        if(playOnEnable)
        {
            PlayAudioClip();
        }
    }

    public void PlayAudioClip()
    {
       playAudioEventSQ.RaiseEvent(audioClip);
    }
}
