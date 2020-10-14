using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int asteroidCount;
    public int currentLevel = 1;
    public GameObject asteroid;
    public GameObject levelPassed;

    private void Start()
    {
        levelPassed = GameObject.FindGameObjectWithTag("Level Passed");
    }

    public void UpdateAsteroidCount(int change)
    {
        asteroidCount += change;

        if (asteroidCount <= 0)
        {
            // new level
            levelPassed.SetActive(true);
            Invoke("StartNewLevel", 4f);
        }
    }

    void StartNewLevel()
    {
        levelPassed.SetActive(false);
        currentLevel++;
        for (int i = 0; i < currentLevel * 2; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-25f, 25f), 15f); // x: randomized, y: top of screen
            Instantiate(asteroid, spawnPosition, Quaternion.identity); // Quaternion.identity is a default/empty rotation
            asteroidCount++;
        }
    }
}
