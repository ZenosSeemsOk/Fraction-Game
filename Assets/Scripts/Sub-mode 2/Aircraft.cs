using UnityEngine;
using UnityEngine.UI;

public class Aircraft : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public GameObject m_Aircraft;
    public bool airCraftHit;
    [SerializeField] private Rigidbody2D rb;
    private SubModeGameManager subGm;

    private void Start()
    {
        subGm = SubModeGameManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            if(rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            Debug.Log("anim should be triggered");
            m_Aircraft.SetActive(true);
            airCraftHit = true;
            subGm.checkGameOver = true;
            Destroy(gameObject, 0.75f);
        }
    }
}
