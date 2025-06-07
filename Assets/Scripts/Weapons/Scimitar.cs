using UnityEngine;

public class Scimitar : Weapon
{
    
    //Scimitars are quick, but do lower damage and use Dexterity
    public override void Attack()
    {
        gameObject.GetComponent<Animator>().SetTrigger("attack");
        
        Vector2 origin = gameObject.transform.position;
        Vector2 size = new Vector2(1, 1);
        float angle = transform.rotation.eulerAngles.z;
        Vector2 direction = gameObject.transform.right;
        
        var target = Physics2D.BoxCast(origin, size, angle, direction, range, targetLayer);

        if (target)
        {
            HealthComponent targetComponent = target.collider.GetComponent<HealthComponent>();

            if (targetComponent)
            {
                Vector2 knockbackForce = (target.transform.position - transform.position) * knockback;
                targetComponent.HitTarget(damage, knockbackForce);
            }
        }
    }
}
