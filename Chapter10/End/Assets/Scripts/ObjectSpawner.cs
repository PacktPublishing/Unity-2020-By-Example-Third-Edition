using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public int NumOfEachPrefabToSpawn = 6;
    public GameObject FoodPrefab;
    public GameObject RockPrefab;

    private IList<Transform> SpawnLocations = new List<Transform>();
    private int CurrentIndex = 0;

    void Awake()
    {
        foreach (Transform Child in transform)
        {
            SpawnLocations.Add(Child);
        }
        SpawnLocations.Shuffle();
    }

    public void SpawnFood()
    {
        SpawnPrefab(FoodPrefab);
    }

    public void SpawnRock()
    {
        SpawnPrefab(RockPrefab);
    }

    public void Reset()
    {
        foreach (var SpawnedLoc in SpawnLocations)
        {
            if (SpawnedLoc.childCount > 0)
            {
                Destroy(SpawnedLoc.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < NumOfEachPrefabToSpawn; ++i)
        {
            SpawnFood();
            SpawnRock();
        }
    }

    private void SpawnPrefab(GameObject Prefab)
    {
        Instantiate(Prefab, SpawnLocations[CurrentIndex], false);
        CurrentIndex = (CurrentIndex + 1) % SpawnLocations.Count;
    }
}
