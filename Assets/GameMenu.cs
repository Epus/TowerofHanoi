using UnityEngine.SceneManagement;
using UnityEngine;

public class GameMenu : MonoBehaviour {

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
