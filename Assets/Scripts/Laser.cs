using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	// Use this for initialization
	void Start () {
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		RaycastHit rHit;
		Debug.DrawRay (transform.position, fwd, Color.magenta, 0.2f);
		if (Physics.Raycast (transform.position, fwd, out rHit, 100.0f)) {
			if (rHit.collider.tag == "pawn") {
				var hit = rHit.transform.gameObject;
				var health = hit.GetComponent<Health>();
				if (health) {
					health.TakeDamage(10);
				}
			}
		}
	}
}
