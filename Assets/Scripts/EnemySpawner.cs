using System.Collections;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float difficulty;

    public GameObject basicEnemy;
    public bool canSpawn = true;
    private DifficultyComponent _d;

    void Start()
    {
        _d = GameObject.Find("GameManager").GetComponent<DifficultyComponent>();

        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        difficulty = _d.difficultyMultiplier * Mathf.Pow(_d.timeElapsed, 2);
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(10f);

        if (canSpawn)
        {
            for (int i = 0; i < difficulty % 10; i++)
            {
                float x = Random.Range(-5, 5);
                float y = Random.Range(-5, 5);
                GameObject newEnemy = Instantiate(basicEnemy, transform.parent.position, Quaternion.identity);
                AIComponent enemyComponent = newEnemy.GetComponent<AIComponent>();
                enemyComponent.damage = (int)(difficulty % 10);
                newEnemy.GetComponent<HealthComponent>().maxHitPoints = (int)difficulty;
                newEnemy.transform.localPosition = new Vector3(transform.position.x + x, transform.position.y + y, 0);
            }
        }

        StartCoroutine(SpawnEnemies());
    }
}
