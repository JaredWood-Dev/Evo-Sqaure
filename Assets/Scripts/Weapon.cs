using UnityEngine;
using Enums;

public abstract class Weapon : MonoBehaviour
{
    /**
     * This script describes the basic 'weapon' which all other weapons will implement.
     */

    public int damage; //The damage the weapon does every instance.
    public float range; //The range of the weapon. For melee, melee range; For ranged, the attack range.
    public float attackSpeed; //The attack speed of the weapon, for persistent weapons it's how often damage ticks.
    public float knockback; //The knock-back power of the weapon.
    public Stat weaponStat; //The Stat the weapon uses for damage.
    public LayerMask targetLayer;

    public abstract void Attack();
}
