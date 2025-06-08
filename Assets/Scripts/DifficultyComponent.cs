using UnityEngine;
using UnityEngine.UI;

public class DifficultyComponent : MonoBehaviour
{
    public float timeElapsed;
    public float difficultyMultiplier;
    public Text timerText;
    public Text hitPointsText;
    private HealthComponent _playerHealth;

    void Start()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>();
    }
    void FixedUpdate()
    {
        timeElapsed += Time.fixedDeltaTime;
        timerText.text = timeElapsed.ToString("00:00");
        hitPointsText.text = _playerHealth.hitPoints.ToString();
    }
}
