using System.Collections;
using UnityEngine;

public class Greataxe : Weapon
{
    /*
     * The Greataxe uses Strength, and is slow, but powerful attacks.
     */
    public float attackDelay;
    public override void Attack()
    {
        gameObject.GetComponent<Animator>().SetTrigger("attack");

        StartCoroutine(CompleteAttack());
    }

    IEnumerator CompleteAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        
        Vector2 origin = transform.position;
        Vector2 size = new Vector2(1, 2);
        float angle = transform.rotation.eulerAngles.z;
        Vector2 direction = transform.right;
        
        var targets = Physics2D.BoxCastAll(origin, size, angle, direction, range, targetLayer);

        foreach (var target in targets)
        {
            HealthComponent targetHealthComponent = target.collider.gameObject.GetComponent<HealthComponent>();
            print(target.transform.gameObject);
            if (targetHealthComponent)
            {
                Vector2 knockbackForce = (target.transform.position - transform.position) * knockback;
                
                targetHealthComponent.HitTarget(damage, knockbackForce);
            }
        }
    }
}