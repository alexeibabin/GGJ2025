using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private float timer = 0f;
    private float timeToChange = 0f;
    
    [SerializeField] private float minTimeToChange;
    [SerializeField] private float maxTimeToChange;
    [SerializeField] private bool changeSpeed;
    
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    [SerializeField] private float minXDirection = -1;
    [SerializeField] private float maxXDirection = 1;
    [SerializeField] private float minYDirection = -1;
    [SerializeField] private float maxYDirection = 1;

    [SerializeField] private GameObject Art;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeToChange = Random.Range(minTimeToChange, maxTimeToChange);
        TakeRandomDirection(true);
    }

    private void TakeRandomDirection(bool changeSpeed = true)
    {
        float speed = changeSpeed ? Random.Range(minSpeed, maxSpeed) : rb.linearVelocity.magnitude;
        
        float directionX = Random.Range(minXDirection, maxXDirection);
        float directionY = Random.Range(minYDirection, maxYDirection);
        rb.linearVelocity = new Vector2(directionX * speed, directionY * speed);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timeToChange > 0 && timer >= timeToChange)
        {
            TakeRandomDirection(changeSpeed);
            timer = 0f;
            timeToChange = Random.Range(minTimeToChange, maxTimeToChange);
        }
        
        float speed = Mathf.Clamp(rb.linearVelocity.magnitude, minSpeed, maxSpeed);
        rb.linearVelocity = rb.linearVelocity.normalized * speed;

        // Rotate the art based on movement direction
        float yRotation = rb.linearVelocity.x > 0 ? 0f : 180f;
        Art.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Edge"))
        {
            return;
        }
        // Get the collision normal
        Vector2 normal = collision.contacts[0].normal;
        
        // Calculate reflection vector using Vector2.Reflect
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 newDirection = Vector2.Reflect(currentVelocity.normalized, normal);
        
        // Maintain the same speed but change direction
        float currentSpeed = currentVelocity.magnitude;
        rb.linearVelocity = newDirection * currentSpeed * -1;
        // Draw a debug line in the new direction for visualization
        Debug.DrawRay(transform.position, newDirection * 2f, Color.red, 0.5f);
    }
}
