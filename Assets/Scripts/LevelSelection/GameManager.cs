using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalLevelUnlocked = 1;
    public int levelindex;
    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
