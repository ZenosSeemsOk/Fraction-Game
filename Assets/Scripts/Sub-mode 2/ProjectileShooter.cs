using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] RectTransform point;
    [SerializeField] GameObject prefab;

    private void Start()
    {
        Instantiate(prefab,point);
    }
}
