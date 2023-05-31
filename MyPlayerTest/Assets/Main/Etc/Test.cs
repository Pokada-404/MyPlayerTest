using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    float a = 10;
    private void Update()
    {
        a -= 0.1f;
        transform.position += Vector3.up * a * Time.deltaTime;
        if(a == 0) { a = 10; }
    }

}
