using UnityEngine;

public class AIComponent : MonoBehaviour
{

    [Header("Movement")]
    public float movementSpeed;
    public GameObject playerTarget;
    public Vector2 wanderTarget;
    
    [Header("Combat")]
    public int damage;
    public float knockBack;
    public float aggroDistance;

    

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        PickNewWanderTarget();
    }

    void Update()
    {
        playerTarget = LocatePlayer(aggroDistance);
        if (playerTarget)
        {
            MoveTo(playerTarget.transform.position);
        }
        else
        {
            MoveTo(wanderTarget);
        }
        
        if ((Random.Range(0, 50) == 50) || Vector3.Distance(transform.position, wanderTarget) < 0.2f)
        {
            PickNewWanderTarget();
        }
    }
    
    void MoveTo(Vector2 location)
    {
        Vector2 diff = location - (Vector2)transform.position;
        
        transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg);
        
        Vector2 locDiff = location - (Vector2)transform.position;
        Vector2 neededSpeed = locDiff * movementSpeed;
        Vector2 forceVector = (neededSpeed - _rb.linearVelocity);
        _rb.AddForce(forceVector);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 knockbackVector = (transform.position - other.gameObject.transform.position) * knockBack;
            other.gameObject.GetComponent<HealthComponent>().HitTarget(damage, knockbackVector);
        }
    }

    GameObject LocatePlayer(float distance)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < distance)
            {
                PickNewWanderTarget();
                return player;
            }
        }
        return null;
    }

    void PickNewWanderTarget()
    {
        wanderTarget = new Vector2(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5));
    }
    
}
