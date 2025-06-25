using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }
    public int CurrentHealth { get { return currentHealth; } }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string DIE_SCENE = "Die";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake() {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        isDead = false;
        currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy) {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer() {
        if (currentHealth < maxHealth) {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    // Method to set player health (used when continuing from Continue scene)
    public void SetHealth(int health) {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthSlider();
        
        // Check if player should be dead
        if (currentHealth <= 0) {
            CheckIfPlayerDeath();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform) {
        if (!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        
        // Play hurt sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerHurt();
        }
        
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath() {
        if (currentHealth <= 0 && !isDead) {
            isDead = true;
            if (ActiveWeapon.Instance != null) {
                Destroy(ActiveWeapon.Instance.gameObject);
            }
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            
            // Play death sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPlayerDeath();
            }
            
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine() {
        yield return new WaitForSeconds(2f);
        
        // Find and destroy DontDestroyOnLoad object before loading Die scene
        GameObject dontDestroyObj = GameObject.Find("DontDestroyOnLoad");
        if (dontDestroyObj != null) {
            Debug.Log("Found DontDestroyOnLoad object before Die scene, destroying it");
            Destroy(dontDestroyObj);
        }
        
        // Find and destroy UICanvas before loading Die scene
        GameObject uiCanvas = GameObject.Find("UICanvas");
        if (uiCanvas != null) {
            Debug.Log("Found UICanvas before Die scene, destroying it");
            Destroy(uiCanvas);
        }
        
        // Reset any scene transition data
        if (SceneManagement.Instance != null) {
            SceneManagement.Instance.SetTransitionName("");
        }
        
        Destroy(gameObject);
        SceneManager.LoadScene(DIE_SCENE);
    }

    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider() {
        if (healthSlider == null) {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
