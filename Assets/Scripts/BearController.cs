using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class BearController : GameCharacterController, IDestroyReporter
{
    //============================================================
    //Fields
    public PlayerController PlayerController;
    public float PlayerDetectionDistance;
    public float TryDetectEveryXSeconds;
    public float DestroyAfterDeathSeconds;

    WaitForSeconds _detectionWait;
    Vector3 _currentPlayerPosition;

    //============================================================
    //Events
    public event Action Destroying;

    //============================================================
    //Properties
    bool PlayerInAttackDistance
    {
        get => Vector3.Distance(this.transform.position, PlayerController.transform.position) <= AttackDistance;
    }

    //============================================================
    //Methods
    // Start is called before the first frame update
    void Start()
    {
        _detectionWait = new WaitForSeconds(TryDetectEveryXSeconds);

        BodyAnimationEventProvider.Event_Attack += Handle_AttackAnimationEvent;
        BodyAnimationEventProvider.Event_Damaged += Handle_DamagedAnimationEnded;

        StartCoroutine(TryDetectPlayer());
    }

    protected override void Handle_AttackAnimationEvent()
    {
        if (PlayerInAttackDistance)
        {
            if(!PlayerController.CharacterIsDead)
            {
                PlayerController.TakeDamage(Attack);
            }
            else
            {
                BodyAnimator.SetTrigger("Stop");
            }
        }
        else
        {
            RunToPlayerAndAttack();
        }
    }

    protected override void Handle_DamagedAnimationEnded()
    {
        if (PlayerInAttackDistance)
        {
            if(PlayerController.CharacterIsDead)
            {
                BodyAnimator.SetTrigger("Stop");
            }
        }
        else
        {
            RunToPlayerAndAttack();
        }
    }

    public override void TakeDamage(int damage)
    {
        StopAllCoroutines();
        
        base.TakeDamage(damage);

        if(!CharacterIsDead)
        {
            BodyAnimator.SetTrigger("Damaged");
        }
        else
        {
            BodyAnimator.SetTrigger("Die");
            NavMeshAgent.isStopped = true;
            Destroying?.Invoke();
            Destroy(this.gameObject, DestroyAfterDeathSeconds);
        }
    }

    

    //============================================================
    //Coroutines
    IEnumerator TryDetectPlayer()
    {
        yield return _detectionWait;

        while (Vector3.Distance(this.transform.position, PlayerController.transform.position) > PlayerDetectionDistance)
        {
            yield return _detectionWait;
        }

        //And when player enters the bounds
        RunToPlayerAndAttack();
    }

    void RunToPlayerAndAttack()
    {
        if(NavMeshAgent.isStopped)
            NavMeshAgent.isStopped = false;

        BodyAnimator.SetTrigger("Run");

        _currentPlayerPosition = PlayerController.transform.position;
        NavMeshAgent.SetDestination(_currentPlayerPosition);
        StartCoroutine(AttackWhenReachPlayer());
    }


    IEnumerator AttackWhenReachPlayer()
    {
        while (!PlayerInAttackDistance)
        {
            if(PlayerController.transform.position != _currentPlayerPosition)
            {
                _currentPlayerPosition = PlayerController.transform.position;
                NavMeshAgent.SetDestination(_currentPlayerPosition);
            }

            yield return null;
        }

        NavMeshAgent.velocity = Vector3.zero;
        NavMeshAgent.isStopped = true;
        this.transform.LookAt(_currentPlayerPosition, Vector3.up);
        BodyAnimator.SetTrigger("Attack");
    }
}