using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSpecificScene : MonoBehaviour
{
    // This function will load the scene at index 1 in your Build Settings
    public void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }

    // Alternative: Load by the actual name of the scene
    public void LoadSceneByName()
    {
        // Replace "Level1" with the exact name of your scene file
        SceneManager.LoadScene("Level1");
    }
}