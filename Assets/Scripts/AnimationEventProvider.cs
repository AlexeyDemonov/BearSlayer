using System;
using UnityEngine;

public class AnimationEventProvider : MonoBehaviour
{
    public event Action Event_Damaged;
    public event Action Event_Attack;

    void RaiseDamagedEvent() => Event_Damaged?.Invoke();
    void RaiseAttackEvent() => Event_Attack?.Invoke();
}