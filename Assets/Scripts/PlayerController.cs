using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /**
     * This Script handles player controls, both moving and shooting.
     */

    [Header("Stats")] 
    public float strength;
    public float dexterity;
    public float movementSpeed;
    public float constitution;
    public float magic;

    [Header("Weapons")] 
    public GameObject heldWeapon = null;
    public Weapon activeWeapon = null;

    private Rigidbody2D _rb;
    private Vector2 _input;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        if (_rb == null)
            print("No Rigidbody Attached.");
        
        if (heldWeapon)
            activeWeapon = heldWeapon.GetComponent<Weapon>();
    }

    void Update()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            activeWeapon.Attack();
        }
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 diff = mousePos - (Vector2)transform.position;
        
        transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg + 90f);
        
        MoveWeapon();
    }

    void FixedUpdate()
    {
        Vector2 targetMovement = _input * movementSpeed;
        
        Vector2 force = (targetMovement - _rb.linearVelocity) / Time.fixedDeltaTime;
        
        _rb.AddForce(force);
    }

    void MoveWeapon()
    {
        if (heldWeapon)
        {
            heldWeapon.transform.position = transform.position + transform.right * 1;
            heldWeapon.transform.rotation = transform.rotation;
        }
    }

    void AttachWeapon(GameObject weapon)
    {
        if (heldWeapon)
            RemoveWeapon();
        heldWeapon = weapon;
    }

    void RemoveWeapon()
    {
        heldWeapon = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
            AttachWeapon(other.gameObject);
    }
}
