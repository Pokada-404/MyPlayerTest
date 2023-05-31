using System.Collections;
using UnityEngine;

public class Test_a : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Aa());
        Time.timeScale = 0;
    }
    private void Update()
    {
        print("a");
    }
    private void FixedUpdate()
    {
        print("b");
    }
    private IEnumerator Aa()
    {
        while (true)
        {
            transform.position += Vector3.up * 0.01f;
            yield return null;
        }
    }
}