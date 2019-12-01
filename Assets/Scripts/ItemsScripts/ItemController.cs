using System;
using UnityEngine;

public class ItemController : MonoBehaviour, IDestroyExtensioner
{
    public ItemTypeEnum ItemType;

    public event Action Destroying;

    public ItemTypeEnum GiveMeYourPowers()
    {
        Destroying?.Invoke();
        Destroy(this.gameObject);
        return ItemType;
    }

    void IForceDestroyer.ForceDestroy()
    {
        Destroying?.Invoke();
        Destroy(this.gameObject);
    }
}