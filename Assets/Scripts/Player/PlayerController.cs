using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }
    

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;
    
    // Scene name for the continue/pause menu
    private const string CONTINUE_SCENE = "Continue";

    protected override void Awake() {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Update() {
        PlayerInput();
        
        // Check for ESC key to show Continue scene
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowContinueScene();
        }
    }

    private void FixedUpdate() {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider() {
        return weaponCollider;
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move() {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRender.flipX = true;
            facingLeft = true;
        } else {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash() {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0) {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            
            // Play dash sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayDash();
            }
            
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine() {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
    
    // Method to show the Continue scene when ESC is pressed
    private void ShowContinueScene()
    {
        // Save current scene name to return to later
        PlayerPrefs.SetString("LastActiveScene", SceneManager.GetActiveScene().name);
        
        // Save player position to return to later
        PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);
        
        // Save player health
        if (PlayerHealth.Instance != null)
        {
            PlayerPrefs.SetInt("PlayerHealth", PlayerHealth.Instance.CurrentHealth);
        }
        
        PlayerPrefs.Save();
        
        // Hide player immediately
        gameObject.SetActive(false);
        
        // Find and destroy DontDestroyOnLoad object before loading Continue scene
        GameObject dontDestroyObj = GameObject.Find("DontDestroyOnLoad");
        if (dontDestroyObj != null)
        {
            Debug.Log("Found DontDestroyOnLoad object before Continue scene, destroying it");
            Destroy(dontDestroyObj);
        }
        
        // Find and destroy UICanvas before loading Continue scene
        GameObject uiCanvas = GameObject.Find("UICanvas");
        if (uiCanvas != null)
        {
            Debug.Log("Found UICanvas before Continue scene, destroying it");
            Destroy(uiCanvas);
        }
        
        // Reset any scene transition data
        if (SceneManagement.Instance != null)
        {
            SceneManagement.Instance.SetTransitionName("");
        }
        
        // Load the Continue scene
        SceneManager.LoadScene(CONTINUE_SCENE);
    }
}
