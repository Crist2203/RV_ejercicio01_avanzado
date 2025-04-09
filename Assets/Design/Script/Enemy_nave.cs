using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_nave : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 direction;
    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
