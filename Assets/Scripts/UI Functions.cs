using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    private bool uiEnabled;

    void Start(){
        uiEnabled = false;
        canvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) && Time.timeScale != 0){
            if(uiEnabled == false){
                Pause();
            }
            else{

                Resume();
            }
        }
    }

    public void Pause(){
        canvas.enabled = true;
        uiEnabled = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Resume(){
        canvas.enabled = false;
        uiEnabled = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ExitToMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitToDesktop(){
        Application.Quit();
    }
}
