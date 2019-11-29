using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemTypeEnum ItemType;

    public ItemTypeEnum GiveMeYourPowers()
    {
        return ItemType;
    }
}