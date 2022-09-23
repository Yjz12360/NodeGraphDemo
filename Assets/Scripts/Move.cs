using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translate = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            translate.z = translate.z + speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            translate.z = translate.z - speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            translate.x = translate.x - speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            translate.x = translate.x + speed * Time.deltaTime;
        transform.Translate(translate);

    }
}
