using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Time Control")]
    public float timePerRound;

    [Header("Spawnables")]
    public List<GameObject> factoriesPrefabs;
    public List<GameObject> housePrefabs;

    [HideInInspector]
    GameGrid gameGridScript;
    public int completedTravel = 0;

    void Start()
    {
        GameObject gridGO = GameObject.Find("GameGrid");
        gameGridScript = gridGO.GetComponent<GameGrid>();
        TutorialRound();
    }

    // Update is called once per frame
    void Update()
    {
        timePerRound -= Time.deltaTime;

        if(timePerRound <= 0.0f)
        {
            RoundEnd();
            
        }

    }

    private void TutorialRound()
    {
        //Spawn one factory

        //Spawn one house
    }

    private void RoundEnd()
    {
        SpawnFactories();
        SpawnHouse();
    }

    private void SpawnFactories()
    {

    }

    private void SpawnHouse()
    {
        int numberOfHouses = Random.Range(0, 2) + 1;

        for(int i = 0; i < numberOfHouses; ++i)
        {
            SpawnOnAvailableTile("house");
        }
    }

    public void SpawnOnAvailableTile(string type)
    {
        
    }
}
