using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleMovement : MonoBehaviour
{
    [Header("Bubble Size Control")]
    [SerializeField] private float minBubbleScale = 0.5f;
    [SerializeField] private float maxBubbleScale = 2f;
    [SerializeField] private int tapsToMax = 5;  // Number of taps to reach max size
    
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
    
    [FormerlySerializedAs("massMultiplier")]
    [Header("Bounciness Control")]
    [SerializeField] private float minBounciness = 1f;
    [SerializeField] private float maxBounciness = 1f;
    
    private Rigidbody2D rb;
    private float currentScale = 1f;
    private float chargeStartTime;
    private bool isCharging = false;
    private Camera mainCamera;

    // Calculate size change per tap
    private float totalScaleRange;
    private float sizeChangePerTap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        
        // Set initial gravity
        rb.gravityScale = baseGravityScale;
        
        totalScaleRange = maxBubbleScale - minBubbleScale;
        sizeChangePerTap = totalScaleRange / tapsToMax;
        
        // Set initial scale based on base gravity
        currentScale = Mathf.Lerp(maxBubbleScale, minBubbleScale, 
            (baseGravityScale - minGravityScale) / (maxGravityScale - minGravityScale));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = Vector3.zero;
            rb.totalForce = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = baseGravityScale;
            currentScale = Mathf.Lerp(maxBubbleScale, minBubbleScale, 
                (baseGravityScale - minGravityScale) / (maxGravityScale - minGravityScale));
        }
        HandleBubbleSize();
        HandleProjectiles();
    }

    private void AdjustBounciness()
    {
        float scalePercent = Mathf.InverseLerp(minGravityScale, maxGravityScale, rb.gravityScale);   
        float bounciness = Mathf.Lerp(minBounciness, maxBounciness, scalePercent);
        rb.sharedMaterial.bounciness = bounciness;
    }

    private void HandleBubbleSize()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            totalScaleRange = maxBubbleScale - minBubbleScale;
            sizeChangePerTap = totalScaleRange / tapsToMax;
            currentScale = Mathf.Min(currentScale + sizeChangePerTap, maxBubbleScale);
            AdjustBounciness();
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

        if (Input.GetMouseButtonUp(0) && currentScale > minBubbleScale)
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

            // Calculate size change per tap (same as in HandleBubbleSize)
            float totalScaleRange = maxBubbleScale - minBubbleScale;
            float sizeChangePerTap = totalScaleRange / tapsToMax;
            
            // Decrease size by one tap's worth
            currentScale = Mathf.Max(currentScale - sizeChangePerTap, minBubbleScale);
            AdjustBounciness();
            isCharging = false;
        }
    }
}
