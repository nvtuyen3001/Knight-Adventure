using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start() {
        // Make sure player is active in game scenes
        if (PlayerController.Instance != null && !PlayerController.Instance.gameObject.activeSelf)
        {
            PlayerController.Instance.gameObject.SetActive(true);
        }
        
        // Check if we're returning from the Continue scene
        if (PlayerPrefs.HasKey("LastActiveScene") && 
            PlayerPrefs.GetString("LastActiveScene") == SceneManager.GetActiveScene().name)
        {
            // Restore player position
            if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY"))
            {
                float x = PlayerPrefs.GetFloat("PlayerPosX");
                float y = PlayerPrefs.GetFloat("PlayerPosY");
                float z = PlayerPrefs.GetFloat("PlayerPosZ");
                
                PlayerController.Instance.transform.position = new Vector3(x, y, z);
                CameraController.Instance.SetPlayerCameraFollow();
                UIFade.Instance.FadeToClear();
                
                // Restore player health if saved
                if (PlayerPrefs.HasKey("PlayerHealth") && PlayerHealth.Instance != null)
                {
                    PlayerHealth.Instance.SetHealth(PlayerPrefs.GetInt("PlayerHealth"));
                }
                
                // Clear the saved position data to prevent reusing it on next scene load
                PlayerPrefs.DeleteKey("LastActiveScene");
                PlayerPrefs.DeleteKey("PlayerPosX");
                PlayerPrefs.DeleteKey("PlayerPosY");
                PlayerPrefs.DeleteKey("PlayerPosZ");
                PlayerPrefs.DeleteKey("PlayerHealth");
                PlayerPrefs.Save();
            }
        }
        else if (transitionName == SceneManagement.Instance.SceneTransitionName) {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();
        }
    }
}
