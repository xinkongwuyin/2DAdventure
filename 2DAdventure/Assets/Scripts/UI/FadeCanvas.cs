
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class FadeCanvas: MonoBehaviour
{
    [Header("ÊÂ¼þ¼àÌý")]
    public FadeEventSQ fadeEvent;

    public Image FadeImage;

    private void OnEnable()
    {
        fadeEvent.OnEventRaise += OnFadeEvent;
    } 

    private void OnDisable()
    {
        fadeEvent.OnEventRaise -= OnFadeEvent;
    }



    private void OnFadeEvent(Color target, float duration,bool fadeIn)
    {
        FadeImage.DOBlendableColor(target, duration);
    }

}
