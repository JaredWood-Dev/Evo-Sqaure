using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    /**
     * Creatures or objects have a "Health Component", which allows them to take damage or be destroyed.
     */

    [Header("Hit Points")]
    public int hitPoints;
    public int maxHitPoints;

    [Header("Defenses")] 
    [Range(-1,1)]
    public float knockbackResistance;
    
    private Rigidbody2D _rb;
    private PlayerController _player;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<PlayerController>();
        if (!_rb)
            print("No Rigidbody2D component attached");

        if (_player)
        {
            maxHitPoints = (int)PlayerStats.Instance.constitution;
        }
        hitPoints = maxHitPoints;
    }

    public bool ChangeHealth(int amount)
    {
        if ((hitPoints += amount) < 1)
        {
            KillTarget();
            return false;
        }
        if ((hitPoints += amount) >= maxHitPoints)
            hitPoints = maxHitPoints;
        else
            hitPoints += amount;
        return true;
    }

    public void HitTarget(int amount, Vector2 force)
    {
        //If the target that was hit was the player, increase constitution XP
        if (_player && ChangeHealth(-amount))
            _player.constitutionXP += amount * _player.constitutionXPRate;
        _rb.AddForce(force * (1 - knockbackResistance), ForceMode2D.Impulse);
    }

    public void HealTarget(int amount)
    {
        ChangeHealth(amount);
    }

    public void KillTarget()
    {
        if (_player)
            _player.PlayerDeath();
        else
            Destroy(gameObject);
    }
}
