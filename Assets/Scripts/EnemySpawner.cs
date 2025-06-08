using Unity.Mathematics.Geometry;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int difficulty;

    public GameObject basicEnemy;

    void Start()
    {
        for (int i = 0; i < difficulty; i++)
        {
            float x = Random.Range(-5, 5);
            float y = Random.Range(-5, 5);
            GameObject newEnemy = Instantiate(basicEnemy, transform.parent.position, Quaternion.identity);
            newEnemy.transform.localPosition = new Vector3(transform.position.x + x, transform.position.y + y, 0);
        }
    }
}
