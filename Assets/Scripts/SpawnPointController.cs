using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public GameObject ObjectToAvoid;

    [Range(0f,100f)]
    public float DoNotSpawnIfObjectToAvoidIsCloserThan = 10f;

    IDestroyReporter _currentInstance;

    bool ObjectToAvoidIsFar
    {
        get => Vector3.Distance(this.transform.position, ObjectToAvoid.transform.position) > DoNotSpawnIfObjectToAvoidIsCloserThan;
    }

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if(_currentInstance == null && ObjectToAvoidIsFar)
        {
            var instance = Instantiate(/*what*/ObjectToSpawn, /*where*/this.transform.position, /*rotation*/Quaternion.Euler(x:0f, y:Random.Range(0f,360f), z:0f));

            var destroyReporter = instance.GetComponent(typeof(IDestroyReporter)) as IDestroyReporter;

            if(destroyReporter != null)
            {
                _currentInstance = destroyReporter;
                _currentInstance.Destroying += () => _currentInstance = null;
            }
        }
    }
}