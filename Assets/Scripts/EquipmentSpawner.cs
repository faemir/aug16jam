using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EquipmentSpawner:NetworkBehaviour {

    public GameObject[] equipment;

    public override void OnStartServer() {
        var index = Random.Range(0, equipment.Length-1);
        var item = (GameObject)Instantiate(equipment[index], transform.position, transform.rotation);
        NetworkServer.Spawn(item);
    }


    void OnDrawGizmos() {
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}