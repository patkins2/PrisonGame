using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointCreator : MonoBehaviour {
	
	public void createNewWaypoint(GameObject go){
		Shader mShader = Shader.Find("Standard");
		Material mMat = new Material (mShader);
		mMat.color = Color.yellow;
//		rend.material = new Material(Shader.Find("Specular"));

		GameObject waypoint = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		waypoint.transform.SetParent (go.transform);
		waypoint.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
		waypoint.transform.localPosition = new Vector3 (Random.Range (-5f, 5f), go.transform.localScale.y/4, Random.Range (-5f, 5f));
		waypoint.name = "waypoint";
		waypoint.GetComponent<Renderer> ().material = mMat;
		waypoint.GetComponent <Collider> ().isTrigger = true;
		waypoint.AddComponent <WaypointIdentifier> ();

//		go.GetComponent <EnemyAI>().patrolPoints.;
	}
}
