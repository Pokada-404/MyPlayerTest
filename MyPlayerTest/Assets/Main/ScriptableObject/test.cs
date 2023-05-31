using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    float _jumpCountPower = 0;
    float _jumpPower = 10;
    float _jumpUpPowerDeceleration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        _jumpCountPower = _jumpPower;
    }

    // Update is called once per frame
    void Update()
    {
        _jumpCountPower -= _jumpUpPowerDeceleration * Time.deltaTime;

        if (_jumpCountPower < 0) { _jumpCountPower = 0; }
        transform.position += Vector3.up * _jumpCountPower * Time.deltaTime;
    }
}
