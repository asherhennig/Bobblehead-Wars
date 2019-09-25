using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Destroys bullets when they go off screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    //Destroys bullets when they collide with a game object
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
