using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpawnPointController : MonoBehaviour
{
    public Animator SpawnAnimator;

    IDestroyReporter _currentInstance;

    public bool IsVacant
    {
        get => _currentInstance == null;
    }

    public void Spawn(GameObject objectToSpawn)
    {
        var instance = Instantiate(/*what*/objectToSpawn, /*where*/this.transform.position, /*rotation*/Quaternion.Euler(x:0f, y:Random.Range(0f,360f), z:0f));

        var destroyReporter = instance.GetComponent(typeof(IDestroyReporter)) as IDestroyReporter;

        if(destroyReporter != null)
        {
            _currentInstance = destroyReporter;
            _currentInstance.Destroying += () => _currentInstance = null;
        }

        SpawnAnimator.SetTrigger("Spawn");
    }
}