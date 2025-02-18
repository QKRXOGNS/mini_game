using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public float spawnRate = 2f;
    public float minHeight = -1f;
    public float maxHeight = 3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPipe), 0f, spawnRate);
    }

    void SpawnPipe()
    {
        float randomY = Random.Range(minHeight, maxHeight);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0);
        Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
    }
}
