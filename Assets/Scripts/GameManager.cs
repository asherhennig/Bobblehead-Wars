using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;           //player object
    public GameObject[] spawnPoints;    //Spawn point object array
    public GameObject alien;            //Alien object

    public int maxAliensOnScreen;       //Max number of aliens
    public int totalAliens;             //Total number of aliens
    public float minSpawnTime;          //Minimum time between spawns
    public float maxSpawnTime;          //Maximum time between spawns
    public int aliensPerSpawn;          //Rate of alien spawns

    //Private variable initialization
    private int aliensOnScreen = 0;             //Tracks number of aliens on screen
    private float generatedSpawnsTime = 0;      //Time between spawns (randomize)
    private float currentSpawnTime = 0;         //Tracks time since last spawn

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                    }
                }
            }
        }
    }
}
