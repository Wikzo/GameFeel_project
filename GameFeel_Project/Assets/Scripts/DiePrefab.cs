using UnityEngine;
using System.Collections;

public class DiePrefab : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        StartCoroutine(DestroyMe());
    }

    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
