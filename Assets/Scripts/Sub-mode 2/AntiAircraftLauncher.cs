using UnityEngine;

public class AntiAircraftLauncher : MonoBehaviour
{
    [SerializeField] private GameObject Projectile;
    [SerializeField] private GameObject trajectory;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Camera cam;
    private float currenttime;

    // Define the rotation limits (in degrees)
    [SerializeField] private float minRotation = -45f;
    [SerializeField] private float maxRotation = 45f;

    // Variables to track rotation and interaction
    private bool isHolding = false;
    private bool isSelected = false;

    void Update()
    {
        // Detect click on the launcher
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isSelected = true;
                isHolding = true;

                trajectory.SetActive(true); // Show trajectory when aiming
            }
        }

        // Rotate the launcher while holding the mouse button
        if (isHolding)
        {
            RotateLauncher();
        }

        // Release logic
        if (Input.GetMouseButtonUp(0))
        {
            if (isSelected && isHolding)
            {
                Shoot();
            }

            isSelected = false;
            isHolding = false;
            trajectory.SetActive(false); // Hide trajectory after releasing the mouse
        }
    }

    void RotateLauncher()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // To Ensure 2D rotation

        // Calculate the direction opposite to the mouse position
        Vector3 direction = transform.position - mousePosition; // Inverted direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Clamp the angle within the specified range
        angle = Mathf.Clamp(angle, minRotation, maxRotation);

        // Rotate the launcher
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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

            // Destroy the bullet after 2 seconds
            if (bullet != null)
            {
                Destroy(bullet, 2);
            }
        }
    }
}
