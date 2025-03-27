using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monsterPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2f;

    private bool isSpawning = false;
    private void Start() {
        StartCoroutine(SpawnMonsterCoroutine());
    }
    public void update()
    {
        if (isSpawning) return;
        StartCoroutine(SpawnMonsterCoroutine());
    }

    IEnumerator SpawnMonsterCoroutine()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnDelay);

        GameObject newMonster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

        Monster monsterController = newMonster.GetComponent<Monster>();
        if (monsterController != null)
        {
            monsterController.spawner = this;
        }

        isSpawning = false;
    }
}