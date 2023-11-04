using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    private Transform _myTransform;

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _myTransform.position += _myTransform.forward * (Time.deltaTime * _moveSpeed);
    }
}
