using System;
using System.Collections;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    //============================================================
    //Fields
    public SpawnPointController[] SpawnPoints;

    [Space(15f)]

    public GameObject Player;
    [Range(0f, 100f)]
    public float DoNotSpawnIfPlayerIsCloserThan = 10f;

    [Space(15f)]

    public GameObject BearPrefab;
    public GameObject[] ItemsPrefabs;
    [Tooltip("X out of 10 times Bear will be spawned instead of Item")]
    [Range(1,10)]
    public int BearOverItemsSpawnChance = 5;

    [Space(15f)]

    public int StartSpawnAmount;
    public float SpawnEveryXSeconds;
    public int IncreaseSpawnEveryXTurns;

    Coroutine _spawning;
    WaitForSeconds _interSpawnWait;
    int _spawnRateThisTurn;
    int _turnCounter = 0;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        _interSpawnWait = new WaitForSeconds(SpawnEveryXSeconds);
        PlayerController.PlayerDied += StopSpawning;
    }

    public void StartSpawning(int bearCoeff)
    {
        BearOverItemsSpawnChance = bearCoeff;
        StartSpawning();
    }

    public void StartSpawning()
    {
        _spawnRateThisTurn = (StartSpawnAmount > SpawnPoints.Length) ? SpawnPoints.Length : StartSpawnAmount;
        _turnCounter = 0;
        _spawning = StartCoroutine(SpawnOverTime());
    }

    public void RestartSpawning()
    {
        foreach (var point in SpawnPoints)
        {
            point.VacantThePoint();
        }

        StartSpawning();
    }

    void StopSpawning()
    {
        StopCoroutine(_spawning);
    }

    IEnumerator SpawnOverTime()
    {
        while (true)
        {
            for (int i = 0; i < _spawnRateThisTurn; i++)
            {
                var point = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];

                if (point.IsVacant && Vector3.Distance(point.transform.position, Player.transform.position) > DoNotSpawnIfPlayerIsCloserThan)
                {
                    int dice = UnityEngine.Random.Range(0, 11);

                    if (dice > BearOverItemsSpawnChance)
                        point.Spawn(ItemsPrefabs[UnityEngine.Random.Range(0, ItemsPrefabs.Length)]);
                    else
                        point.Spawn(BearPrefab);
                }
                else
                {
                    //I want it to just skip this spawn if point is occupied or close to Player, so that is intentional
                }
            }


            _turnCounter++;
            if (_turnCounter >= IncreaseSpawnEveryXTurns)
            {
                _turnCounter = 0;
                _spawnRateThisTurn = (_spawnRateThisTurn < SpawnPoints.Length) ? _spawnRateThisTurn + 1 : SpawnPoints.Length;
            }


            yield return _interSpawnWait;
        }
    }
}