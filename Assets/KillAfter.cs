using UnityEngine;

public class KillAfter : MonoBehaviour
{
    public float TTL = 3f;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TTL)
        {
            Destroy(gameObject);
        }
    }
}
