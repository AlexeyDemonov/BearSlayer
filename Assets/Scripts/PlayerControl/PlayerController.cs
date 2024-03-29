using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : GameCharacterController
{
    //============================================================
    //Fields
    public Transform StartPosition;

    public PlayerInventoryController PlayerInventory;
    public Slider HealthBar;

    public float WalkSpeed;
    public float RunSpeed;
    public float WalkToRunSwitchDistance;

    public float StopAndIdleDistance;
    public float ItemPickupDistance;

    WaitForSeconds _halfSecondWait = new WaitForSeconds(0.5f);

    BearController _bearToAttack = null;
    Vector3 _bearLastPosition = Vector3.zero;

    ItemController _itemToPickup = null;

    //============================================================
    //Properties
    new public int Health
    {
        get => base.Health;

        set
        {
            base.Health = value;

            if (base.Health > 100)
                base.Health = 100;
            if(base.Health < 0)
                base.Health = 0;

            HealthBar.value = base.Health;
        }
    }

    //============================================================
    //Events
    public static event Action PlayerDied;

    //============================================================
    //Methods
    void GoToPosition(Vector3 newPosition)
    {
        if (NavMeshAgent.isStopped)
            NavMeshAgent.isStopped = false;

        var distance = Vector3.Distance(this.transform.position, newPosition);

        if (distance > WalkToRunSwitchDistance)
        {
            NavMeshAgent.speed = RunSpeed;
            BodyAnimator.SetTrigger("Run");
        }
        else
        {
            NavMeshAgent.speed = WalkSpeed;
            BodyAnimator.SetTrigger("Walk");
        }

        NavMeshAgent.SetDestination(newPosition);
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
            StartCoroutine(KeepTrackingTheBear());
        }
    }

    public void PickupThisItem(ItemController item)
    {
        if (!CharacterIsDead)
        {
            StopPreviousActions();
            _itemToPickup = item;
            GoToPosition(item.gameObject.transform.position);
            StartCoroutine(StopAndPickupWhenReachItem());
        }
    }

    public override void TakeDamage(int damage)
    {
        if(!CharacterIsDead)
        {
            base.TakeDamage(damage);
            HealthBar.value = Health;

            if/*now*/(CharacterIsDead)
            {
                StopPreviousActions();
                NavMeshAgent.isStopped = true;
                NavMeshAgent.velocity = Vector3.zero;
                PlayerDied?.Invoke();
                BodyAnimator.SetTrigger("Die");
            }
        }
    }

    public void Revive()
    {
        this.transform.position = StartPosition.position;
        BodyAnimator.SetTrigger("Stop");
        Health = 100;
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

    IEnumerator StopAndPickupWhenReachItem()
    {
        yield return StartCoroutine(RunUntilDistance(ItemPickupDistance));

        BodyAnimator.SetTrigger("Stop");

        PlayerInventory.AquireItem(_itemToPickup.GiveMeYourPowers());
    }

    IEnumerator StopAndAttackWhenReachBear()
    {
        yield return StartCoroutine(RunUntilDistance(AttackDistance));

        this.transform.LookAt(_bearToAttack.transform.position, Vector3.up);
        BodyAnimator.SetTrigger("Attack");
    }

    IEnumerator KeepTrackingTheBear()
    {
        _bearLastPosition = _bearToAttack.transform.position;
        yield return _halfSecondWait;

        while (Vector3.Distance(this.transform.position, _bearToAttack.transform.position) > AttackDistance && NavMeshAgent.isStopped == false)
        {
            if (_bearLastPosition != _bearToAttack.transform.position)
            {
                _bearLastPosition = _bearToAttack.transform.position;
                NavMeshAgent.SetDestination(_bearLastPosition);
            }

            yield return _halfSecondWait;
        }
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