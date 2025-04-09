using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_nave : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 direction;
    private float lifetime = 2f;
            
    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if(lifetime < 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
