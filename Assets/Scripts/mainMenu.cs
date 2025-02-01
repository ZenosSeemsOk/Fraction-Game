using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
