using UnityEngine;

public class TimedDestroy : MonoBehaviour {
	float t = AIData.delayB4Destroy;

	void Start () {
		Destroy (gameObject, t);
	}

	void Update () {
		
	}
}
