using UnityEngine;
using UnityEngine.AI;

public abstract class GameCharacterController : MonoBehaviour
{
    //============================================================
    //Fields
    public Animator BodyAnimator;
    public AnimationEventProvider BodyAnimationEventProvider;
    public NavMeshAgent NavMeshAgent;


    [Range(0, 100)]
    public int Health = 100;

    [Range(0, 100)]
    public int Attack = 10;

    [Range(0f, 20f)]
    public float AttackDistance = 5f;

    [Range(0, 100)]
    public int Defense = 0;

    //============================================================
    //Properties
    public bool CharacterIsDead
    {
        get => Health <= 0;
    }

    //============================================================
    //Handlers
    protected virtual void Handle_AttackAnimationEvent() { }
    protected virtual void Handle_DamagedAnimationEnded() { }

    //============================================================
    //Methods
    public virtual void TakeDamage(int damage)
    {
        damage -= Defense;

        if (damage > 0 && !CharacterIsDead)
        {
            Health -= damage;
        }
    }

    //============================================================
    //Initializer
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        BodyAnimationEventProvider.Event_Attack += Handle_AttackAnimationEvent;
        BodyAnimationEventProvider.Event_Damaged += Handle_DamagedAnimationEnded;
    }
}