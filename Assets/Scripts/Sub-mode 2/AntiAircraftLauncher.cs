using UnityEngine;

public class AntiAircraftLauncher : MonoBehaviour
{
    [SerializeField] private GameObject Projectile;
    [SerializeField] private GameObject trajectory;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private RectTransform newShootPoint;
    private Camera cam;
    private float currenttime;

    // Define the rotation limits (in degrees)
    [SerializeField] private float minRotation = -45f;
    [SerializeField] private float maxRotation = 45f;

    // Rotation sensitivity (speed of rotation)
    [SerializeField] private float rotationSensitivity = 5f;

    // Variables to track rotation and interaction
    public bool isHolding = false;
    private bool isSelected = false;

    private void Start()
    {
        cam = Camera.main;  // Assign the main camera automatically
        isHolding = false;
    }

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
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Clamp the angle within the specified range
        targetAngle = Mathf.Clamp(targetAngle, minRotation, maxRotation);

        // Smoothly rotate the launcher using sensitivity
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSensitivity * Time.deltaTime
        );
    }

    void Shoot()
    {
        // Ensure cooldown period before shooting again
        if (Time.time > (currenttime + 0.5f))
        {
            Debug.Log("Projectile shot");
            // Instantiate the projectile with the shoot point's rotation
            //GameObject bullet = Instantiate(Projectile, shootPoint.position, shootPoint.rotation);
            GameObject bullet = Instantiate(Projectile, newShootPoint);

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