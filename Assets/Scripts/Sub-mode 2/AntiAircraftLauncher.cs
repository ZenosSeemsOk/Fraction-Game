using UnityEngine;

public class AntiAircraftLauncher : MonoBehaviour
{
    [SerializeField] private AudioSource m_source;
    [SerializeField] private AudioSource m_source2;
    [SerializeField] private GameObject Projectile;
    [SerializeField] private GameObject trajectory;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private RectTransform newShootPoint;
    private SubModeGameManager gameManager;
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

    // Reference to pause manager (or you can use your own game manager logic)
    private bool isPaused = false;

    private void Start()
    {
        cam = Camera.main;  // Assign the main camera automatically
        isHolding = false;
        // Optionally: Subscribe to an event or method to update the pause state
        // Example: GameManager.instance.OnPauseChanged += OnPauseChanged;
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        // Detect click on the launcher
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                m_source.Play(); // Play sound only when interacting with the launcher
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
            m_source2.Play(); // Play only if game is not paused
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

    // Update this method when the pause state changes
    public void SetPauseState(bool paused)
    {
        isPaused = paused;
    }
}
