using System;
using System.Collections;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public GameObject textPrefab;

    [Header("Stunning")] 
    public bool isStunned = false;
    public float stunDuration = 0.1f;
    private float _stunTimer = 0.0f;
    
    private Rigidbody2D _rb;
    private PlayerController _player;
    private AudioSource _as;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<PlayerController>();
        _as = GetComponent<AudioSource>();
        if (!_rb)
            print("No Rigidbody2D component attached");

        if (_player)
        {
            maxHitPoints = (int)PlayerStats.Instance.constitution;
        }
        hitPoints = maxHitPoints;

        textPrefab = GameObject.Find("Damage Amount Holder");
    }

    public bool ChangeHealth(int amount)
    {
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
        _as.pitch = _as.pitch = Random.Range(0.9f, 1.1f);
        _as.Play();
        
        //If the target that was hit was the player, increase constitution XP
        if (ChangeHealth(-amount))
        {
            if (_player)
            {
                _player.constitutionXP += amount * _player.constitutionXPRate;
            }
        }
        
        if (hurtEffect)
            Instantiate(hurtEffect, transform.position, Quaternion.identity);
        
        if (_player)
            SpawnNumber(amount, Color.red);
        else if (GameObject.Find("Player").GetComponent<PlayerController>().activeWeapon.weaponStat == Stat.Magic)
            SpawnNumber(amount, Color.blue);
        else
            SpawnNumber(amount, Color.white);
        
        isStunned = true;
        _rb.linearVelocity = Vector2.zero;
        _stunTimer = stunDuration;
        
        _rb.AddForce(force * (1 - knockbackResistance), ForceMode2D.Impulse);
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

    public void SpawnNumber(int num, Color color)
    {
        var text = Instantiate(textPrefab, transform.position, Quaternion.identity);
        text.transform.GetChild(0).GetComponent<TextMeshPro>().text = num.ToString();
        text.transform.GetChild(0).GetComponent<TextMeshPro>().color = color;
        text.GetComponent<DestroyAfterTime>().enabled = true;

        StartCoroutine(DeleteNumber(text));
    }

    IEnumerator DeleteNumber(GameObject number)
    {
        yield return new WaitForSeconds(1);
        
        Destroy(number);
    }

    void Update()
    {
        if (_stunTimer < 0)
            isStunned = false;
        
        _stunTimer -= Time.fixedDeltaTime;
    }
}
