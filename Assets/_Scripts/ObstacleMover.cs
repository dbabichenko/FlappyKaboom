using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public int minSpeed = 1;
    public int maxSpeed = 10;
    

    private int speed;

    private void Start()
    {
        float growScale = Random.Range(1f, 4f);
        speed = Random.Range(minSpeed, maxSpeed);
        transform.localScale += Vector3.one * growScale;
    }

    void Update()
    {
        Vector2 pos = new Vector2(
            gameObject.transform.position.x - speed * Time.deltaTime,
            gameObject.transform.position.y);
        gameObject.transform.position = pos;
    }
}
