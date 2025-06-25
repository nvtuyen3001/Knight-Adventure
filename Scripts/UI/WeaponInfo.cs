using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// ScriptableObject containing weapon configuration and statistics
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Info")]
    public class WeaponInfo : ScriptableObject
    {
        [Header("Weapon Configuration")]
        [Tooltip("The prefab to instantiate when using this weapon")]
        public GameObject weaponPrefab;
        
        [Header("Combat Stats")]
        [Tooltip("Time between attacks in seconds")]
        [Range(0.1f, 5f)]
        public float weaponCooldown = 1f;
        
        [Tooltip("Damage dealt per attack")]
        [Range(1, 100)]
        public int weaponDamage = 10;
        
        [Tooltip("Maximum effective range of the weapon")]
        [Range(0.5f, 20f)]
        public float weaponRange = 5f;

        #region Validation
        
        /// <summary>
        /// Validates weapon configuration
        /// </summary>
        /// <returns>True if weapon is properly configured</returns>
        public bool IsValid()
        {
            if (weaponPrefab == null)
            {
                Debug.LogError($"Weapon '{name}' has no prefab assigned!");
                return false;
            }
            
            if (weaponCooldown <= 0)
            {
                Debug.LogError($"Weapon '{name}' has invalid cooldown: {weaponCooldown}");
                return false;
            }
            
            if (weaponDamage <= 0)
            {
                Debug.LogError($"Weapon '{name}' has invalid damage: {weaponDamage}");
                return false;
            }
            
            if (weaponRange <= 0)
            {
                Debug.LogError($"Weapon '{name}' has invalid range: {weaponRange}");
                return false;
            }
            
            return true;
        }
        
        #endregion
    }
}
