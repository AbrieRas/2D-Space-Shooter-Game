using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9.4f, 9.4f);
            Vector3 spawnPosition = new Vector3(randomX, 7.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9.4f, 9.4f);
            Vector3 spawnPosition = new Vector3(randomX, 7.5f, 0);
            int randomPowerup = Random.Range(0, 3);

            GameObject newPowerup = Instantiate(_powerups[randomPowerup], spawnPosition, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;

            float randomSpawn = Random.Range(3f, 8f);
            yield return new WaitForSeconds(randomSpawn);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
