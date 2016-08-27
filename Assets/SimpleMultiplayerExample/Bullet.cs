using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public GameObject impactEffectPrefab;

	void OnCollisionEnter(Collision collision) {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health) {
            health.TakeDamage(10);
            Instantiate(impactEffectPrefab, collision.contacts[0].point, hit.transform.rotation);
        }
        Destroy(gameObject);
    }
}
