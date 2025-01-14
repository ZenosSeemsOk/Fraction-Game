using UnityEngine;

public class AntiAircraftLauncher : MonoBehaviour
{
    [SerializeField] private GameObject Projectile;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Camera cam;
    private float currenttime;
    private Vector3 mousePos;

    // Define the rotation limits (in degrees)
    [SerializeField] private float minRotation = -45f;
    [SerializeField] private float maxRotation = 45f;

    void Update()
    {
        // Get mouse position in world space
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure the z-coordinate is 0 for 2D rotation

        // Calculate the direction to look at
        Vector3 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        // Clamp the angle within the specified range
        angle = Mathf.Clamp(angle, minRotation, maxRotation);

        // Rotate the object towards the mouse position but within the clamped range
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Check if space is pressed and then shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Ensure cooldown period before shooting again
        if (Time.time > (currenttime + 0.5f))
        {
            // Instantiate the projectile with the shoot point's rotation
            GameObject bullet = Instantiate(Projectile, shootPoint.position, shootPoint.rotation);

            // Initialize the bullet's direction to match the cannon's rotation
            bullet.GetComponent<Projectile>().Initialize(shootPoint.up);

            // Update the current time to enforce cooldown
            currenttime = Time.time;

            // Destroy the bullet after 3 seconds
            if (bullet != null)
            {
                Destroy(bullet, 2);
            }
        }
    }
}
