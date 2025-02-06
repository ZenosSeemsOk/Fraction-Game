using UnityEngine;
using UnityEngine.UI;

public class Aircraft : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource a_Bird;
    [SerializeField] private AudioSource a_Plane;
    public GameObject m_Aircraft;
    public bool airCraftHit;
    [SerializeField] private Rigidbody2D rb;
    private SubModeGameManager subGm;
    private LevelSelection lv;

    private void Start()
    {
        lv = LevelSelection.Instance;
        subGm = SubModeGameManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            if(lv.checkLevelIndex <= 3 )
            {
                a_Bird.Play();
            }
            else
            {
                a_Plane.Play();
            }
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
