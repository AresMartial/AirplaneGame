using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public void PlayPressed()
    {
        LoadNextScene();
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

    public static void LoadNextScene()
    {
        int nextSceneId;
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneId = 0;
        }
        else
        {
            nextSceneId = SceneManager.GetActiveScene().buildIndex + 1;            
        }
        SceneManager.LoadScene(nextSceneId);
    }
}
