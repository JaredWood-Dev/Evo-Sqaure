using System.Collections;
using Enums;
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
    public float movementSpeed; //TODO: IMPLEMENT LOGORITHMIC GROWTH
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
    private bool _canMoveCamera = true;

    void Start()
    {
        //Init Stats
        strength = PlayerStats.Instance.strength;
        dexterity = PlayerStats.Instance.dexterity;
        movementSpeed = PlayerStats.Instance.speed;
        constitution = PlayerStats.Instance.constitution;
        magic = PlayerStats.Instance.magic;
        
        _rb = GetComponent<Rigidbody2D>();
        
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

        if (Input.GetKeyDown(KeyCode.Space) && _coolDown < 0)
        {
            activeWeapon.Attack();
            
            //Apply the appropriate weapon xp increases
            switch (activeWeapon.weaponStat)
            {
                case Stat.Strength:
                    strengthXP += strengthXPRate;
                    break;
                case Stat.Dexterity:
                    dexterityXP += dexterityXPRate;
                    break;
                case Stat.Magic:
                    magicXP += magicXPRate;
                    break;
            }

            _coolDown = activeWeapon.attackSpeed;
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
        
        if (other.CompareTag("Room"))
            _canMoveCamera = true;
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
        
        //Reload the Scene
        //TODO: REPLACE WITH BETTER SCENE MANAGEMENT
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.5f);
        
        SceneManager.LoadScene(0);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (_canMoveCamera)
        {
            _canMoveCamera = false;
            if (other.CompareTag("Room"))
            {
                if (transform.position.x - other.transform.position.x > 10)
                {
                    Camera.main.GetComponent<CameraController>().ShiftCamera(Direction.East);
                }

                if (transform.position.x - other.transform.position.x < -10)
                {
                    Camera.main.GetComponent<CameraController>().ShiftCamera(Direction.West);
                }

                if (transform.position.y - other.transform.position.y > 10)
                {
                    Camera.main.GetComponent<CameraController>().ShiftCamera(Direction.North);
                }

                if (transform.position.y - other.transform.position.y < -10)
                {
                    Camera.main.GetComponent<CameraController>().ShiftCamera(Direction.South);
                }
            }
        }
    }
}
