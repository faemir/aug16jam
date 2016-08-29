using UnityEngine;
using System.Collections;

public class ProjectileBullet : MonoBehaviour, IProjectile {

    public GameObject impactEffectPrefab;
    public int damage = 10;
    public float speed = 10.0f;
    public float lifetime = 10.0f;

    private Rigidbody _rb;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    public void ServerFire() {
        _rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    public void ClientFire() {

    }

	void OnCollisionEnter(Collision collision) {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health) {
            health.TakeDamage(damage);
            Instantiate(impactEffectPrefab, collision.contacts[0].point, hit.transform.rotation);
        }
        Destroy(gameObject);
    }
}
