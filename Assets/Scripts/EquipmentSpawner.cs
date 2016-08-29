using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EquipmentSpawner:NetworkBehaviour {

    public GameObject[] equipment;

    public override void OnStartServer() {
        var index = Random.Range(0, equipment.Length);
        var item = (GameObject)Instantiate(equipment[index], transform.position, transform.rotation);
        NetworkServer.Spawn(item);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
    }
}