using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueMenu : MonoBehaviour
{
    [SerializeField] private string defaultSceneName = "Scene1"; // Default scene if no last scene is saved
    
    private void Start()
    {
        // Make sure no player objects exist in the Continue scene
        DestroyPlayerObjects();
    }
    
    public void ContinueGame()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Get the last active scene name
        string lastScene = PlayerPrefs.GetString("LastActiveScene", defaultSceneName);
        Debug.Log($"Continuing game from scene: {lastScene}");
        
        // Load the last active scene
        SceneManager.LoadScene(lastScene);
    }
    
    public void ExitToMainMenu()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Clear any saved game state
        PlayerPrefs.DeleteKey("LastActiveScene");
        PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.DeleteKey("PlayerPosZ");
        PlayerPrefs.DeleteKey("PlayerHealth");
        PlayerPrefs.Save();
        
        // Load the main menu scene
        SceneManager.LoadScene("Win"); // Using Win as the main menu scene
    }
    
    public void ExitGame()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    private void DestroyPlayerObjects()
    {
        // Find and destroy any player that might have persisted
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Debug.Log("ContinueMenu: Found player, destroying it");
            Destroy(player.gameObject);
        }
        
        // Find and destroy any player health that might have persisted
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("ContinueMenu: Found player health, destroying it");
            Destroy(playerHealth.gameObject);
        }
        
        // Additional check for any player related objects
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Player") || 
                obj.tag == "Player" || 
                obj.GetComponent<PlayerController>() != null)
            {
                Debug.Log($"ContinueMenu: Found player object: {obj.name}, destroying it");
                Destroy(obj);
            }
        }
    }
} 