using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    //0 - MainMenu
    //1 - GameScene(NewCastle)
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;

    }

    public void Settings()
    {

    }
}
