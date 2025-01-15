using UnityEngine;
using UnityEngine.UI;

public class Aircraft : MonoBehaviour
{
    public Image m_Aircraft;
    [SerializeField] private ParticleSystem hit_Particle_sytem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            m_Aircraft.enabled = true;

            // Instantiate a copy of the particle system to avoid modifying the prefab
            ParticleSystem particleInstance = Instantiate(hit_Particle_sytem, transform.position, Quaternion.identity);
            particleInstance.Play();

            // Destroy the instance after 1 second
            Destroy(particleInstance.gameObject, 1f);
        }
    }
}
