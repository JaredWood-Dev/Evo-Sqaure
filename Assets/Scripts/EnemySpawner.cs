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
            float x = Random.Range(-10f, 10f);
            float y = Random.Range(-10f, 10f);
            Instantiate(basicEnemy, transform.parent.position += new Vector3(x, y), Quaternion.identity);
        }
    }
}
