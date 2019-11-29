using UnityEngine;
using System.Collections;


public class PlayerInventoryController : MonoBehaviour
{
    //============================================================
    //Fields
    public PlayerController PlayerController;

    public GameObject ArmorIndicator;
    public GameObject SwordIndicator;

    [Range(0, 100)]
    public int HealthItemPower = 50;
    [Range(1, 100)]
    public int ArmorItemDefenseAddition = 5;
    [Range(1, 100)]
    public int SwordItemDamageAddition = 20;
    [Range(1f, 100f)]
    public float ItemDurationInSeconds = 5f;

    Coroutine _armorDurationCoroutine;
    Coroutine _swordDurationCoroutine;

    WaitForSeconds _duration;

    //============================================================
    //Methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _duration = new WaitForSeconds(ItemDurationInSeconds);
    }

    public void AquireItem(ItemTypeEnum itemType)
    {
        switch (itemType)
        {
            case ItemTypeEnum.HEALTH:
                PlayerController.Health += HealthItemPower;
                break;


            case ItemTypeEnum.ARMOR:

                if(_armorDurationCoroutine == null)
                {
                    ArmorIndicator.SetActive(true);
                    PlayerController.Defense += ArmorItemDefenseAddition;
                }
                else/*if (_armorDurationCoroutine != null)*/
                    StopCoroutine(_armorDurationCoroutine);

                _armorDurationCoroutine = StartCoroutine(RemoveArmorAfterTime());

                break;


            case ItemTypeEnum.SWORD:

                if(_swordDurationCoroutine == null)
                {
                    SwordIndicator.SetActive(true);
                    PlayerController.Attack += SwordItemDamageAddition;
                }
                else/*if (_swordDurationCoroutine != null)*/
                    StopCoroutine(_swordDurationCoroutine);

                _swordDurationCoroutine = StartCoroutine(RemoveSwordAfterTime());

                break;
        }
    }

    //============================================================
    //Coroutines
    IEnumerator RemoveArmorAfterTime()
    {
        yield return _duration;
        PlayerController.Defense -= ArmorItemDefenseAddition;
        _armorDurationCoroutine = null;
        ArmorIndicator.SetActive(false);
    }

    IEnumerator RemoveSwordAfterTime()
    {
        yield return _duration;
        PlayerController.Attack -= SwordItemDamageAddition;
        _swordDurationCoroutine = null;
        SwordIndicator.SetActive(false);
    }
}