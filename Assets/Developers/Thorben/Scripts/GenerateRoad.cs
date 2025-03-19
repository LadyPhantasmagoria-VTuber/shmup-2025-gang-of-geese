using UnityEngine;
using System.Collections.Generic;

public class InfiniteHighway : MonoBehaviour
{
    public GameObject roadPrefab;        // The road segment prefab
    public int numberOfRoads = 5;       // Number of road segments to instantiate at once
    public float roadLength = 20f;      // Length of each road segment
    public float speed = 10f;           // Speed of the road moving

    private List<GameObject> roads;     // List of current road segments
    private float spawnZ = 0f;          // The z position to spawn the road

    void Start()
    {
        roads = new List<GameObject>();
        for (int i = 0; i < numberOfRoads; i++)
        {
            SpawnRoad();
        }
    }

    void Update()
    {
        MoveRoads();
        if (roads[0].transform.position.z < -roadLength)
        {
            RecycleRoad();
        }
    }

    void MoveRoads()
    {
        foreach (var road in roads)
        {
            road.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }

    void SpawnRoad()
    {
        // The position where the new road segment will spawn.
        float spawnPositionZ = spawnZ + roadLength / 2f;
        GameObject newRoad = Instantiate(roadPrefab, new Vector3(0f, 0f, spawnPositionZ), Quaternion.identity);
        roads.Add(newRoad);
        spawnZ += roadLength;  // Update spawn position for the next road
    }

    void RecycleRoad()
    {
        GameObject oldRoad = roads[0];
        roads.RemoveAt(0);
        Destroy(oldRoad);  // Remove the old road segment
        SpawnRoad();       // Spawn a new road segment at the end, aligned correctly
    }
}
