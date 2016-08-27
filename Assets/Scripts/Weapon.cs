using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Weapon : MonoBehaviour {

	public GameObject bulletPrefab;
	public float bulletSpeed = 6.0f;
	public float bulletLifetime = 2.0f;
	public float gunCooldown = 0.1f;
	public float laserCooldown = 1.0f;
	public gunTypes gunType = Bullet;
	private Transform nozzle;
	private bool cooldown;
	private enum gunTypes {bullet, laser};
	void Start() {
		nozzle = transform.FindChild("Nozzle");
	}

	IEnumerator Fire() {
		if (cooldown) {
			yield break;
		}
		cooldown = true;
		switch (gunType) {
			case gunTypes.bullet:
				var bullet = (GameObject)Instantiate (
					bulletPrefab,
					nozzle.position,
					nozzle.rotation);
				bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * bulletSpeed;
				NetworkServer.Spawn (bullet);
				Destroy (bullet, bulletLifetime);
				yield return new WaitForSeconds (gunCooldown);
				cooldown = false;
				break;
		case gunTypes.laser:
			cooldown = true;
			Vector3 fwd = nozzle.TransformDirection(Vector3.forward);
			RaycastHit rHit;
			Debug.DrawRay (nozzle.position, fwd, Color.magenta, 0.2f);
			if (Physics.Raycast (nozzle.position, fwd, out rHit, 100.0f)) {
				if (rHit.collider.tag == "pawn") {
					var hit = rHit.transform.gameObject;
					var health = hit.GetComponent<Health>();
					if (health) {
						health.TakeDamage(10);
					}
				}
			}
			yield return new WaitForSeconds (laserCooldown);
			cooldown = false;
			break;
		}

	}
}
