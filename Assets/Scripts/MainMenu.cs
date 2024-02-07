using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    /// <summary>
    /// Switches scene to the scene name that is in the parameter
    /// </summary>
    /// <param name="l_sceneName"></The name of the scene the game needs to switch to>
    public void SceneSwitch(string l_sceneName)
    {
        SceneManager.LoadScene(l_sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}