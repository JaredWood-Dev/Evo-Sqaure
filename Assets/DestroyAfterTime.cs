using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time;

    void Start()
    {
        StartCoroutine(StartTime(time));
    }

    IEnumerator StartTime(float t)
    {
        yield return new WaitForSeconds(t);
        
        Destroy(gameObject);
    }
}
