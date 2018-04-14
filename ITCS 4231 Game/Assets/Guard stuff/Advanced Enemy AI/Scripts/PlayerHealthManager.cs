using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {

	[SerializeField] public static float currentHealth;

	void Start () {
		currentHealth = 100f;
	}

	void Update () {
		if(currentHealth <= 0){
			killPlayer ();
		} 
//		else 
//			Debug.Log ("Player health = " + currentHealth);
	}

	void playerTakeDamage(float damageAmmount){
		currentHealth -= damageAmmount;
	}

	void killPlayer(){
		//insert player kill code
		//destroy player gameobject
		//instanciate player ragdoll etc.
		Debug.Log ("Player Dead");
	}

}
