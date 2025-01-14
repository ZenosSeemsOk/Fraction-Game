using UnityEngine;
using UnityEngine.UI;

public class Aircraft : MonoBehaviour
{
    public Image m_Aircraft;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Projectile")
        {
            m_Aircraft.enabled = true;
        }
    }
}
