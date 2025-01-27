using UnityEngine;

public class SubModeGameManager : MonoBehaviour
{
    [SerializeField] private Aircraft aircraft;
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    private void Update()
    {
        if(aircraft != null)
        {
            if(aircraft.airCraftHit)
            {
                gm.totalLevelUnlocked += 1;
                Debug.Log("Game Over");
            }
        }
    }
}
