using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float simpleEnemyMovementRate;
    public float minHorizontalMove;
    public float maxHorizontalMove;

    public GameController gameController;
    
    public Material material;
    RaycastHit2D hitInfo;

    public bool dead = false;
    public bool isFirstTime = true;

    public int curveMovement = 0;
    public int touchCount;

    public float disolving = 1f;
    public float multi = 0.5f;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        material = GetComponent<SpriteRenderer>().material;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.GetCurrentGameState() == GameStates.GAMEPLAY)
        {
            //ONLY MOVES OR GET CLICKED IF IT IS NOT DEAD
            if (!dead)
            {
                // GAME OBJECT JUST GOT ACTIVATED? SET NEW RANDOM SIN AMPLITUDE (MULTI) AND OFFSET...AND IF IT IS GONNA HAVE SIN MOVEMENT OR STRAIGHT MOVEMENT
                if (isFirstTime)
                {
                    multi = Random.Range(-0.7f, 0.7f);
                    offset = Random.Range(-1.7f, 1.7f);
                    curveMovement = Random.Range(0, 2);

                    isFirstTime = false;

                }

                if(curveMovement == 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - simpleEnemyMovementRate, 0);
                }
                else
                {
                    transform.position = new Vector2(multi * Mathf.Sin(transform.position.y) + offset, transform.position.y - simpleEnemyMovementRate);
                }

                //Y LESS THAN -5 LOSE, SET SPAWN RATE AND MOVEMENT RATE BACK TO PRESET
                if (transform.position.y <= -5f)
                {
                    simpleEnemyMovementRate = 0.1f;
                    gameController.spawner.GetComponent<SpawnController>().spawnRateEnemy = 0.5f; ;
                    gameController.CallGameOver();
                    gameObject.SetActive(false);
                }

                //ENABLE MULTI TOUCH ON SCREEN
                if (Input.touchCount > 0)
                {
                    touchCount = Input.touchCount;

                    for (int i = 0; i < touchCount; i++)
                    {
                        if (Input.GetTouch(i).phase == TouchPhase.Began)
                        {
                            hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);

                            if (hitInfo)
                            {

                                if (hitInfo.collider.transform == transform)
                                {
                                    AudioController.PlaySound(SoundEffects.SPLASH);

                                    //INCREASE SPEED AND SPAWN RATE EVERY ENEMY KILLED OR TOUCHED
                                    simpleEnemyMovementRate += 0.001f;

                                    if(gameController.spawner.GetComponent<SpawnController>().spawnRateEnemy <= 0)
                                    {
                                        gameController.spawner.GetComponent<SpawnController>().spawnRateEnemy = 0;
                                    }
                                    else
                                    {
                                        gameController.spawner.GetComponent<SpawnController>().spawnRateEnemy -= simpleEnemyMovementRate * 0.001f;
                                    }
                                    
                                    isFirstTime = true;
                                    dead = true;
                                }
                            }
                        }
                    }
                }

                //ENABLE MOUSE CLICK
                /*
                if(Input.GetMouseButtonDown(0))
                {
                    hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hitInfo)
                    {

                        if (hitInfo.collider.transform == transform)
                        {
                            isFirstTime = true;
                            dead = !dead;
                        }
                    }
                }*/

            }
        }

        if(dead)
        {
            KillEnemy();
        }
    }

    //CHAGE MATERIAL PROPERTY TO GIVE SHADER EFFECT
    public void KillEnemy()
    {
        material.SetFloat("_Fade", disolving);
        disolving -= Time.deltaTime * 2;

        if (disolving <= 0f)
        {
            dead = false;
            gameObject.SetActive(false);
            material.SetFloat("_Fade", 1f);
            disolving = 1f;

        }
    }

}