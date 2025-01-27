using UnityEngine;
using UnityEngine.UI;

public class Aircraft : MonoBehaviour
{
    public GameObject m_Aircraft;
    public bool airCraftHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            m_Aircraft.SetActive(true);
            airCraftHit = true;
        }
    }
}
