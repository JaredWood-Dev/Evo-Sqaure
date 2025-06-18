using System.Collections;
using Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    //In this context, "XP" is the amount of increase to each stat after the end of a run.
    [Header("Experience and Rates")]
    public float strengthXP;
    public float dexterityXP;
    public float movementSpeedXP;
    public float constitutionXP;
    public float magicXP;
    
    //Rates are how fast the XP builds up
    [Space]
    public float strengthXPRate;
    public float dexterityXPRate;
    public float movementSpeedXPRate;
    public float constitutionXPRate;
    public float magicXPRate;

    [Header("Weapons")] 
    public GameObject playerHand;
    public GameObject heldWeapon = null;
    public Weapon activeWeapon = null;
    private float _coolDown;

    private Rigidbody2D _rb;
    private Vector2 _input;
    private HealthComponent _healthComponent;

    void Start()
    {
        //Init Stats
        strength = PlayerStats.Instance.strength;
        dexterity = PlayerStats.Instance.dexterity;
        movementSpeed = PlayerStats.Instance.speed;
        constitution = PlayerStats.Instance.constitution;
        magic = PlayerStats.Instance.magic;
        
        _rb = GetComponent<Rigidbody2D>();
        _healthComponent = GetComponent<HealthComponent>();
        
        if (_rb == null)
            print("No Rigidbody Attached.");
        
        if (heldWeapon)
            activeWeapon = heldWeapon.GetComponent<Weapon>();
    }

    void Update()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if (_input != Vector2.zero )
            movementSpeedXP += movementSpeedXPRate * Time.deltaTime;

        if (heldWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _coolDown < 0)
            {
                activeWeapon.Attack();

                _coolDown = activeWeapon.attackSpeed;
            }
        }

        _coolDown -= 0.1f;
        
        //TODO: REMOVE AFTER TESTING
        if (Input.GetKeyDown(KeyCode.LeftShift))
            GetComponent<HealthComponent>().HitTarget(5, Vector2.zero);
        
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
        if (playerHand)
        {
            //playerHand.transform.position = transform.position + transform.right * 1;
            //playerHand.transform.rotation = transform.rotation;
        }
    }

    void AttachWeapon(GameObject weapon)
    {
        if (heldWeapon)
            RemoveWeapon();
        heldWeapon = weapon;
        activeWeapon = weapon.GetComponent<Weapon>();
        heldWeapon.GetComponent<RotateObject>().enabled = false;
        
        weapon.transform.SetParent(playerHand.transform);
        weapon.GetComponent<Animator>().enabled = true; 
    }

    void RemoveWeapon()
    {
        heldWeapon.GetComponent<RotateObject>().enabled = true;
        heldWeapon.GetComponent<Animator>().enabled = false;
        heldWeapon.transform.parent = null;
        heldWeapon.transform.position = transform.position + (Vector3.up * -1.5f);
        heldWeapon = null;
        activeWeapon = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
            AttachWeapon(other.gameObject);
        
        if (other.gameObject.CompareTag("Room"))
        {
            if (Camera.main != null)
                Camera.main.GetComponent<CameraController>()
                    .ShiftCamera(other.transform.parent.GetComponent<RoomComponent>().roomLocation);
        }
    }

    public void PlayerDeath()
    {
        //Increase the stats
        strength += strengthXP;
        dexterity += dexterityXP;
        movementSpeed += movementSpeedXP;
        constitution += constitutionXP;
        magic += magicXP;
        
        //Save the stats
        PlayerStats.Instance.strength = strength;
        PlayerStats.Instance.dexterity = dexterity;
        PlayerStats.Instance.speed = movementSpeed;
        PlayerStats.Instance.constitution = constitution;
        PlayerStats.Instance.magic = magic;
        
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        
        //Reload the Scene
        GameObject.Find("GameManager").GetComponent<SceneManagerComponent>().PlayerDeath();
    }
}
