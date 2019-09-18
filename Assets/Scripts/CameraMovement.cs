using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This Script attaches the camera to the player 
keeping the camera at a fixed angle to the character*/

public class CameraMovement : MonoBehaviour
{
    public GameObject followTarget;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)                                                 //if there is movement enter statement
        {                                                           
            transform.position = Vector3.Lerp(transform.position,                 //Moves camera with player  
                                            followTarget.transform.position,
                                            Time.deltaTime * moveSpeed);
        }
    }
}
