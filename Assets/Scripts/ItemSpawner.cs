using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public float spawnRate = 1f;
    private float maxXPos;
    public float initialSpawnRate = 1f;
    public float spawnRateDecreaseFactor = 0.05f;
    public float minimumSpawnRate = 0.1f;

    private float nextSpawnTime;

    private void Awake()
    {
        maxXPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        spawnRate = initialSpawnRate;
        nextSpawnTime = Time.time + spawnRate;
    }

    private void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnItem();
            nextSpawnTime = Time.time + spawnRate;
            spawnRate = Mathf.Clamp(spawnRate - spawnRateDecreaseFactor, minimumSpawnRate, float.MaxValue);
        }
    }

    public GameBounds gameBounds;

    private void SpawnItem()
    {
        if(gameBounds == null)
        {
            Debug.LogError("no bounds set");
        }


        float randomX = Random.Range(-maxXPos, maxXPos);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);

        int randomIndex = Random.Range(0, itemPrefabs.Length);
        GameObject item = Instantiate(itemPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        // Adjust scale or other properties of the item if required
    }
}
