using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneManager : MonoBehaviour
{
    private void Start()
    {
        // Find and destroy any player that might have persisted
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Debug.Log("Found player in Win scene, destroying it");
            Destroy(player.gameObject);
        }
        
        // Find and destroy any player health that might have persisted
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("Found player health in Win scene, destroying it");
            Destroy(playerHealth.gameObject);
        }
        
        // Find and destroy any camera controller that might have persisted
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            Debug.Log("Found camera controller in Win scene, destroying it");
            Destroy(cameraController.gameObject);
        }
    }
} 