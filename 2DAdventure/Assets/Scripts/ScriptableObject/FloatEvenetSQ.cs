using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "FloatEvent", menuName = "Event/FloatEvent")]
public class FloatEvenetSQ: ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void RaiseEvent(float amount)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(amount);
        }
    }
}
