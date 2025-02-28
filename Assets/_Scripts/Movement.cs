using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleMovement : MonoBehaviour
{
    [Header("Bubble Size Control")]
    [SerializeField] private float minBubbleScale = 0.5f;
    [SerializeField] private float maxBubbleScale = 2f;
    [SerializeField] private float minStopForce = 2f;
    [SerializeField] private float maxStopForce = 2f;
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
    [SerializeField] private float minPlayerRecoilForce = 5f;
    [SerializeField] private float maxPlayerRecoilForce = 15f;
    
    [Header("Bounciness Control")]
    [SerializeField] private float minBounciness = 1f;
    [SerializeField] private float maxBounciness = 1f;
    
    [Header("Pointer Settings")]
    [SerializeField] private Transform aimPointer;
    [SerializeField] private float pointerDistance = 0.5f;
    
    
    [Header("Alternative Movement")]
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private bool relativeDecrease = false;
    [SerializeField] private float maxDecreasePercent = 0.25f;
    [SerializeField] private bool cooldown = false;
    [SerializeField] private float shotCooldown = 0.5f;
    private float shotCooldownTimer = 0f;
    
    [SerializeField] private bool autoGrow = false;
    [SerializeField] private float timeToGrow = 0.5f;
    
    
    
    private Rigidbody2D rb;
    private float currentScale = 1f;
    private float chargeStartTime;
    private bool isCharging = false;
    private Camera mainCamera;

    private bool isActive = true;

    // Calculate size change per tap
    private float totalScaleRange;
    private float sizeChangePerTap;

    private float ScalePercentage
    {
        get
        {
            return (currentScale - minBubbleScale) / (maxBubbleScale - minBubbleScale);
        }
    }

    private void Awake()
    {
        Game.EventHub.Subscribe<ResetEvent>(OnGameReset);
        Game.EventHub.Subscribe<PauseEvent>(evt => isActive = Game.SessionData.IsPaused);
        Game.EventHub.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnPlayerDeath(PlayerDeathEvent evt)
    {
        isActive = false;
    }

    private void OnGameReset(ResetEvent evt)
    {
        isActive = true;
        ResetMovement();    
    }

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
            ResetMovement();
        }

        if (!isActive)
        {
            return;
        }
        
        HandleBubbleSize();
        HandleProjectiles();

        if (rb.linearVelocity.magnitude >= maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    public void ResetMovement()
    {
        transform.position = Vector3.zero;
        rb.totalForce = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = baseGravityScale;
        currentScale = Mathf.Lerp(maxBubbleScale, minBubbleScale, 
            (baseGravityScale - minGravityScale) / (maxGravityScale - minGravityScale));
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
            
            float stopForce = Mathf.Lerp(minStopForce, maxStopForce, ScalePercentage) * -1;
        
            rb.AddForce(rb.linearVelocity.normalized * stopForce );
        }

        if (autoGrow)
        {
            float growthPerSecond = totalScaleRange / timeToGrow;
            currentScale = Mathf.Min(currentScale + (growthPerSecond * Time.deltaTime), maxBubbleScale);
            AdjustBounciness();
        }

        // Apply scale directly
        transform.localScale = Vector3.one * currentScale;

        // Update gravity scale based on current size
        float gravityScale = Mathf.Lerp(maxGravityScale, minGravityScale, ScalePercentage);
        
        // Ensure gravity maintains its original sign
        float gravitySign = Mathf.Sign(baseGravityScale);
        rb.gravityScale = gravityScale * gravitySign;
    }

    private void HandleProjectiles()
    {
        if (!isActive) return;
        
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        HandleAimPointer(direction);
        if (cooldown)
        {
            shotCooldownTimer += Time.deltaTime;
            if (shotCooldownTimer < shotCooldown)
            {
                return;
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            chargeStartTime = Time.time;
            isCharging = true;
        }

        if (Input.GetMouseButtonUp(0) && currentScale > minBubbleScale)
        {
            shotCooldownTimer = 0f;
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
            float recoilForce = Mathf.Lerp(minPlayerRecoilForce, maxPlayerRecoilForce, chargeFactor);
            rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
            
            // Calculate size change per tap (same as in HandleBubbleSize)
            totalScaleRange = maxBubbleScale - minBubbleScale;
            sizeChangePerTap = totalScaleRange / tapsToMax;
            
            // Decrease size based on charge time if relativeDecrease is true
            if (relativeDecrease)
            {
                float decreaseMultiplier = Mathf.Lerp(1f, maxDecreasePercent * tapsToMax, chargeFactor);
                currentScale = Mathf.Max(currentScale - (sizeChangePerTap * decreaseMultiplier), minBubbleScale);
            }
            else
            {
                currentScale = Mathf.Max(currentScale - sizeChangePerTap, minBubbleScale);
            }
            AdjustBounciness();

            isCharging = false;
        }
    }

    private void HandleAimPointer(Vector2 direction)
    {
        aimPointer.position = (Vector2)transform.position + direction * pointerDistance * currentScale;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        aimPointer.rotation = Quaternion.Euler(0, 0, angle);
    }
}
