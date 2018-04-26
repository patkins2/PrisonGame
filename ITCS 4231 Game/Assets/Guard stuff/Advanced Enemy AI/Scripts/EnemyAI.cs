using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent (typeof(Animation))]
[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent (typeof(AudioSource))]
public class EnemyAI : MonoBehaviour
{
	private Transform[] patrolPoints;
	private GameObject player;	
	private AudioClip gunfireSound;
	private AudioClip enemyAlertSound;
	private AudioClip enemyDeathSound;
	private float walkSpeed;
	private float runSpeed;
	private float maxPatrolWaitTime;
	private float maxAttackDistance;
	private float minAttackDistance;
	private int enemyType;
	private NavMeshAgent agent;
	private Vector3 defaultPos;
	private Vector3 enemySize;
	private Vector3 playerSize;
	private Vector3 positionToInvestigate;
	private Animation anim;
	private AudioSource mSource;
	private GameObject playerLookAt;
	private GameObject sightObject;
	private GameObject[] deathSpawnItems;
	private int destPoint = 0;
	private float distanceToAttackFrom;
	private float ammountOfPlayerDamage;
	private float ammountOfEnemyDamage;
	private float switchOffRadarDistance;
	private float retreatDistance;
	private float delayBeforeDestroy;
	private int difficulty;
	private bool hasFiredOne;
	private bool isWaiting;
	private bool isAttacking;
	private bool hasDeathAnim;
	private int waypointCount;
	//animations
	private AnimationClip[] walkAnimations;
	private AnimationClip[] runAnimations;
	private AnimationClip[] aimAnimations;
	private AnimationClip[] shootAnimations;
	private AnimationClip[] idleAnimations;
	private AnimationClip[] deathAnimations;
	//Debugging
	public bool playerIsDead;
	public bool playerDetected;
	public bool patrolReset;
	public bool isSeeking;
	private bool enemyRetreating;
	private bool hasKilledEnemy;
	private float currentPlayerHealth;
	public float currentEnemyHealth = 100f;
	private bool randomPatroler;
	private bool hasPlayedDetectSound;
	private bool hasPlayedShootingSound;

	AIData info;

	void Awake ()
	{
		info = transform.parent.GetComponent<AIData> ();

		player = info.player;

		player = info.player;
		if(player == null)
			player = GameObject.Find ("Player");
		if(player == null)
			player = GameObject.FindWithTag ("Player");
		
		waypointCount = 0;
		Transform par = transform.parent;
		int childrenCount = par.childCount;

		for (int i = 0; i < childrenCount; i++) {
			if (par.GetChild (i).GetComponent <WaypointIdentifier> () != null) {
				waypointCount += 1;
			}
		}
		patrolPoints = new Transform[waypointCount];
		int curIndex = 0;
		for (int i = 0; i < childrenCount; i++) {
			if (par.GetChild (i).GetComponent <WaypointIdentifier> () != null) {
				patrolPoints [curIndex] = transform.parent.GetChild (i);
				if(patrolPoints [curIndex].gameObject.GetComponent <MeshRenderer> () != null)
					patrolPoints [curIndex].gameObject.GetComponent <MeshRenderer> ().enabled = false;
				if(patrolPoints [curIndex].gameObject.GetComponent <Collider> () != null)
					patrolPoints [curIndex].gameObject.GetComponent <Collider> ().enabled = false;
				curIndex++;
			}
		}
		
		enemySize = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);
		playerSize = new Vector3 (player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
		
		sightObject = GameObject.CreatePrimitive (PrimitiveType.Cube);
		sightObject.transform.SetParent (transform);
		sightObject.transform.localPosition = new Vector3 (0, (enemySize.y / 2), 0);
		sightObject.transform.localScale = new Vector3 (enemySize.x / 5, enemySize.y / 5, enemySize.z / 5);
		sightObject.name = "EnemyRadar";
		sightObject.AddComponent <EnemyRadar> ();
		sightObject.GetComponent <Collider> ().isTrigger = true;
		sightObject.GetComponent <MeshRenderer> ().enabled = false;

		//setup player lookAt
		if (player.transform.Find ("playerLookAt") == null) {
			playerLookAt = GameObject.CreatePrimitive (PrimitiveType.Cube);
			playerLookAt.transform.SetParent (player.transform);
			playerLookAt.transform.localPosition = new Vector3 (0, 0, 0);
			playerLookAt.transform.localScale = new Vector3 (playerSize.x / 5, playerSize.y / 5, playerSize.z / 5);
			playerLookAt.name = "playerLookAt";
			playerLookAt.tag = "Player";
			MeshRenderer rend = playerLookAt.GetComponent <MeshRenderer> ();
			DestroyImmediate (rend);
			MeshFilter mf = playerLookAt.GetComponent <MeshFilter> ();
			DestroyImmediate (mf);
			playerLookAt.GetComponent <Collider> ().isTrigger = true;
		}
	}

