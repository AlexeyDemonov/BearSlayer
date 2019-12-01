using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController PlayerController;
    public WaypointController WaypointController;

    Camera _mainCamera;
    bool _gameIsRunning;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _mainCamera = Camera.main;
        PlayerController.PlayerDied += () => _gameIsRunning = false;
    }

    public void StartGame()
    {
        _gameIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _gameIsRunning)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                var hittedObject = hitInfo.collider.gameObject;
                string tag = hittedObject.tag;

                switch (tag)
                {
                    case "Bear":
                        WaypointController.ShowBear(hittedObject.transform);
                        PlayerController.AttackThisBear(hittedObject.GetComponent<BearController>());
                        break;

                    case "Item":
                        WaypointController.ShowItem(hittedObject.transform);
                        PlayerController.PickupThisItem(hittedObject.GetComponent<ItemController>());
                        break;

                    default:
                        WaypointController.ShowPosition(hitInfo.point);
                        PlayerController.GoToThatPosition(hitInfo.point);
                        break;
                }
            }
        }
    }
}