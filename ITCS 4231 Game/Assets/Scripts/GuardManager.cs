using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardManager : MonoBehaviour {

    public Animator anim;
    public Transform[] points;
    public Transform Player;
    //private int destPoint = 0;
    private NavMeshAgent agent;
    public AudioClip footstep;
    AudioSource audioSource;

    public GameObject target;
    private int playerPosition;
    private Vector3 targetTran;
    public int maxRange = 10;
    public int minRange = 5;
    int MoveSpeed = 2;

    // Use this for initialization
    void Start () {

        /*audioSource = GetComponent<AudioSource>();
        anim = gameObject.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //footstep = AudioClip.Equals(footstep_concrete_1);

        target = GameObject.FindWithTag("Player");
        targetTran = target.transform.position;

        agent.autoBraking = false;

        //GotoNextPoint();*/

        //anim.SetInteger(HashIDs.self.guardMovementTypeInt, (int)GuardMovementType.idle);
        //print("Guard Mvmt type: " + anim.GetInteger(HashIDs.self.guardMovementTypeInt));
    }

    /*void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }*/

    // Update is called once per frame
    void Update () {

        //transform.LookAt(Player);

        if (Vector3.Distance(transform.position, Player.position) >= minRange && Vector3.Distance(transform.position, Player.position) <= maxRange)
        {
            anim.SetInteger(HashIDs.self.guardMovementTypeInt, (int)GuardMovementType.chase);
            //print("chase " + anim.GetInteger(HashIDs.self.guardMovementTypeInt));
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            transform.LookAt(Player);

            /*if (Vector3.Distance(transform.position, Player.position) <= maxRange)
            {
                //Here Call any function U want Like Shoot at here or something
            }*/

        }
        else
        {
            anim.SetInteger(HashIDs.self.guardMovementTypeInt, (int)GuardMovementType.shoot);
            //print("aim" + HashIDs.self.guardMovementTypeInt.ToString());
        }
    }
}
