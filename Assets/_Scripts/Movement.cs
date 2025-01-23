using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleMovement : MonoBehaviour
{
    [Header("Bubble Size Control")]
    [SerializeField] private float minBubbleScale = 0.5f;
    [SerializeField] private float maxBubbleScale = 2f;
    [SerializeField] private float scaleSpeed = 2f;
    [SerializeField] private float minScaleDecrease = 0.2f;  // Added: minimum size decrease per shot
    [SerializeField] private float maxScaleDecrease = 0.5f;  // Added: maximum size decrease per shot
    
    [Header("Gravity Control")]
    [SerializeField] private float minGravityScale = 0.2f;
    [SerializeField] private float baseGravityScale = 0.6f;    // Should be between min and max
    [SerializeField] private float maxGravityScale = 1f;
    
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float minProjectileForce = 5f;
    [SerializeField] private float maxProjectileForce = 15f;
    [SerializeField] private float minProjectileScale = 0.5f;
    [SerializeField] private float maxProjectileScale = 2f;
    
    private Rigidbody2D rb;
    private float currentScale = 1f;
    private float chargeStartTime;
    private bool isCharging = false;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        
        // Set initial gravity
        rb.gravityScale = baseGravityScale;
        
        // Set initial scale based on base gravity
        currentScale = Mathf.Lerp(maxBubbleScale, minBubbleScale, 
            (baseGravityScale - minGravityScale) / (maxGravityScale - minGravityScale));
    }

    // Update is called once per frame
    void Update()
    {
        HandleBubbleSize();
        HandleProjectiles();
    }

    private void HandleBubbleSize()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Instantly increase size by a step
            currentScale = Mathf.Min(currentScale + (scaleSpeed * Time.deltaTime), maxBubbleScale);
        }

        // Apply scale directly
        transform.localScale = Vector3.one * currentScale;

        // Update gravity scale based on current size
        float t = (currentScale - minBubbleScale) / (maxBubbleScale - minBubbleScale);
        float gravityScale = Mathf.Lerp(maxGravityScale, minGravityScale, t);
        
        // Ensure gravity maintains its original sign
        float gravitySign = Mathf.Sign(baseGravityScale);
        rb.gravityScale = gravityScale * gravitySign;
    }

    private void HandleProjectiles()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        if (Input.GetMouseButtonDown(0))
        {
            chargeStartTime = Time.time;
            isCharging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float chargeTime = Time.time - chargeStartTime;
            float chargeFactor = Mathf.Clamp01(chargeTime);

            // Create and setup projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            
            // Scale projectile based on charge time
            float projectileScale = Mathf.Lerp(minProjectileScale, maxProjectileScale, chargeFactor);
            projectile.transform.localScale = Vector3.one * projectileScale;

            // Apply force to projectile
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            float projectileForce = Mathf.Lerp(minProjectileForce, maxProjectileForce, chargeFactor);
            projectileRb.AddForce(direction * projectileForce, ForceMode2D.Impulse);

            // Apply recoil force to player
            rb.AddForce(-direction * projectileForce, ForceMode2D.Impulse);

            // Calculate size decrease based on projectile scale
            float scaleDecrease = Mathf.Lerp(minScaleDecrease, maxScaleDecrease, chargeFactor);
            currentScale = Mathf.Max(currentScale - scaleDecrease, minBubbleScale);

            isCharging = false;
        }
    }
}
