using UnityEngine;
using System.Collections;

public class ParticleDestroyer:MonoBehaviour {

    // Use this for initialization
    void Start() {
        StartCoroutine(Lifetime());
    }

    IEnumerator Lifetime() {
        float lifetime = 0.0f;
        var particles = GetComponent<ParticleSystem>();
        if (particles) {
            lifetime = particles.duration + particles.startLifetime;
        }
        var sound = GetComponent<AudioClip>();
        if (sound) {
            lifetime = Mathf.Max(sound.length, lifetime);
        }
        yield return new WaitForSeconds(lifetime + 0.1f);
        Destroy(gameObject);
    }

}
