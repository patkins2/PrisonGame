using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

	private AIData info;
	private float takeDamageValue;
	private float damagePlayerValue;

	void Start () {
		info = transform.parent.parent.parent.GetComponent <AIData>();
		Destroy (gameObject, 5f);

//		takeDamageValue = info.takeDamageValue;
//		damagePlayerValue = info.damagePlayerValue;
	}

	void setPlayer(GameObject player){

	}

	void OnCollisionEnter (Collision col)
	{
			Debug.Log ("Bullet collided with " + col.gameObject.name);
			col.gameObject.BroadcastMessage ("playerTakeDamage", damagePlayerValue, SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerEnter (Collider col)
	{
			Debug.Log ("Bullet collided with " + col.gameObject.name);
		col.gameObject.BroadcastMessage ("ApplyDamage", damagePlayerValue, SendMessageOptions.DontRequireReceiver);
	}

}
