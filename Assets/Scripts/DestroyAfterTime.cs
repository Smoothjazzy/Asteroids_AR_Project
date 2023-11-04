using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    [SerializeField] private float _destroyDelay = 5;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _destroyDelay);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
