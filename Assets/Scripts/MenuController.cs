using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private UIController controller;

    public void LoadGame()
    {
        SceneManager.LoadScene("battle_map");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Continue()
    {
        Time.timeScale = 1;
        controller.HidePause();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("main_menu");
    }
}
