using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private float destroyX = -10f;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
