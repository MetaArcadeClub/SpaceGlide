using UnityEngine;

public class Comet : MonoBehaviour
{
    public float mass = 1.0f;
    private float bottomEdge;
    private const float MAX_MASS = 10f;

    private static float massIncreaseFactor = 1f;  // Factor by which the mass will increase

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1; // This assumes your gravity settings in Unity are set to make the object fall downwards.
        }

        mass = Random.Range(0.5f, 1.5f);
        Debug.Log("Comet mass: " + mass);

        rb.mass = mass;

        // Cap the mass at a maximum value
        if (mass > MAX_MASS) mass = MAX_MASS;

        bottomEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y - 1f;
    }

    private void Update()
    {
        // Destroying the comet once it passes the bottom edge of the screen
        if (transform.position.y < bottomEdge)
        {
            Destroy(gameObject);
        }
    }

    public static void IncreaseMassFactor()
    {
        massIncreaseFactor += 0.1f;
    }

    public static void ResetMassFactor()
    {
        massIncreaseFactor = 0.1f;
    }
}
