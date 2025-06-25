using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    
    // Win scene name constant
    private const string WIN_SCENE = "Win";
    
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
        string currentScene = SceneManager.GetActiveScene().name;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            // Play scene transition sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySceneTransition();
            }
            
            // Check if we're in Scene3 and all enemies are defeated
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "Scene3" && FindObjectsOfType<EnemyAI>().Length == 0)
            {
                // Load Win scene when player enters the exit in Scene3 after defeating all enemies
                UIFade.Instance.FadeToBlack();
                StartCoroutine(LoadWinSceneRoutine());
            }
            else
            {
                // Normal scene transition for other cases
                SceneManagement.Instance.SetTransitionName(sceneTransitionName);
                UIFade.Instance.FadeToBlack();
                StartCoroutine(LoadSceneRoutine());
            }
        }
    }

    // Special routine for loading the Win scene
    private IEnumerator LoadWinSceneRoutine()
    {
        Debug.Log("Loading Win scene...");
        
        // Make sure to destroy everything that might persist between scenes
        GameObject dontDestroyObj = GameObject.Find("DontDestroyOnLoad");
        if (dontDestroyObj != null)
        {
            Debug.Log("Found DontDestroyOnLoad object, destroying it");
            Destroy(dontDestroyObj);
        }
        
        // Find and disable any UICanvas that might persist
        GameObject uiCanvas = GameObject.Find("UICanvas");
        if (uiCanvas != null)
        {
            Debug.Log("Found UICanvas, destroying it");
            Destroy(uiCanvas);
        }
        
        // Clean up any other persistent systems
        if (SceneManagement.Instance != null)
        {
            SceneManagement.Instance.SetTransitionName("");
        }
        
        float timeRemaining = waitToLoadTime;
        while (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        
        // Load Win scene directly
        SceneManager.LoadScene(WIN_SCENE, LoadSceneMode.Single);
    }

    private IEnumerator LoadSceneRoutine()
    {
        float timeRemaining = waitToLoadTime;

        while (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
