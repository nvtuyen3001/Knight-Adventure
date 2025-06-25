using UnityEngine;
using UnityEngine.SceneManagement;

// Attach this script to a GameObject in the Continue scene
public class ContinueSceneInitializer : MonoBehaviour
{
    // This will run as soon as the scene loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnSceneLoaded()
    {
        // Only run this in the Continue scene
        if (SceneManager.GetActiveScene().name == "Continue")
        {
            Debug.Log("Continue scene loaded - hiding player and destroying persistent objects");
            DestroyPersistentObjects();
        }
    }
    
    private void Awake()
    {
        // Immediate check in Awake
        DestroyPersistentObjects();
    }
    
    private void Start()
    {
        // Secondary check in Start
        DestroyPersistentObjects();
        
        // Schedule additional checks to ensure everything is destroyed
        Invoke("DestroyPersistentObjects", 0.1f);
        Invoke("DestroyPersistentObjects", 0.5f);
    }
    
    private static void DestroyPersistentObjects()
    {
        // Find and destroy DontDestroyOnLoad object
        GameObject dontDestroyObj = GameObject.Find("DontDestroyOnLoad");
        if (dontDestroyObj != null)
        {
            Debug.Log("ContinueSceneInitializer: Found DontDestroyOnLoad object, destroying it");
            Destroy(dontDestroyObj);
        }
        
        // Find and destroy any player that might have persisted
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Debug.Log("ContinueSceneInitializer: Found player, destroying it");
            Destroy(player.gameObject);
        }
        
        // Find and destroy any player health that might have persisted
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("ContinueSceneInitializer: Found player health, destroying it");
            Destroy(playerHealth.gameObject);
        }
        
        // Find and destroy any camera controller that might have persisted
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            Debug.Log("ContinueSceneInitializer: Found camera controller, destroying it");
            Destroy(cameraController.gameObject);
        }
        
        // Find and disable any UICanvas that might persist
        GameObject uiCanvas = GameObject.Find("UICanvas");
        if (uiCanvas != null)
        {
            Debug.Log("ContinueSceneInitializer: Found UICanvas, destroying it");
            Destroy(uiCanvas);
        }
        
        // Additional check for any persistent objects
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            // Check if the object is player related
            if (obj.name.Contains("Player") || 
                obj.tag == "Player" || 
                obj.GetComponent<PlayerController>() != null)
            {
                Debug.Log($"ContinueSceneInitializer: Found player object: {obj.name}, destroying it");
                Destroy(obj);
            }
            
            // Check if the object has DontDestroyOnLoad in its name or tag
            if (obj.name.Contains("DontDestroy") || 
                (obj.tag != null && obj.tag.Contains("DontDestroy")))
            {
                Debug.Log($"ContinueSceneInitializer: Found potential DontDestroyOnLoad object: {obj.name}, destroying it");
                Destroy(obj);
            }
        }
    }
} 