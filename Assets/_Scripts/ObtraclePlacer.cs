using UnityEngine;
using System;


public class ObstraclePlacer : MonoBehaviour
{

    public GameObject obstacle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("PlaceObstacle", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaceObstacle()
    { 
        float minY = -5f;
        float maxY = 5f;
        float minX = -100f;
        float maxX = 100f;
        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomY = UnityEngine.Random.Range(minY, maxY);
        Vector2 pos = new Vector2(randomX, randomY);
        Instantiate(obstacle, pos, Quaternion.identity);
    }
}
