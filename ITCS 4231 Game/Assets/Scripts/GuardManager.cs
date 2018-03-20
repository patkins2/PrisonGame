using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardManager : MonoBehaviour {

    public Animator anim;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    public AudioClip footstep;
    AudioSource audioSource;

    public GameObject target;
    private int playerPosition;
    private Vector3 targetTran;
    public int maxRange;
    public int minRange;

    // Use this for initialization
    void Start () {

        audioSource = GetComponent<AudioSource>();
        anim = gameObject.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        footstep = AudioClip.Equals(footstep_concrete_1);

        target = GameObject.FindWithTag("Player");
        targetTran = target.transform.position;

        agent.autoBraking = false;

        GotoNextPoint();

    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void Update () {

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
        if ((Vector3.Distance(transform.position, target.transform.position) < maxRange) && (Vector3.Distance(transform.position, target.transform.position) > minRange) && )
        {
            transform.LookAt(targetTran);
            transform.Translate(Vector3.forward * Time.deltaTime);
        }

    }
}
