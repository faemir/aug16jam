using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gun : MonoBehaviour {

    public GameObject bulletPrefab;
    public float bulletSpeed = 6.0f;
    public float bulletLifetime = 2.0f;
    public float gunCooldown = 0.1f;
    private Transform nozzle;
    private bool cooldown;
    void Start() {
        nozzle = transform.FindChild("Nozzle");
    }

    IEnumerator Fire() {
        if (cooldown) {
            yield break;
        }
        cooldown = true;
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            nozzle.position,
            nozzle.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, bulletLifetime);
        yield return new WaitForSeconds(gunCooldown);
        cooldown = false;
    }
}
