using UnityEngine;
using System.Collections;

public class ProjectileLaser : MonoBehaviour, IProjectile {

    public GameObject impactEffectPrefab;
    public int damage = 50;
    public float maximumRaycastRange = 1000.0f;
    public Color startColor;
    public Color endColor;
    public float fadeTime;

    private LineRenderer _lineRenderer;
    private float _startTime;

    void Awake() {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void ClientFire() {
        Fire();
    }

    public void ServerFire() {
        Fire();
    }

    void Fire() {
        _lineRenderer.SetVertexCount(2);
        _lineRenderer.SetColors(startColor, endColor);
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, maximumRaycastRange)) {
            Health health = hit.collider.gameObject.GetComponent<Health>();
            if (health) {
                health.TakeDamage(damage);
                Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
            Hit(hit.point);
        } else {
            Hit(transform.position + transform.forward * maximumRaycastRange);
        }
    }

    void Hit(Vector3 position) {
        _startTime = Time.time;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, position);
    }

    void Update() {
        // Fade out
        float lerp = (Time.time - _startTime) / fadeTime;
        Color start = Color.Lerp(startColor, Color.clear, lerp);
        Color end = Color.Lerp(endColor, Color.clear, lerp);
        _lineRenderer.SetColors(start, end);
        if (lerp > 1.0f) {
            Destroy(gameObject);
        }
    }
}
