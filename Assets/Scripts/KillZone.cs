using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the collider entering the KillZone is a comet, destroy it
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Star") || other.gameObject.CompareTag("AlienShip"))
        {
            Destroy(other.gameObject);
        }
    }
}