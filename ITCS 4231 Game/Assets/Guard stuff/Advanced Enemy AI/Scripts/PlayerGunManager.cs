using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class PlayerGunManager : MonoBehaviour
{
	private GameObject bullet;
	public float bulletForce = 2000.0f;
	public Transform spawnpoint;
	AIData info;
	public GameObject mBullet;

	void Awake ()
	{
		spawnpoint = transform;
	}

	void Start ()
	{

		if (bullet == null) {
			mBullet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			mBullet.transform.SetParent (transform);
			mBullet.AddComponent<Rigidbody> ();
			mBullet.transform.localPosition = new Vector3 (0, 0, 0);
			mBullet.transform.localScale = new Vector3 (transform.localScale.x / 10, transform.localScale.y / 10, transform.localScale.z / 10);
			mBullet.name = "bullet";
			mBullet.AddComponent<Bullet> ();
			bullet = mBullet;
		}

		info = transform.parent.parent.GetComponent <AIData> ();
//		bullet = info.bullet;
//		bullet.gameObject.AddComponent <Bullet> ();
	}

	void Update ()
	{
		if (Input.GetButtonUp ("Fire1")) {
			GameObject shot;
			shot = Instantiate (bullet, spawnpoint.position, transform.rotation);
			shot.transform.SetParent (transform);
			shot.GetComponent<Rigidbody> ().AddForce (transform.forward * bulletForce);
//			shot.gameObject.AddComponent<Bullet> ();
		}
	}

	void shootPlayer(){

	}
}
