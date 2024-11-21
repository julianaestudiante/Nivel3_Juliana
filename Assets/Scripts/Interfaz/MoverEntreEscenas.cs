using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoverEntreEscenas : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        int nextSceneIndex = currentSceneIndex + 1; 

        SceneManager.LoadScene(nextSceneIndex); 
    }

    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        int previousSceneIndex = currentSceneIndex - 1; 

        // If it's the first scene, loop back to the last scene
        if (previousSceneIndex < 0)
        {
            previousSceneIndex = SceneManager.sceneCountInBuildSettings - 1; 
        }

        SceneManager.LoadScene(previousSceneIndex); 
    }
}
