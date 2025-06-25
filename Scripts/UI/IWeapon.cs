using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Interface defining the contract for all weapon implementations
    /// </summary>
    public interface IWeapon
    {
        /// <summary>
        /// Executes the weapon's attack behavior
        /// </summary>
        void Attack();
        
        /// <summary>
        /// Gets the weapon's configuration and statistics
        /// </summary>
        /// <returns>WeaponInfo containing weapon data</returns>
        WeaponInfo GetWeaponInfo();
    }
}
