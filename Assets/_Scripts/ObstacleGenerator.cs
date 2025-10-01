using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    
    public List<GameObject> obstacles = new List<GameObject>();
    public Vector2 initPoint;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("PlaceObstacle", 0, 1f);
    }

    void PlaceObstacle()
    {
        int currentTree = Random.Range(0, obstacles.Count);
        Instantiate(obstacles[currentTree], initPoint, Quaternion.identity);
    }
}
