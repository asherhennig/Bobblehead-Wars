using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;           //player object
    public GameObject[] spawnPoints;    //Spawn point object array
    public GameObject alien;            //Alien object
    public GameObject upgradePrefab;
    public GameObject deathFloor;

    public Gun gun;
    public Animator arenaAnimator;

    public int maxAliensOnScreen;       //Max number of aliens
    public int totalAliens;             //Total number of aliens    
    public int aliensPerSpawn;          //Rate of alien spawns
    public float minSpawnTime;          //Minimum time between spawns
    public float maxSpawnTime;          //Maximum time between spawns
    public float upgradeMaxTimeSpawn = 7.5f;
    
    //Private variable initialization
    private int aliensOnScreen = 0;             //Tracks number of aliens on screen
    private float generatedSpawnsTime = 0;      //Time between spawns (randomize)
    private float currentSpawnTime = 0;         //Tracks time since last spawn
    private float actualUpgradeTime = 0;
    private float currentUpgradeTime = 0;
    private bool spawnedUpgrade = false;

    // Start is called before the first frame update
    void Start()
    {
        actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f,
                            upgradeMaxTimeSpawn);
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        currentUpgradeTime += Time.deltaTime;

        if (currentUpgradeTime > actualUpgradeTime)
        {
            //1
            if (!spawnedUpgrade)
            {
                //2
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                //3
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                //4
                spawnedUpgrade = true;

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

        currentSpawnTime += Time.deltaTime;     //Calculates the time since last frame update

        //Spawn time randomizer
        if (currentSpawnTime > generatedSpawnsTime)
        {
            currentSpawnTime = 0;
            generatedSpawnsTime = Random.Range(minSpawnTime, maxSpawnTime);
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                List<int> previousSpawnLocations = new List<int>();         //keeps track of spawned aliens
                //Limits number of spawns to number of spawns points
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }
                //Stop code from spawning more than max number of aliens
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ?
                    aliensPerSpawn - totalAliens : aliensPerSpawn;

                //Alien Spawning
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        aliensOnScreen += 1;

                        // 1
                        int spawnPoint = -1;
                        // 2
                        while (spawnPoint == -1)
                        {
                            // 3
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            // 4
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }
                        GameObject spawnLocation = spawnPoints[spawnPoint];
                        GameObject newAlien = Instantiate(alien) as GameObject;
                        newAlien.transform.position = spawnLocation.transform.position;

                        //Alien targeting system
                        Alien alienScript = newAlien.GetComponent<Alien>();
                        alienScript.target = player.transform;

                        Vector3 targetRotation = new Vector3(player.transform.position.x,
                                                            newAlien.transform.position.y,
                                                            player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);

                        alienScript.OnDestroy.AddListener(AlienDestoryed);

                        alienScript.GetDeathParticles().SetDeathFloor(deathFloor);
                    }
                }
            }
        }
    }

    public void AlienDestoryed()
    {
        aliensOnScreen -= 1;
        totalAliens -= 1;

        if (totalAliens == 0)
        {
            Invoke("endGame", 2.0f);
        }
    }

    private void endGame()
    {
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.elevatorArrived);
        arenaAnimator.SetTrigger("PlayerWon");
    }
}
