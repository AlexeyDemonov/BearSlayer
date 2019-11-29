using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemTypeEnum ItemType;

    public ItemTypeEnum GiveMeYourPowers()
    {
        Destroy(this.gameObject, 0.5f);
        return ItemType;
    }
}