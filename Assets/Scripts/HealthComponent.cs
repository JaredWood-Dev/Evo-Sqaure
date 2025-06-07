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

    void Start()
    {
        hitPoints = maxHitPoints;

        _rb = GetComponent<Rigidbody2D>();
        if (!_rb)
            print("No Rigidbody2D component attached");
    }

    public void ChangeHealth(int amount)
    {
        if ((hitPoints += amount) <= 0)
            KillTarget();
        if ((hitPoints += amount) >= maxHitPoints)
            hitPoints = maxHitPoints;
        else
            hitPoints += amount;
    }

    public void HitTarget(int amount, Vector2 force)
    {
        ChangeHealth(-amount);
        _rb.AddForce(force * (1 - knockbackResistance), ForceMode2D.Impulse);
    }

    public void HealTarget(int amount)
    {
        ChangeHealth(amount);
    }

    public void KillTarget()
    {
        if (gameObject.GetComponent<PlayerController>())
            gameObject.GetComponent<PlayerController>().PlayerDeath();
        else
            Destroy(gameObject);
    }
}
