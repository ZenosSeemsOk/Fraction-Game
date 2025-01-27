
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float Speed;

    private Vector3 Direction;

    // This method is called when the projectile is instantiated
    public void Initialize(Vector3 direction)
    {
        // Set the direction to move in based on the shoot point's rotation
        Direction = direction;
    }

    private void Update()
    {
        // Move the projectile in the direction it's facing
        transform.position += Direction * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
