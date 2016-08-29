using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeaponLauncher : NetworkBehaviour {

    public GameObject projectilePrefab;

    // Three kinds of projectiles:
    //  a) Slow moving, damage mechanism is OnCollision  (e.g. missles)
    //      networkSpawnProjectile = true
    //      physics simulation is not synced (NetworkTransform Send Rate = 0)
    //      OnCollision on server has authority
    //  b) Fast moving, damage mechanism is OnCollision  (e.g. bullets)
    //      networkSpawnProjectile = true
    //      physics simulation should probably be synced? (TODO)
    //      OnCollision on server has authority
    //  c) Hitscan, damage mechanism is raycast
    //      networkSpawnProjectile = false
    public bool networkSpawnProjectile;
    public float rateOfFire = 1.0f;
    public float maxRaycastRange = 1000.0f;
    public float equipTime = 0.1f;              // Time to wait after equipping before firing


    private Transform _nozzle;
    private bool _firing = false;
    private bool _ready = false;

    void Start() {
        _nozzle = transform.FindChild("Nozzle");
    }

    IEnumerator OnEquip() {
        yield return new WaitForSeconds(equipTime);
        _ready = true;
    }

    void OnDrop() {
        _ready = false;
    }

    // Authorative fire command
    [Server]
    void Fire() {
        if (!_firing && _ready) {
            StartCoroutine(FireRoutine());
        }
    }



    IEnumerator FireRoutine() {
        _firing = true;
        RpcFire();
        var projectileObj = (GameObject)Instantiate(
                projectilePrefab,
                _nozzle.position,
                _nozzle.rotation);
        projectileObj.GetComponent<IProjectile>().ServerFire();
        if (networkSpawnProjectile) {
            NetworkServer.Spawn(projectileObj);
        }
        yield return new WaitForSeconds(1.0f / rateOfFire);
        _firing = false;
    }

    // Client visual effects
    [ClientRpc]
    void RpcFire() {
        if (!networkSpawnProjectile) {
            var projectileObj = (GameObject)Instantiate(
                projectilePrefab,
                _nozzle.position,
                _nozzle.rotation);
            projectileObj.GetComponent<IProjectile>().ClientFire();
        }
        // Can also do muzzle flash effects here
    }
}
