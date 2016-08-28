using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// Players equip items by flying into them when they don't already have an item equipped
public class Equippable : NetworkBehaviour {

    public Vector3 equippedLocalPosition;
    public Vector3 equippedLocalRotation;
    public bool equipped {
        get {
            return _equipped;
        }
    }

    [SyncVar]
    private bool _equipped;
    private PlayerController _owner;
    private Collider _trigger;
    private ParticleSystem _particles;
    

    public void Awake() {
        _trigger = GetComponent<Collider>();
        _particles = GetComponent<ParticleSystem>();
    }

    // Server authority
    [Command]
    public void CmdEquip(NetworkInstanceId parentId) {
        if (_equipped) return;
        _equipped = true;
        RpcEquip(parentId);
    }
    [Command]
    public void CmdDrop() {
        _equipped = false;
        RpcDrop();
    }

    [ClientRpc]
    void RpcEquip(NetworkInstanceId parentId) {
        _owner = ClientScene.FindLocalObject(parentId).GetComponent<PlayerController>();
        transform.parent = _owner.transform;
        _owner.equippedItem = this;
        transform.localPosition = equippedLocalPosition;
        transform.localRotation = Quaternion.Euler(equippedLocalRotation);
        _trigger.enabled = false;
        _particles.Stop();
    }

    [ClientRpc]
    void RpcDrop() {
        StartCoroutine(Drop());
    }
  
    IEnumerator Drop() {
        transform.parent = null;
        _owner.equippedItem = null;
        _owner = null;
        yield return new WaitForSeconds(1.0f);
        _trigger.enabled = true;
        _particles.Play();
    }
}
