using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform launchPosition;

    void fireBullet ()
    {
        // 1    Sets Game Object to Bullet Prefab
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        // 2    Sets start position to front of gun barrel
        bullet.transform.position = launchPosition.position;
        // 3    Moves bullets
        bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * 100;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {                   
        if (Input.GetMouseButtonDown(0))                    
        {
            if (!IsInvoking("fireBullet"))
            {
                InvokeRepeating("fireBullet", 0f, 0.1f);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("fireBulet");
        }
    }
}
