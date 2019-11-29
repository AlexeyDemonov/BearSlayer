using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WaypointController : MonoBehaviour
{
    Animator _animator;

    //============================================================
    //Private methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void ChangePositionAndShow(Vector3 newPosition, string animationName)
    {
        this.transform.position = newPosition;
        _animator.SetTrigger(animationName);
    }

    //============================================================
    //Public methods
    public void ShowPosition(Vector3 newPosition)
    {
        ChangePositionAndShow(newPosition, "ShowPosition");
    }

    public void ShowBear(Transform bearPosition)
    {
        ChangePositionAndShow(bearPosition.position, "ShowBear");
    }

    public void ShowItem(Transform itemPosition)
    {
        ChangePositionAndShow(itemPosition.position, "ShowItem");
    }
}