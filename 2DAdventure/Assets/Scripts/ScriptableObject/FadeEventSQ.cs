using UnityEngine;

using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/FadeEventSQ")]

public class FadeEventSQ : ScriptableObject
{

    public UnityAction<Color, float, bool> OnEventRaise;


    /// <summary>
    /// Öð½¥±äºÚ
    /// </summary>
    /// <param name="duration"></param>
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.black,duration,true);
    }
    /// <summary>
    /// Öð½¥±äÍ¸Ã÷
    /// </summary>
    /// <param name="duration"></param>
    public void FadeOut(float duration)
    {
        RaiseEvent(Color.clear,duration,false);
    }

    public void RaiseEvent(Color target, float duration, bool fadeIn)
    {
        OnEventRaise?.Invoke(target, duration, fadeIn);
    }
}
