﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Alien : MonoBehaviour
{
    public UnityEvent OnDestroy;
    public Transform target;        //Target for alien
    private NavMeshAgent agent;

    public float navigationUpdate;

    private float navigationTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                agent.destination = target.position;
                navigationTime = 0;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Die();     //Destroys alien on contact with selected game object.
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
    }

    public void Die()
    {
        Destroy(gameObject);
        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
    }
}
