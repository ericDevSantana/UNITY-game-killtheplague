using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    //SIMPLE ENEMY SPAWN VARIABLES
    public int maxEnemySpawn;
    public float spawnRateEnemy;
    private float timerToSpawnEnemy;

    public float minAxisXspawn;
    public float maxAxisXspawn;

    public float spaceBetweenEnemies;
    public float randomScale;

    public GameObject enemies;

    public List<GameObject> enemiesPrefab;

    public GameObject tempEnemy;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //CREATE ENEMY POOL
        for(int i = 0; i < maxEnemySpawn; i++)
        {
            tempEnemy = Instantiate(enemies) as GameObject;
            enemiesPrefab.Add(tempEnemy);
            tempEnemy.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.GetCurrentGameState() == GameStates.GAMEPLAY)
        {
            timerToSpawnEnemy += Time.deltaTime;

            if (timerToSpawnEnemy > spawnRateEnemy)
            {
                timerToSpawnEnemy = 0;
                SpawnSimpleEnemy();
            }

        }
    }

    // GO THROUGH ENEMY POOL TO SPAWN A DEACTIVATED ONE
    private void SpawnSimpleEnemy()
    {
        spaceBetweenEnemies = Random.Range(minAxisXspawn, maxAxisXspawn);
        randomScale = Random.Range(.15f, .25f);

        tempEnemy = null;

        for(int i = 0; i < maxEnemySpawn; i++)
        {
            if(enemiesPrefab[i].activeSelf == false)
            {
                tempEnemy = enemiesPrefab[i];
                break;
            }
        }

        if(tempEnemy != null)
        {
            tempEnemy.transform.position = new Vector3(spaceBetweenEnemies, transform.position.y, transform.position.z);
            tempEnemy.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            tempEnemy.SetActive(true);
        }
    }

}