	void Start ()
	{
		if (player == null)
			Debug.Log ("No player found. Please attach aplayer ot target");
		
		ammountOfPlayerDamage = info.playerTakeDamageValue;
		ammountOfEnemyDamage = info.enemyTakeDamageValue;
		delayBeforeDestroy = info.delayBeforeDestroy;

		//get animations
		walkAnimations = info.walkAnimations;
		runAnimations = info.runAnimations;
		aimAnimations = info.aimAnimations;
		shootAnimations = info.shootAndAttackAnimations;
		idleAnimations = info.idleAnimations;
		deathAnimations = info.deathAnimations;

		deathSpawnItems = info.deathSpawnItems;

		gunfireSound = info.gunfireSound;
		enemyAlertSound = info.enemyAlertSound;
		enemyDeathSound = info.enemyDeathSound;

		//Add a navmesh agent to the enemy
		if (GetComponent <NavMeshAgent> () == null)
			gameObject.AddComponent <NavMeshAgent> ();
		
		agent = GetComponent<NavMeshAgent> ();
		agent.angularSpeed = 360f;
		agent.speed = info.walkSpeed;
		agent.autoBraking = false;
		agent.stoppingDistance = 1f;
		minAttackDistance = info.minAttackDistance;
		maxAttackDistance = info.maxAttackDistance;

		if (GetComponent <AudioSource> () == null)
			gameObject.AddComponent <AudioSource> ();
		mSource = GetComponent <AudioSource>();
		mSource.volume = info.aiVolume;

		if (info.enemyBehavior == AIData.EnemyType.Patroling)
			enemyType = 1;
		else {
			enemyType = 2;
			patrolPoints = new Transform[0];
		}
		
		defaultPos = gameObject.transform.position;
		positionToInvestigate = defaultPos;
		isWaiting = false;

		distanceToAttackFrom = Random.Range (minAttackDistance, maxAttackDistance);

		anim = GetComponent <Animation> ();
		if (anim == null)
			anim = gameObject.AddComponent <Animation> ();

		string newName;
		if (idleAnimations != null && idleAnimations.Length > 0) {
			for (int i = 0; i < idleAnimations.Length; i += 1) {
				newName = idleAnimations [i].name;
				idleAnimations [i].legacy = true;
				anim.AddClip (idleAnimations[i], newName);
			}
		}
		if (walkAnimations != null && walkAnimations.Length > 0) {
			for (int i = 0; i < walkAnimations.Length; i += 1) {
				newName = walkAnimations [i].name;
				walkAnimations [i].legacy = true;
				anim.AddClip (walkAnimations [i], newName);
			}
		}
		if (runAnimations != null && runAnimations.Length > 0) {
			for (int i = 0; i < runAnimations.Length; i += 1) {
				newName = runAnimations [i].name;
				runAnimations [i].legacy = true;
				anim.AddClip (runAnimations [i], newName);
			}
		}
		if (aimAnimations != null && aimAnimations.Length > 0) {
			for (int i = 0; i < aimAnimations.Length; i += 1) {
				newName = aimAnimations [i].name;
				aimAnimations [i].legacy = true;
				anim.AddClip (aimAnimations [i], newName);
			}
		}
		if (shootAnimations != null && shootAnimations.Length > 0) {
			for (int i = 0; i < shootAnimations.Length; i += 1) {
				newName = shootAnimations [i].name;
				shootAnimations [i].legacy = true;
				anim.AddClip (shootAnimations [i], newName);
			}
		}
		if (deathAnimations != null && deathAnimations.Length > 0) {
			for (int i = 0; i < deathAnimations.Length; i += 1) {
				deathAnimations[i].wrapMode = WrapMode.Once;
				newName = deathAnimations [i].name;
				deathAnimations [i].legacy = true;
				anim.AddClip (deathAnimations [i], newName);
			}
		}

		if (idleAnimations != null)
			playAnimation (idleAnimations);
	}

	public void GotoNextPoint ()
	{
		if (patrolPoints.Length == 0)
			return;
//		Debug.Log ("Going to next point...");
		StartCoroutine (pauseAndContinuePatrol ());
	}

