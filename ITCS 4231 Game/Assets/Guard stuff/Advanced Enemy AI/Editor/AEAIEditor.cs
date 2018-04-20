using UnityEditor;
using UnityEngine;
using NUnit.Framework.Internal.Execution;

public class AEAIEditor : EditorWindow {

	[MenuItem("Tools/AEAI/Create AI")]
	public static void AddEnemyAI(){
		foreach (GameObject go in Selection.gameObjects){
			if (go.GetComponent<AEAIIdentifier> () == null) {
				GameObject enemy = GameObject.CreatePrimitive (PrimitiveType.Cube);
				enemy.name = "AIEnemy";
				enemy.transform.position = go.transform.position;

				MeshRenderer rend = enemy.GetComponent <MeshRenderer> ();;
				DestroyImmediate (rend);
				BoxCollider bc = enemy.GetComponent <BoxCollider> ();
				DestroyImmediate (bc);
				MeshFilter mf = enemy.GetComponent <MeshFilter> ();
				DestroyImmediate (mf);

//				[DisallowMultipleComponent()]
				enemy.AddComponent<AEAIIdentifier> ();
				enemy.AddComponent<AIData> ();

				go.AddComponent <Animation> ();
				go.AddComponent <EnemyAI> ();
				go.transform.SetParent (enemy.transform);
			} else 
				Debug.Log ("Selected GameObject already has AI");
		}
	}

	[MenuItem("Tools/AEAI/Add New Waypoint")]
	public static void createWaypoint(){
		WaypointCreator wp = new WaypointCreator ();
		foreach (GameObject go in Selection.gameObjects){
			if(go.GetComponent<AEAIIdentifier> () != null)
				wp.createNewWaypoint (go);
			else
				Debug.Log ("No AI on selected GameObject. Create AI first");
		}
	}

//	[MenuItem("AEAI/Add Player Shoot Manager")]
//	public static void addPlayerShootManager(){
//
//		//creating a gun manager
//		foreach (GameObject go in Selection.gameObjects) {
//			GameObject gunManager = new GameObject ();
//			gunManager.transform.SetParent (go.transform);
//			gunManager.transform.localPosition = new Vector3 (0, (go.transform.position.y / 2), 0);
//			gunManager.transform.localScale = new Vector3 (go.transform.localScale.x / 50, go.transform.localScale.y / 50, go.transform.localScale.z / 50);
//			gunManager.name = "gunManager";
//			gunManager.AddComponent<PlayerGunManager> ();
//		}
//	}

//	[ContextMenuItem("boo","foo")]
//	public static void boo(){
//
//	}

	void OnGui(){

	}
}
