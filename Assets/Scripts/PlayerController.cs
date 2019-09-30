﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float[] hitForce;
    public float moveSpeed = 50.0f;     //Movement speed for player
    public float timeBetweenHits = 2.5f;

    private int hitNumber = -1;
    private float timeSinceHit = 0;
    private bool isHit = false;
    private bool isDead = false;

    public Rigidbody marineBody;
    public Rigidbody head;
    public LayerMask layerMask;
    public Animator bodyAnimator;

    private Vector3 currentLookTarget = Vector3.zero;
    private CharacterController characterController;
    private DeathParticles deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        deathParticles = gameObject.GetComponentInChildren<DeathParticles>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),
                                0, Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);

        if (isHit)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > timeBetweenHits)
            {
                isHit = false;
                timeSinceHit = 0;
            }
        }
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

    void OnTriggerEnter(Collider other)
    {
        Alien alien = other.gameObject.GetComponent<Alien>();
        if (alien != null)
        {
            //1
            if (!isHit)
            {
                //2
                hitNumber += 1;
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                //3
                if (hitNumber < hitForce.Length)
                {
                    cameraShake.intensity = hitForce[hitNumber];
                    cameraShake.Shake();
                }
                else
                {
                    Die();
                }
                //4
                isHit = true;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.hurt);
            }
            alien.Die();
        }
    }

    public void Die()
    {
        bodyAnimator.SetBool("IsMoving", false);
        marineBody.transform.parent = null;
        marineBody.isKinematic = false;
        marineBody.useGravity = true;
        marineBody.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        marineBody.gameObject.GetComponent<Gun>().enabled = false;

        Destroy(head.gameObject.GetComponent<HingeJoint>());
        head.transform.parent = null;
        head.useGravity = true;

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.marineDeath);

        deathParticles.Activate();

        Destroy(gameObject);
    }
}
