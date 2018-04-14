using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[DisallowMultipleComponent]
public class AIData : MonoBehaviour {

	[Tooltip("Player or target gameObject that the enemy should find")]
	public GameObject player;

	[Space (20)]
	[Header ("AI Agent")]
	[Tooltip("Enemy speed while walking")]
	public float walkSpeed = 1.8f;
	[Tooltip("Enemy speed while running")]
	public float runSpeed = 3.5f;
	[Tooltip("During patrol, maximum time to wait before moving to next patrol point")]
	public float maxPatrolWaitTime = 4;
	[Tooltip("Maximum distance to begin attack when player is detected")]
	public float maxAttackDistance = 8f;
	[Tooltip("Minimum distance to begin attack when player is detected")]
	public float minAttackDistance = 4f;

	[HideInInspector]
	public float retreatDistance;

	public enum EnemyType{Patroling, Standing};
	[Tooltip("Defines the enemy behavior. Patrolling waypoints or standing at a fixed position. " +
		"Position and rotation will remain as initially set until an event occurs that requires " +
		"the enemy to move or look")]
	public EnemyType enemyBehavior;
	[Tooltip("If or not the patrolling enemy moves to waypoints at random")]
	public bool randomPatroler;

	//sight and hearing
	[Space (20)]
	[Header ("Sensors")]
	[Tooltip("Enemy can see as far as this distance")]
	public float sightDistance = 12f;
	[Tooltip("Enemy can see as wide as this angle (in degrees)")]
	public float viewAngle = 90f;
	[Tooltip("Enables/disables the enemy's hearing sensor")]
	public bool canHear = true;
	[Tooltip("Enemy can hear as far as this distance in a circular fashion")]
	public float hearingRadius = 15f;
	[Tooltip("Enemy can hear target if it produces sound with volume equal to or greater than this value")]
	[Range(0, 1)]
	public float audibilityThreshold = 0.35f;

	//animations
	[Space (20)]
	[Header ("Animations")]
	[Tooltip("Enemy's walk animations.")]
	public AnimationClip[] walkAnimations;
	[Tooltip("Enemy's run animations")]
	public AnimationClip[] runAnimations;
	[Tooltip("Enemy's aim animations")]
	public AnimationClip[] aimAnimations;
	[Tooltip("Enemy's attack animations")]
	public AnimationClip[] shootAndAttackAnimations;
	[Tooltip("Enemy's idle animations")]
	public AnimationClip[] idleAnimations;
	[Tooltip("Enemy's death animations")]
	public AnimationClip[] deathAnimations;
//	public AlternateAnimations alternateAnimations;

	[Space (20)]
	[Header ("Sounds")]
	[Range(0, 1)]
	public float aiVolume = 0.6f;
	public AudioClip gunfireSound;
	public AudioClip enemyAlertSound;
	public AudioClip enemyDeathSound;

	[Space(20)]
	[Tooltip("Percentage of damage to take when enemy is attacked by player. Ignore if using another damage system")]
	public float enemyTakeDamageValue = 25f;
	[Tooltip("Percentage of damage to deduct from player when attacked by enemy (once for each attack strike). Ignore if using another damage system")]
	public float playerTakeDamageValue = 10f;

	[Tooltip("If or not the enemy has a death animation.")]
	public bool hasDeathAnim = false;
	[Tooltip("Items to be spawned on enemy death. These may include ragdolls, death effects etc.")]
	public GameObject[] deathSpawnItems;
	[Tooltip("Time taken before spawned items disappear.")]
	public float delayBeforeDestroy = 3f;
	public static float delayB4Destroy;

	[Space (20)]
	[Header ("Visuals & Performance")]
	[Tooltip("")]
	public bool drawRays = true;
	public bool drawVisionCone = true;
	public bool drawHearingRadius = true;
	public bool drawOverlapSphere = false;
	[Tooltip("Turn off enemy radar when player is away by this value (save performance)" +
		"if set value is less than both sight distance and hearing radius of this agent, " +
		"the set value will be ignored since radar should not be off if player is within sight or hearing distance")]
	public float switchOffRadarDistance = 30;

	void Awake () {

		if(player == null)
			player = GameObject.Find ("Player");
		if(player == null)
			player = GameObject.FindWithTag ("Player");

		BroadcastMessage ("setPlayer", player ,SendMessageOptions.DontRequireReceiver);

		retreatDistance = sightDistance + (sightDistance/3);

		delayB4Destroy = delayBeforeDestroy;
//		if (bullet == null) {
//			bullet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
//			bullet.transform.SetParent (transform);
//			bullet.AddComponent<Rigidbody> ();
//			bullet.transform.localPosition = new Vector3 (0, 0, 0);
//			bullet.transform.localScale = new Vector3 (transform.localScale.x / 10, transform.localScale.y / 10, transform.localScale.z / 10);
//			bullet.name = "bullet";
//			bullet.AddComponent<Bullet> ();
//		}
	}

	void Update(){
		retreatDistance = sightDistance + (sightDistance/3);
		delayB4Destroy = delayBeforeDestroy;
	}
}

//Not required anymore
[System.Serializable]
public class AlternateAnimations {
	public AnimationClip altWalkAnimation;
	public AnimationClip altRunAnimation;
	public AnimationClip altAimAnimation;
	public AnimationClip altShootAnimation;
	public AnimationClip altIdleAnimation;
}