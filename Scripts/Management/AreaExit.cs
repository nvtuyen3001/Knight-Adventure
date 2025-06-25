using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoadTime = 1f;
    private bool isRestrictedScene = false;
    private Collider2D exitCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Get components
        exitCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Get the current scene name
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Check if we're in one of the restricted scenes
        isRestrictedScene = (currentScene == "Scene1" || currentScene == "Scene2" || currentScene == "Scene3");
        
        // Keep the GameObject active but disable visible/interactive components
        if (isRestrictedScene)
        {
            DisableExit();
        }
    }
    
    private void Update()
    {
        // Only check for enemies in restricted scenes
        if (isRestrictedScene)
        {
            CheckEnemies();
        }
    }
    
    private void CheckEnemies()
    {
        // Find all enemies in the scene
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        
        // Enable/disable exit based on enemy count
        if (enemies.Length == 0)
        {
            EnableExit();
        }
        else
        {
            DisableExit();
        }
    }
    
    private void EnableExit()
    {
        // Enable visible and interactive components
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        
        if (exitCollider != null)
        {
            exitCollider.enabled = true;
        }
    }
    
    private void DisableExit()
    {
        // Disable visible and interactive components
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        
        if (exitCollider != null)
        {
            exitCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine() {
        float timeRemaining = waitToLoadTime; // Use a local copy to prevent modifications
        
        while (timeRemaining >= 0) 
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
