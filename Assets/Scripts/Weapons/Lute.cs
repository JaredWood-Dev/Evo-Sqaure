using System.Collections;
using UnityEngine;

public class Lute : Weapon
{

    public ParticleSystem lightningEffect;
    private ParticleSystem _lightningSystem;
    public float spellDuration = 4f;
    private float _spellTimer = 0f;
    public override void Attack()
    {
        StartCoroutine(TickDamage());
        _lightningSystem = Instantiate(lightningEffect, transform.position, Quaternion.identity);
        _lightningSystem.transform.rotation = transform.localRotation;
        
        _spellTimer = spellDuration;
    }

    IEnumerator TickDamage()
    {
        yield return new WaitForSeconds(attackSpeed);
        
        GetComponent<Animator>().SetBool("isCasting", true);
        
        _lightningSystem.transform.rotation = transform.parent.parent.rotation;
        _lightningSystem.transform.position = transform.position + (transform.parent.parent.right * (range / 2));
        
        
        Vector2 origin = transform.position;
        Vector2 size = new Vector2(1, 1);
        float angle = transform.rotation.eulerAngles.z;
        Vector2 direction = gameObject.transform.parent.transform.parent.transform.right;
        
        var targets = Physics2D.BoxCastAll(origin, size, angle, direction, range, targetLayer);

        foreach (var target in targets)
        {
            HealthComponent targetHealth = target.collider.GetComponent<HealthComponent>();
            if (targetHealth)
            {
                targetHealth.HitTarget(damage + (int)playerController.magic, Vector2.zero);
                ApplyXP();
            }
        }

        if (Input.GetKey(KeyCode.Space) && _spellTimer > 0)
        {
            StartCoroutine(TickDamage());
        }
        else
        {
            GetComponent<Animator>().SetBool("isCasting", false);
            Destroy(_lightningSystem);
        }
    }
    
    void FixedUpdate()
    {
            _spellTimer -= Time.fixedDeltaTime;
    }
}
