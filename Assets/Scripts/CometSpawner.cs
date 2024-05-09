using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public GameObject[] cometPrefabs;
    public float spawnRate = 1f;
    private float maxXPos;
    public float initialSpawnRate = 1f;
    public float spawnRateDecreaseFactor = 0.05f;
    public float minimumSpawnRate = 0.1f;
    public float cometScaleFactor = 0.7f;

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
            SpawnComet();
            nextSpawnTime = Time.time + spawnRate;
            spawnRate = Mathf.Clamp(spawnRate - spawnRateDecreaseFactor, minimumSpawnRate, float.MaxValue);
        }
    }

    public GameBounds gameBounds;

    private void SpawnComet()
    {
        if(gameBounds == null)
        {
            Debug.LogError("no bounds set");
        }

        float randomX = Random.Range(-maxXPos, maxXPos);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);

        int randomIndex = Random.Range(0, cometPrefabs.Length);
        GameObject comet = Instantiate(cometPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        comet.transform.localScale *= cometScaleFactor;
    }

    public void ResetSpawnRate()
    {
        spawnRate = initialSpawnRate;
        nextSpawnTime = Time.time + 2.5f;
    }

}
