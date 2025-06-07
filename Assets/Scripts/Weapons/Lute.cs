using UnityEngine;

public class Lute : Weapon
{
    public override void Attack()
    {
        Vector2 origin = transform.position;
        Vector2 size = new Vector2(1, 1);
        float angle = transform.rotation.eulerAngles.z;
        Vector2 direction = transform.right;
        
        var targets = Physics2D.BoxCastAll(origin, size, angle, direction, range, targetLayer);

        foreach (var target in targets)
        {
            HealthComponent targetHealth = target.collider.GetComponent<HealthComponent>();
            if (targetHealth)
            {
                targetHealth.HitTarget(damage, transform.position);
            }
        }
    }
}
