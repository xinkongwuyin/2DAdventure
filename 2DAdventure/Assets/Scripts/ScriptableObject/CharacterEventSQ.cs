using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName="Event/CharacterEventSQ")]
public class CharacterEventSQ : ScriptableObject
{
    public UnityAction<Character> OnEventRaised;

    public void RaiseEvent(Character character)
    {
        OnEventRaised?.Invoke(character);
    }
}
