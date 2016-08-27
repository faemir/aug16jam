using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Weapon_laser : MonoBehaviour {
	public float laserCooldown = 1.0f;
	private Transform nozzle;
	private bool cooldown;
	public LineRenderer LaserLR;

	void Start() {
		nozzle = transform.FindChild("Nozzle");
		cooldown = false;
		LaserLR.SetVertexCount(2);
		LaserLR.SetWidth(0.5f, 0.5f);
		LaserLR.enabled = false;
	}

	IEnumerator Fire() {
		if (cooldown) {
			yield break;
		}
		cooldown = true;
		Vector3 fwd = nozzle.TransformDirection(Vector3.forward);
		RaycastHit rHit;
		//Debug.DrawRay (nozzle.position, fwd * 100.0f, Color.magenta, 2.0f);

		LaserLR.enabled = true;
		LaserLR.SetPosition(0, nozzle.position);
		LaserLR.SetPosition(1, fwd * 100.0f);

		if (Physics.Raycast (nozzle.position, fwd, out rHit, 100.0f)) {
				var hit = rHit.transform.gameObject;
				var health = hit.GetComponent<Health>();
				if (health) {
					health.TakeDamage(50);
				}
		}
		yield return new WaitForSeconds (laserCooldown);
		cooldown = false;
	}
}
