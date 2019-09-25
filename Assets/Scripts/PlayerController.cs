using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 50.0f;     //Movement speed for player
    public Rigidbody head;
    public LayerMask layerMask;
    public Animator bodyAnimator;

    private Vector3 currentLookTarget = Vector3.zero;
    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),
                                0, Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);
    }

    void FixedUpdate()
    {
        //Player direction controls
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),
                                            0, Input.GetAxis("Vertical"));


        if (moveDirection == Vector3.zero)
        {
            bodyAnimator.SetBool("IsMoving", false);        //Set Animator to not moving if character vector = 0
        }

        else
        {
            head.AddForce(transform.right * 150, ForceMode.Acceleration);            //head bobble functionality

            bodyAnimator.SetBool("IsMoving", true);         //Set Animator to moving if character vector != 0
        }

        //Green ray that follows mouse
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        if (Physics.Raycast(ray, out hit, 1000, layerMask,
                            QueryTriggerInteraction.Ignore))
        {
        }

        //Set look direction to mouse location
        if (hit.point != currentLookTarget)
        {
            currentLookTarget = hit.point;
        }

        //1
        Vector3 targetPosition = new Vector3(hit.point.x,
                                            transform.position.y, 
                                            hit.point.z);
        //2
        Quaternion rotation = Quaternion.LookRotation(targetPosition -
                                                        transform.position);
        //3
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation,
                                                Time.deltaTime * 10.0f);
    }
}
