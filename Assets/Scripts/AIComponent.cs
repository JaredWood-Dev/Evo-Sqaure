using UnityEngine;

public class AIComponent : MonoBehaviour
{

    [Header("Movement")]
    public float movementSpeed;
    public GameObject playerTarget;
    public float minForce;
    public float maxForce;
    
    [Header("Combat")]
    public int damage;
    public float knockBack;
    public float aggroDistance;

    

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
            Wander();
        }
    }
    
    void MoveTo(Vector2 location)
    {
        Vector2 diff = location - (Vector2)transform.position;
        
        transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg);
        
        Vector2 locDiff = location - (Vector2)transform.position;
        Vector2 neededSpeed = locDiff * movementSpeed;
        Vector2 forceVector = (neededSpeed - _rb.linearVelocity) / Time.fixedDeltaTime;
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

    void Wander()
    {
        Vector2 target = (Vector2)transform.position + new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)); 
        
        while (Vector2.Distance(transform.position, target) > 0.1f)
            MoveTo(target);
        
    }

    GameObject LocatePlayer(float distance)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < distance)
            {
                return player;
            }
        }
        return null;
    }
    
    
}
