using UnityEngine;


public class SpawnPointController : MonoBehaviour
{

    //============================================================
    //Fields
    public Animator SpawnAnimator;

    IDestroyExtensioner _currentInstance;

    //============================================================
    //Properties
    public bool IsVacant
    {
        get => _currentInstance == null;
    }

    //============================================================
    //Methods
    public void Spawn(GameObject objectToSpawn)
    {
        var instance = Instantiate(/*what*/objectToSpawn, /*where*/this.transform.position, /*rotation*/Quaternion.Euler(x:0f, y:Random.Range(0f,360f), z:0f));

        var destroyTrigger = instance.GetComponent(typeof(IDestroyExtensioner)) as IDestroyExtensioner;

        if(destroyTrigger != null)
        {
            _currentInstance = destroyTrigger;
            _currentInstance.Destroying += UnattachInstance;
        }

        SpawnAnimator.SetTrigger("Spawn");
    }

    public void VacantThePoint()
    {
        if(_currentInstance != null)
        {
            _currentInstance.Destroying -= UnattachInstance;
            _currentInstance.ForceDestroy();
            _currentInstance = null;
        }
    }

    void UnattachInstance()
    {
        _currentInstance.Destroying -= UnattachInstance;
        _currentInstance = null;
    }
}