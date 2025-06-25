using UnityEngine;
using UnityEngine.SceneManagement;

public class DieSceneManager : MonoBehaviour
{
    private void Start()
    {
        // Find and destroy DontDestroyOnLoad object
        GameObject dontDestroyObj = GameObject.Find("DontDestroyOnLoad");
        if (dontDestroyObj != null)
        {
            Debug.Log("Found DontDestroyOnLoad object in Die scene, destroying it");
            Destroy(dontDestroyObj);
        }
        
        // Find and destroy any player that might have persisted
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Debug.Log("Found player in Die scene, destroying it");
            Destroy(player.gameObject);
        }
        
        // Find and destroy any player health that might have persisted
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("Found player health in Die scene, destroying it");
            Destroy(playerHealth.gameObject);
        }
        
        // Find and destroy any camera controller that might have persisted
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            Debug.Log("Found camera controller in Die scene, destroying it");
            Destroy(cameraController.gameObject);
        }
        
        // Find and disable any UICanvas that might persist
        GameObject uiCanvas = GameObject.Find("UICanvas");
        if (uiCanvas != null)
        {
            Debug.Log("Found UICanvas in Die scene, destroying it");
            Destroy(uiCanvas);
        }
    }
} 