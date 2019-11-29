using UnityEngine;


public class PlayerInventoryController : MonoBehaviour
{
    public PlayerController PlayerController;

    public GameObject Armor;
    public GameObject Sword;

    [Range(0, 100)]
    public int HealthItemPower = 50;
    [Range(1, 5)]
    public int ArmorItemDefenseMultiplier = 2;
    [Range(1, 5)]
    public int SwordItemDamageMultiplier = 2;
    [Range(1f, 100f)]
    public float ItemDurationInSeconds = 5f;

    Coroutine _armorDuration;
    Coroutine _swordDuration;

    WaitForSeconds _duration;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _duration = new WaitForSeconds(ItemDurationInSeconds);
    }

    public void AquireItem(ItemTypeEnum itemType)
    {

    }
}