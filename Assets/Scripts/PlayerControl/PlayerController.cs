using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class PlayerController : GameCharacterController
{
    //============================================================
    //Fields
    public PlayerInventoryController PlayerInventory;

    public float WalkSpeed;
    public float RunSpeed;
    public float WalkToRunSwitchDistance;

    public float StopAndIdleDistance;
    public float ItemPickupDistance;


    WaitForSeconds _halfSecondWait = new WaitForSeconds(0.5f);

    BearController _bearToAttack = null;
    ItemController _itemToPickup = null;

    //============================================================
    //Properties
    new public int Health
    {
        get => base.Health;

        set
        {
            base.Health += value;

            if(base.Health > 100)
                base.Health = 100;
        }
    }

    //============================================================
    //Methods
    void GoToPosition(Vector3 position)
    {
        if(NavMeshAgent.isStopped)
            NavMeshAgent.isStopped = false;

        var distance = Vector3.Distance(this.transform.position, position);

        if(distance > WalkToRunSwitchDistance)
        {
            NavMeshAgent.speed = RunSpeed;
            BodyAnimator.SetTrigger("Run");
        }
        else
        {
            NavMeshAgent.speed = WalkSpeed;
            BodyAnimator.SetTrigger("Walk");
        }

        NavMeshAgent.SetDestination(position);
    }

    void StopPreviousActions()
    {
        StopAllCoroutines();
        _bearToAttack = null;
        _itemToPickup = null;
    }

    public void GoToThatPosition(Vector3 newPosition)
    {
        if (!CharacterIsDead)
        {
            StopPreviousActions();
            GoToPosition(newPosition);
            StartCoroutine(StopAndIdleWhenReachPosition());
        }
    }

    public void AttackThisBear(BearController bear)
    {
        if (!this.CharacterIsDead && !bear.CharacterIsDead)
        {
            StopPreviousActions();
            _bearToAttack = bear;
            GoToPosition(_bearToAttack.gameObject.transform.position);
            StartCoroutine(StopAndAttackWhenReachBear());
        }
    }

    public void PickupThisItem(ItemController item)
    {
        if(!CharacterIsDead)
        {
            StopPreviousActions();
            _itemToPickup = item;
            GoToPosition(item.gameObject.transform.position);
            StartCoroutine(StopAndPickupWhenReachItem());
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if/*now*/(CharacterIsDead)
        {
            StopPreviousActions();
            BodyAnimator.SetTrigger("Die");
        }
    }

    //============================================================
    //Coroutines
    IEnumerator RunUntilDistance(float distance)
    {
        yield return _halfSecondWait;

        while (NavMeshAgent.remainingDistance > distance)
        {
            yield return null;
        }

        NavMeshAgent.velocity = Vector3.zero;
        NavMeshAgent.isStopped = true;
    }

    IEnumerator StopAndIdleWhenReachPosition()
    {
        yield return StartCoroutine(RunUntilDistance(StopAndIdleDistance));

        BodyAnimator.SetTrigger("Stop");
    }

    IEnumerator StopAndAttackWhenReachBear()
    {
        yield return StartCoroutine(RunUntilDistance(AttackDistance));

        this.transform.LookAt(_bearToAttack.transform.position, Vector3.up);
        BodyAnimator.SetTrigger("Attack");
    }

    IEnumerator StopAndPickupWhenReachItem()
    {
        yield return StartCoroutine(RunUntilDistance(ItemPickupDistance));

        BodyAnimator.SetTrigger("Stop");

        PlayerInventory.AquireItem(_itemToPickup.GiveMeYourPowers());
    }

    //============================================================
    //Animation event handlers
    protected override void Handle_AttackAnimationEvent()
    {
        if (_bearToAttack != null)
        {
            if (Vector3.Distance(this.transform.position, _bearToAttack.transform.position) <= AttackDistance)
            {
                if (!_bearToAttack.CharacterIsDead)
                {
                    _bearToAttack.TakeDamage(Attack);
                }
                else
                {
                    StopAllCoroutines();
                    BodyAnimator.SetTrigger("Stop");
                    _bearToAttack = null;
                }
            }
            else
            {
                AttackThisBear(_bearToAttack);
            }
        }
    }
}