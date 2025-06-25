using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Scene1"; // Name of your game scene
    
    // List of menu-type scenes where player should be hidden
    private readonly string[] menuScenes = { "Win", "Die", "Continue" };

    private void Awake()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Add appropriate scene manager based on the current scene
        switch (currentScene)
        {
            case "Win":
                if (FindObjectOfType<WinSceneManager>() == null)
                {
                    GameObject manager = new GameObject("WinSceneManager");
                    manager.AddComponent<WinSceneManager>();
                }
                break;
                
            case "Die":
                if (FindObjectOfType<DieSceneManager>() == null)
                {
                    GameObject manager = new GameObject("DieSceneManager");
                    manager.AddComponent<DieSceneManager>();
                }
                break;
                
            case "Continue":
                if (FindObjectOfType<ContinueSceneManager>() == null)
                {
                    GameObject manager = new GameObject("ContinueSceneManager");
                    manager.AddComponent<ContinueSceneManager>();
                }
                break;
        }
    }
    
    // Called right after Awake
    private void OnEnable()
    {
        // Double check for Continue scene specifically
        if (SceneManager.GetActiveScene().name == "Continue" && FindObjectOfType<ContinueSceneManager>() == null)
        {
            GameObject manager = new GameObject("ContinueSceneManager");
            manager.AddComponent<ContinueSceneManager>();
        }
    }

    public void StartGame()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        SceneManager.LoadScene(gameSceneName);
    }

    public void ExitGame()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Application.Quit();
    }
}
