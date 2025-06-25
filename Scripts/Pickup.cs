using System.Collections;
using UnityEngine;

namespace Pickups
{
    /// <summary>
    /// Handles pickup item behavior including movement toward player and collection effects
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        #region Enums
        
        private enum PickUpType
        {
            GoldCoin,
            StaminaGlobe,
            HealthGlobe,
        }
        
        #endregion

        #region Serialized Fields
        
        [Header("Pickup Configuration")]
        [SerializeField] private PickUpType pickUpType;
        
        [Header("Movement Settings")]
        [SerializeField] private float pickUpDistance = 5f;
        [SerializeField] private float accelerationRate = 0.2f;
        [SerializeField] private float moveSpeed = 3f;
        
        [Header("Spawn Animation")]
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private float heightY = 1.5f;
        [SerializeField] private float popDuration = 1f;
        
        #endregion

        #region Private Fields
        
        private Vector3 moveDirection;
        private Rigidbody2D rigidBody;
        private bool isInitialized = false;
        
        #endregion

        #region Unity Lifecycle
        
        private void Awake() 
        {
            rigidBody = GetComponent<Rigidbody2D>();
            ValidateComponents();
        }

        private void Start() 
        {
            if (isInitialized)
            {
                StartCoroutine(SpawnAnimationRoutine());
            }
        }

        private void Update() 
        {
            if (!isInitialized || PlayerController.Instance == null) return;
            
            HandlePlayerDetection();
        }

        private void FixedUpdate() 
        {
            if (isInitialized)
            {
                rigidBody.velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
            }
        }

        private void OnTriggerStay2D(Collider2D other) 
        {
            if (other.gameObject.GetComponent<PlayerController>()) 
            {
                ProcessPickup();
                Destroy(gameObject);
            }
        }
        
        #endregion

        #region Private Methods
        
        /// <summary>
        /// Validates required components are present
        /// </summary>
        private void ValidateComponents()
        {
            if (rigidBody == null)
            {
                Debug.LogError($"Pickup '{gameObject.name}' requires a Rigidbody2D component!");
                return;
            }
            
            if (animationCurve == null)
            {
                Debug.LogWarning($"Pickup '{gameObject.name}' has no animation curve assigned, using default linear curve.");
                animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
            }
            
            isInitialized = true;
        }
        
        /// <summary>
        /// Handles detection and movement toward player
        /// </summary>
        private void HandlePlayerDetection()
        {
            Vector3 playerPosition = PlayerController.Instance.transform.position;
            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

            if (distanceToPlayer < pickUpDistance) 
            {
                moveDirection = (playerPosition - transform.position).normalized;
                moveSpeed += accelerationRate * Time.deltaTime;
            } 
            else 
            {
                moveDirection = Vector3.zero;
                moveSpeed = Mathf.Max(0, moveSpeed - accelerationRate * Time.deltaTime);
            }
        }

        /// <summary>
        /// Handles the spawn animation with random trajectory
        /// </summary>
        /// <returns>Coroutine for spawn animation</returns>
        private IEnumerator SpawnAnimationRoutine() 
        {
            Vector2 startPoint = transform.position;
            
            // Generate random end point within reasonable range
            float randomX = transform.position.x + Random.Range(-2f, 2f);
            float randomY = transform.position.y + Random.Range(-1f, 1f);
            Vector2 endPoint = new Vector2(randomX, randomY);

            float elapsedTime = 0f;

            while (elapsedTime < popDuration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / popDuration;
                float heightMultiplier = animationCurve.Evaluate(normalizedTime);
                float currentHeight = Mathf.Lerp(0f, heightY, heightMultiplier);

                transform.position = Vector2.Lerp(startPoint, endPoint, normalizedTime) + new Vector2(0f, currentHeight);
                yield return null;
            }
            
            // Ensure final position is set
            transform.position = endPoint;
        }

        /// <summary>
        /// Processes the pickup effect based on type
        /// </summary>
        private void ProcessPickup() 
        {
            switch (pickUpType)
            {
                case PickUpType.GoldCoin:
                    if (EconomyManager.Instance != null)
                    {
                        EconomyManager.Instance.UpdateCurrentGold();
                    }
                    else
                    {
                        Debug.LogWarning("EconomyManager instance not found!");
                    }
                    break;
                    
                case PickUpType.HealthGlobe:
                    if (PlayerHealth.Instance != null)
                    {
                        PlayerHealth.Instance.HealPlayer();
                    }
                    else
                    {
                        Debug.LogWarning("PlayerHealth instance not found!");
                    }
                    break;
                    
                case PickUpType.StaminaGlobe:
                    if (Stamina.Instance != null)
                    {
                        Stamina.Instance.RefreshStamina();
                    }
                    else
                    {
                        Debug.LogWarning("Stamina instance not found!");
                    }
                    break;
                    
                default:
                    Debug.LogError($"Unknown pickup type: {pickUpType}");
                    break;
            }
        }
        
        #endregion
    }
}
