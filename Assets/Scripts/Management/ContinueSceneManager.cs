using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueSceneManager : MonoBehaviour
{
    private void Start()
    {
        // Find and destroy DontDestroyOnLoad object
        GameObject dontDestroyObj = GameObject.Find("DontDestroyOnLoad");
        if (dontDestroyObj != null)
        {
            Debug.Log("Found DontDestroyOnLoad object in Continue scene, destroying it");
            Destroy(dontDestroyObj);
        }
        
        // Find and destroy any player that might have persisted
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Debug.Log("Found player in Continue scene, destroying it");
            Destroy(player.gameObject);
        }
        
        // Find and destroy any player health that might have persisted
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("Found player health in Continue scene, destroying it");
            Destroy(playerHealth.gameObject);
        }
        
        // Find and destroy any camera controller that might have persisted
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            Debug.Log("Found camera controller in Continue scene, destroying it");
            Destroy(cameraController.gameObject);
        }
        
        // Find and disable any UICanvas that might persist
        GameObject uiCanvas = GameObject.Find("UICanvas");
        if (uiCanvas != null)
        {
            Debug.Log("Found UICanvas in Continue scene, destroying it");
            Destroy(uiCanvas);
        }
        
        // Additional check for any persistent objects
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            // Check if the object has DontDestroyOnLoad in its name or tag
            if (obj.name.Contains("DontDestroy") || (obj.tag != null && obj.tag.Contains("DontDestroy")))
            {
                Debug.Log($"Found potential DontDestroyOnLoad object: {obj.name}, destroying it");
                Destroy(obj);
            }
        }
    }
} 