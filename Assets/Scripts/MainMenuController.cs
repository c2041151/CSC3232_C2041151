using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Start(){
        Cursor.lockState = CursorLockMode.None;
    }
    public void LoadLevelOne(){
        SceneManager.LoadScene("Level 1");
    }

    public void ExitGame(){
        Application.Quit();
    }
}
