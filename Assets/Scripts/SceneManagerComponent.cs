using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerComponent : MonoBehaviour
{
    public GameObject HUD;
    public GameObject Player;
    public GameObject deathScreen;

    [Header("Stat Labels")] 
    public Text strengthLabel;
    public Text dexterityLabel;
    public Text speedLabel;
    public Text constitutionLabel;
    public Text magicLabel;

    private bool _canRestart = false;

    void Start()
    {
        HUD.SetActive(true);
        deathScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void PlayerDeath()
    {
        _canRestart = true;
        HUD.SetActive(false);
        deathScreen.SetActive(true);
        Time.timeScale = 0;
        
        strengthLabel.text = Player.GetComponent<PlayerController>().strength.ToString("0000") + " + " + Player.GetComponent<PlayerController>().strengthXP.ToString("000");
        dexterityLabel.text = Player.GetComponent<PlayerController>().dexterity.ToString("0000") + " + " + Player.GetComponent<PlayerController>().dexterityXP.ToString("000");
        speedLabel.text = Player.GetComponent<PlayerController>().movementSpeed.ToString("0000") + " + " + Player.GetComponent<PlayerController>().movementSpeedXP.ToString("000");
        constitutionLabel.text = Player.GetComponent<PlayerController>().constitution.ToString("0000") + " + " + Player.GetComponent<PlayerController>().constitutionXP.ToString("000");
        magicLabel.text = Player.GetComponent<PlayerController>().magic.ToString("0000") + " + " + Player.GetComponent<PlayerController>().magicXP.ToString("000");
    }

    void Update()
    {
        if (_canRestart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
