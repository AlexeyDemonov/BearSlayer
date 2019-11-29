using System;
using UnityEngine;

public class ItemController : MonoBehaviour, IDestroyReporter
{
    public ItemTypeEnum ItemType;

    public event Action Destroying;

    public ItemTypeEnum GiveMeYourPowers()
    {
        Destroying?.Invoke();
        Destroy(this.gameObject);
        return ItemType;
    }
}