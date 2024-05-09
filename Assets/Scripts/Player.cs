using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    private int originalSpriteOrder;


    public GameObject shieldIconPrefab;
    public GameObject timeFreezeIconPrefab;
    public GameObject magnetIconPrefab;
    private GameObject currentSkillIcon;
    private float defaultMass = 1.0f;  // You may need to adjust this based on your game's requirements
    private bool isShielded = false;
    private float shieldDuration = 5f;
    private float timeFreezeDuration = 3f;
    private Vector2 originalGravity;
    public float timeFreezeGravityMultiplier = 0.5f;  // Adjust this value as needed
    public Vector2 timeFreezeGravity = new Vector2(0f, -4.9f);  // Adjust this value as needed
    private float magnetDuration = 5f;
    private float magnetPullForce = 15f;
    public float hoverHeight = -3.5f;
    public float moveSpeed = 5f;
    private float leftBound;
    private float rightBound;
    public int lives = 3;
    public event System.Action<int> OnLivesChanged;
    private bool isSkillActive = false;  // Flag to track if a skill is active
    private List<GameObject> affectedObjects = new List<GameObject>();
    public Slider tiltSensitivitySlider;
    private float tiltSensitivity;
    private float tiltInput;


    private void ClearCurrentSkillIcon()
    {
        if (currentSkillIcon)
        {
            Destroy(currentSkillIcon);
            currentSkillIcon = null;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSpriteOrder = spriteRenderer.sortingOrder;

        leftBound = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        rightBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

#if UNITY_IOS || UNITY_ANDROID
            float mobileScaleFactor = 1f; // Adjust the scale for mobile
            transform.localScale *= mobileScaleFactor;

            // Camera.main.orthographicSize *= 1f; // Zoom out the camera for a wider view on mobile
            rightBound *= 1f; // Adjust the right boundary to be tighter on mobile
#endif
    }

    private Vector3 startingPosition = new Vector3(0, -4f, 0);

    private void Start()
    {
        if (PlayerPrefs.HasKey("TiltSensitivity"))
        {
            tiltSensitivity = PlayerPrefs.GetFloat("TiltSensitivity");
        }
        else
        {
            tiltSensitivity = 1.5f; // or any default value you want
        }

        originalGravity = Physics2D.gravity;
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
        transform.position = startingPosition;  // Set starting position when the game starts
    }

    private void OnEnable()
    {
        //transform.position = startingPosition;  // Ensures that whenever the player object is enabled, it starts at the designated position
    }

    private void Update()
    {
        tiltInput = Input.acceleration.x * tiltSensitivity;

        Vector3 position = transform.position;
        // Combine inputs from keyboard and accelerometer
        float keyboardInput = Input.GetAxis("Horizontal");
        float horizontalInput = keyboardInput + tiltInput;

        position.x += horizontalInput * moveSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, leftBound, rightBound);
        position.y = startingPosition.y;  // Always keep the y position consistent with startingPosition's y value

        transform.position = position;
    }

    Vector3 GetRawTilt()
    {
        // Here, retrieve the raw tilt input from the device.
        // For the purpose of this example, we'll just return a dummy value.
        return new Vector3(1, 0, 0);
    }


    public void SaveTiltSensitivity()
    {
        float sensitivity = tiltSensitivitySlider.value;
        PlayerPrefs.SetFloat("TiltSensitivity", sensitivity);
        tiltSensitivity = tiltSensitivitySlider.value;
        PlayerPrefs.Save();
    }

    private void ActivateShield()
    {
        if (!isShielded)
        {
            isSkillActive = true;  // Set skill active flag
            isShielded = true;
            spriteRenderer.sortingOrder = originalSpriteOrder + 1;
            Vector3 shieldOffset = Vector3.zero;
            currentSkillIcon = Instantiate(shieldIconPrefab, transform.position + shieldOffset, Quaternion.identity, transform);
            currentSkillIcon.transform.localScale = new Vector3(5f, 5f, 1f);
            StartCoroutine(ShieldCooldown());
        }
    }

    private IEnumerator ShieldCooldown()
    {
        yield return new WaitForSeconds(shieldDuration);
        isShielded = false;
        spriteRenderer.sprite = sprites[spriteIndex];
        spriteRenderer.sortingOrder = originalSpriteOrder;
        ClearCurrentSkillIcon();
    }

    private void ActivateTimeFreeze()
    {
        isSkillActive = true;
        spriteRenderer.sortingOrder = originalSpriteOrder + 1;
        Vector3 timeFreezeOffset = Vector3.zero;
        currentSkillIcon = Instantiate(timeFreezeIconPrefab, transform.position + timeFreezeOffset, Quaternion.identity, transform);
        currentSkillIcon.transform.localScale = new Vector3(2f, 2f, 1f);

        // Store the current gravity
        originalGravity = Physics2D.gravity;

        // Halve the gravity for the Time Freeze powerup
        Physics2D.gravity = originalGravity * 0.5f;

        StartCoroutine(TimeFreezeCooldown());
    }


    private IEnumerator TimeFreezeCooldown()
    {
        yield return new WaitForSeconds(timeFreezeDuration);
        // Restore the gravity to its original value
        Physics2D.gravity = originalGravity;

        spriteRenderer.sprite = sprites[spriteIndex];
        spriteRenderer.sortingOrder = originalSpriteOrder;
        ClearCurrentSkillIcon();
    }


    private void ActivateMagnet()
    {
        isSkillActive = true;  // Set skill active flag
        spriteRenderer.sortingOrder = originalSpriteOrder + 1;
        currentSkillIcon = Instantiate(magnetIconPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform);
        currentSkillIcon.transform.localScale = new Vector3(1f, 1f, 1f);
        StartCoroutine(MagnetEffect());
    }

    private IEnumerator MagnetEffect()
    {
        float timer = 0f;
        while (timer < magnetDuration)
        {
            GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");
            foreach (GameObject star in stars)
            {
                Vector3 direction = transform.position - star.transform.position;
                star.transform.position += direction.normalized * magnetPullForce * Time.deltaTime;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.sprite = sprites[spriteIndex];
        spriteRenderer.sortingOrder = originalSpriteOrder;
        ClearCurrentSkillIcon();
    }




    public void RestartPlayer()
    {
        // Cancel any active power-ups
        CancelCurrentSkill();

        // Reset player attributes
        lives = 3;
        transform.position = startingPosition;
        spriteIndex = 0;
        CancelInvoke(nameof(AnimateSprite));
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
        OnLivesChanged?.Invoke(lives);

        // Reset game time (just to be sure)
        Time.timeScale = 1f;

        foreach (GameObject obj in affectedObjects)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.mass = defaultMass;
            }
        }
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle" && !isShielded)
        {
            // Start the flicker coroutine when hit by a comet
            StartCoroutine(FlickerPlayer());

            lives--;
            OnLivesChanged?.Invoke(lives);

            if (lives <= 0)
            {
                FindObjectOfType<GameManager>().GameOver();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.tag == "Scoring")
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
        else if (other.gameObject.tag == "Star")
        {
            FindObjectOfType<GameManager>().IncreaseScoreBy(100); // Or whatever score value you want
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "AlienShip")
        {
            GrantRandomSkill();
            Destroy(other.gameObject);
        }
    }

    private void GrantRandomSkill()
    {
        // If there's an active skill, cancel it
        if (isSkillActive)
        {
            CancelCurrentSkill();
        }

        int randomSkill = Random.Range(0, 3);

        switch (randomSkill)
        {
            case 0:
                ActivateShield();
                break;
            case 1:
                ActivateTimeFreeze();
                break;
            case 2:
                ActivateMagnet();
                break;
        }
    }

    private void CancelCurrentSkill()
    {
        // Cancel any active coroutines to prevent multiple skills running simultaneously
        StopAllCoroutines();

        // Reset time scale just in case the time freeze was active
        Time.timeScale = 1f;

        // Destroy the skill icon if it exists
        ClearCurrentSkillIcon();

        // Set the sprite back to normal
        spriteRenderer.sprite = sprites[spriteIndex];
        spriteRenderer.sortingOrder = originalSpriteOrder;

        // Reset shield flag if shield was active
        isShielded = false;

        // Reset skill active flag
        isSkillActive = false;
        Physics2D.gravity = originalGravity;
    }

    private IEnumerator FlickerPlayer()
    {
        float flickerDuration = 0.5f; // Adjust the flicker duration as needed
        float flickerInterval = 0.1f; // Adjust the flicker interval as needed

        isShielded = true; // Player is temporarily shielded during flicker

        for (float time = 0; time < flickerDuration; time += flickerInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle player visibility
            yield return new WaitForSeconds(flickerInterval);
        }

        spriteRenderer.enabled = true; // Ensure the player is visible after flicker
        isShielded = false; // Reset the shield flag

        // Handle any additional logic after the flicker if needed
    }
}