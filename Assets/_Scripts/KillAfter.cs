using UnityEngine;

public class KillAfter : MonoBehaviour
{
    public float TTL = 3f;
    private float timer;
    public GameObject objectToInstantiate; // Reference to the particle effect prefab

    // Update is called once per frame
    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TTL)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Instantiate the particle effect at the collision point
        if (objectToInstantiate != null)
        {
            // Use the collision contact point for the particle effect position
            ContactPoint2D contact = collision.contacts[0];
            GameObject particleEffect = Instantiate(objectToInstantiate, transform.position, Quaternion.identity);

            // Optionally destroy the particle effect after it finishes
            Destroy(particleEffect, 2f); // Adjust the lifetime based on your particle duration
        }

        // Delay the destruction of the current object to allow the particle effect to display
        Destroy(gameObject, 0.1f); // Small delay to ensure the collision effect is visible
    }
}