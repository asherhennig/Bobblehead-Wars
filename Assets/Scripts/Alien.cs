using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Alien : MonoBehaviour
{
    public UnityEvent OnDestroy;
    public Transform target;        //Target for alien
    public Rigidbody head;

    private NavMeshAgent agent;

    public float navigationUpdate;
    public bool isAlive = true;

    private float navigationTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
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

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAlive)
        {
            Die();                               //Destroys alien on contact with selected game object.
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        }
    }

    public void Die()
    {
        isAlive = false;
        head.GetComponent<Animator>().enabled = false;
        head.isKinematic = false;
        head.useGravity = true;
        head.GetComponent<SphereCollider>().enabled = true;
        head.gameObject.transform.parent = null;
        head.velocity = new Vector3(0, 26.0f, 3.0f);

        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        Destroy(gameObject);
    }
}
