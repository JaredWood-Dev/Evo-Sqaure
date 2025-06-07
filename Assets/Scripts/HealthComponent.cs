using System;
using System.Collections;
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
    
    [Header("Effects")]
    public ParticleSystem deathEffect;
    public ParticleSystem hurtEffect;
    
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
        print("dealing " + amount + "damage");
        if ((hitPoints + amount) < 1)
        {
            KillTarget();
            return false;
        }
        if ((hitPoints + amount) >= maxHitPoints)
            hitPoints = maxHitPoints;
        else
            hitPoints += amount;
        return true;
    }

    public void HitTarget(int amount, Vector2 force)
    {
        print(gameObject + " was hit for " + amount);
        //If the target that was hit was the player, increase constitution XP
        if (ChangeHealth(-amount))
        {
            if (_player)
            {
                _player.constitutionXP += amount * _player.constitutionXPRate;
            }
        }

        _rb.AddForce(force * (1 - knockbackResistance), ForceMode2D.Impulse);
        
        if (hurtEffect)
            Instantiate(hurtEffect, transform.position, Quaternion.identity);
    }

    public void HealTarget(int amount)
    {
        ChangeHealth(amount);
    }

    public void KillTarget()
    {
        if (deathEffect)
        {
            ParticleSystem effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        if (_player)
            _player.PlayerDeath();
        else
            Destroy(gameObject);
    }
}