	void Update ()
	{

		sightObject.transform.localRotation = transform.parent.rotation;
		gunfireSound = info.gunfireSound;
		walkSpeed = info.walkSpeed;
		runSpeed = info.runSpeed;
		maxPatrolWaitTime = info.maxPatrolWaitTime;
		maxAttackDistance = info.maxAttackDistance;
		minAttackDistance = info.minAttackDistance;
		randomPatroler = info.randomPatroler;
		switchOffRadarDistance = info.switchOffRadarDistance;
		if (switchOffRadarDistance < info.sightDistance && switchOffRadarDistance < info.hearingRadius) {
			if (info.sightDistance > info.hearingRadius)
				switchOffRadarDistance = info.sightDistance;
			else
				switchOffRadarDistance = info.hearingRadius;
		}

		retreatDistance = info.retreatDistance;
		hasDeathAnim = info.hasDeathAnim;
		currentPlayerHealth = PlayerHealthManager.currentHealth;

		if (currentEnemyHealth <= 0 && !hasKilledEnemy) {
			if(enemyDeathSound != null)
				mSource.PlayOneShot (enemyDeathSound);
			killEnemy ();
		}

		if (!hasKilledEnemy) {
			if (!playerDetected && !isWaiting && agent.remainingDistance <= 1f) {
				GotoNextPoint ();
			}
		}

		if (playerIsDead || patrolReset) {
			playerDetected = false;
			resetPatrol ();
		}

		//if player has gone too far
		if (playerDetected && agent.remainingDistance > retreatDistance) {
//			Debug.Log ("Player outta Range...");
			patrolReset = true;
		}

		if (getDistanceToPlayer () >= switchOffRadarDistance) {
			sightObject.SetActive (false);
		} else if (getDistanceToPlayer () < switchOffRadarDistance && !enemyRetreating) {
			sightObject.SetActive (true);
		}

		if (isSeeking) {
			positionToInvestigate = player.transform.position;
			this.agent.destination = positionToInvestigate;
			if (this.agent.transform.position == positionToInvestigate) {
				pauseAndContinuePatrol ();
				isSeeking = false;
			}
		}

		if (playerDetected) {
			if(enemyAlertSound != null && !hasPlayedDetectSound)
				mSource.PlayOneShot (enemyAlertSound);
			hasPlayedDetectSound = true;

//			Debug.Log ("Chasing player");
			isSeeking = false;
			isAttacking = true;
			this.agent.destination = player.transform.position;

			agent.stoppingDistance = distanceToAttackFrom;

			if (Vector3.Distance (transform.position, player.transform.position) > distanceToAttackFrom) {
				if (runAnimations != null)
					playAnimation (runAnimations);
				agent.speed = runSpeed;
			}
			if (agent.remainingDistance <= distanceToAttackFrom) {
				if (shootAnimations != null)
					playAnimation (shootAnimations);
				Vector3 pointToLook = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z);
				transform.LookAt (pointToLook);
				if (!playerIsDead && !hasFiredOne) {
					StartCoroutine (addShootingRandomness ());
				}
			}
		}
	}

	void shoot ()
	{
		if(gunfireSound != null && !hasPlayedShootingSound)
			mSource.PlayOneShot (gunfireSound);
		hasPlayedShootingSound = true;
		player.SendMessage ("playerTakeDamage", ammountOfPlayerDamage, SendMessageOptions.DontRequireReceiver);
	}

	IEnumerator pauseAndContinuePatrol ()
	{

		agent.stoppingDistance = 1f;
		isWaiting = true;
		if (idleAnimations != null)
			playAnimation (idleAnimations);

		float waitTime = Random.Range (maxPatrolWaitTime - 3f, maxPatrolWaitTime);
		if (waitTime <= 0f)
			waitTime = 1f;

		yield return new WaitForSeconds (waitTime);

		if (randomPatroler) {
			agent.destination = patrolPoints [destPoint].position;
			int nextPos;
			do {
				nextPos = Random.Range (0, patrolPoints.Length);
			} while (nextPos == destPoint);
			destPoint = nextPos;
		} else {
				agent.destination = patrolPoints [destPoint].position;
				destPoint = (destPoint + 1) % patrolPoints.Length;
		}
		if (walkAnimations != null)
			playAnimation (walkAnimations);
		isWaiting = false;
	}

	void goToNextPointDirect ()
	{
		if (randomPatroler) {
			agent.destination = patrolPoints [destPoint].position;
			int nextPos;
			do {
				nextPos = Random.Range (0, patrolPoints.Length);
			} while (nextPos == destPoint);
			destPoint = nextPos;
		} else {
			if (enemyType == 1) {
				agent.destination = patrolPoints [destPoint].position;
				destPoint = (destPoint + 1) % patrolPoints.Length;
			} else {
				agent.destination = defaultPos;
			}
		}
		if (walkAnimations != null)
			playAnimation (walkAnimations);
	}

	public void attackFromElsewhere ()
	{
		StartCoroutine (addShootingRandomness ());
	}

	IEnumerator reEnableSight ()
	{
		enemyRetreating = true;
		yield return new WaitForSeconds (10f);
		sightObject.SetActive (true);
		enemyRetreating = false;
	}

	void resetPatrol ()
	{
		hasPlayedDetectSound = false;
//		Debug.Log ("Patrol reset...");
		if (playerIsDead) {
			sightObject.SetActive (false);
			StartCoroutine (reEnableSight ());
		}
		patrolReset = false;
		playerIsDead = false;
		isAttacking = false;
		playerDetected = false;
		isSeeking = false;
		agent.speed = walkSpeed;

		agent.stoppingDistance = 1f;
//		if (enemyType == 1) {
			if (walkAnimations != null)
				playAnimation (walkAnimations);
			if (enemyType == 1 && patrolPoints.Length > 0)
				goToNextPointDirect ();
			else
				agent.destination = defaultPos;
	}

	IEnumerator addShootingRandomness ()
	{
		hasFiredOne = true;
		float n = Random.Range (0.8f, 1.6f);
		yield return new WaitForSeconds (n);
		shoot ();
		hasFiredOne = false;
		hasPlayedShootingSound = false;
	}

	void playAnimation (AnimationClip clip)
	{
		anim.Play (clip.name);
	}

	void playAnimation (AnimationClip[] clips)
	{
		if (clips.Length > 0) {
			int rand = Random.Range (0, clips.Length);
			if (clips [rand] != null)
				anim.Play (clips [rand].name);
//			Debug.Log ("Now Playing: " + clips [rand].name);
		} else {
//			Debug.LogWarning("Some enemy animations are missing ");
		}
	}

	float playDeathAnimationReturnLenth (AnimationClip[] clips)
	{
//		anim.Stop ();
		float duration = 0;
		if (clips.Length > 0) {
			int rand = Random.Range (0, clips.Length);
			if (clips [rand] != null) {
				duration = clips [rand].length;
				anim.Play (clips [rand].name);
//				Debug.Log ("Now Playing: " + clips [rand].name);
			}
		}
		return duration;
	}

	public float getDistanceToPlayer ()
	{
		return Vector3.Distance (transform.position, player.transform.position);
	}

	public void setPlayerDetected ()
	{
		playerDetected = true;
	}

	public void setPlayerNotDetected ()
	{
		playerDetected = false;
	}

	public void setResetPatrolBool ()
	{
		patrolReset = true;
	}

	public void enemyTakeDamage(){
		currentEnemyHealth -= ammountOfEnemyDamage;
	}

	void killEnemy(){
		//embed enemy kill code
		hasKilledEnemy = true;
		mSource.Stop ();
		if (hasDeathAnim && deathAnimations != null && deathAnimations.Length > 0) {
			float deathAnimLength = playDeathAnimationReturnLenth (deathAnimations);
			if (delayBeforeDestroy > 0f) {
				playerDetected = false;
				sightObject.SetActive (false);
				StartCoroutine(killEnemyAfterDeathAnim (deathAnimLength));
			}
		} else {
			if (deathSpawnItems != null && deathSpawnItems.Length > 0) {
				Vector3 location = transform.position;
				Destroy (transform.parent.gameObject);
				for (int i = 0; i < deathSpawnItems.Length; i += 1) {
					if (deathSpawnItems [i] != null) {
						GameObject go = Instantiate (deathSpawnItems [i], location, Quaternion.identity);
						go.AddComponent <TimedDestroy>();
					}
				}
			} else {
				Destroy (transform.parent.gameObject);
//				Debug.Log ("Enemy killed");
			}
		}
	}

	IEnumerator killEnemyAfterDeathAnim(float lnth){
		yield return new WaitForSeconds (lnth);
		if (deathSpawnItems != null && deathSpawnItems.Length > 0) {
			Vector3 location = transform.position;
			Destroy (transform.parent.gameObject);
			for (int i = 0; i < deathSpawnItems.Length; i += 1) {
				if (deathSpawnItems [i] != null) {
					GameObject go = Instantiate (deathSpawnItems [i], location, Quaternion.identity);
					go.AddComponent <TimedDestroy>();
				}
			}
		} else {
			Destroy (transform.parent.gameObject, delayBeforeDestroy);
		}
	}

}