using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// Players equip items by flying into them when they don't already have an item equipped
public class Equippable : NetworkBehaviour {

    public Vector3 equippedLocalPosition;
    public Vector3 equippedLocalRotation;
    public bool equipped {
        get {
            return _owner != null;
        }
    }

    private PlayerController _owner;
    private Collider _trigger;
    private ParticleSystem _particles;

    [SyncVar(hook = "OnChangeOwnerId")]
    private NetworkInstanceId _ownerId;

    public void Awake() {
        _trigger = GetComponent<Collider>();
        _particles = GetComponent<ParticleSystem>();
        _ownerId = netId;
    }

    // Server authority
    void OnTriggerEnter(Collider collider) {
        if (equipped) return;
        if (!isServer) return;
        PlayerController player = collider.GetComponent<PlayerController>();
        if (player && !player.equippedItem) {
            _ownerId = player.netId;
        }
    }

    void OnChangeOwnerId(NetworkInstanceId ownerId) {
        // Equipped
        if (_owner == null) {
            _owner = ClientScene.FindLocalObject(ownerId).GetComponent<PlayerController>();
            if (!_owner) {
                Debug.LogError("Could not get PlayerController for equipment owner.");
            }
            if (isServer) {
                GetComponent<NetworkIdentity>().AssignClientAuthority(_owner.connectionToClient);
            }
            _owner.equippedItem = this;
            transform.parent = _owner.transform;
            transform.localPosition = equippedLocalPosition;
            transform.localRotation = Quaternion.Euler(equippedLocalRotation);
            _particles.Stop();
            _trigger.enabled = false;
            SendMessage("OnEquip", SendMessageOptions.DontRequireReceiver);
        } else {
        // Dropped
            if (isServer) {
                GetComponent<NetworkIdentity>().RemoveClientAuthority(_owner.connectionToClient);
            }
            _owner.equippedItem = null;
            _owner = null;
            transform.parent = null;
            StartCoroutine(Drop());
            SendMessage("OnDrop", SendMessageOptions.DontRequireReceiver);
        }
    }

    // Command to server from owner client
    [Command]
    public void CmdDrop() {
        _ownerId = netId;
    }

    IEnumerator Drop() {
        yield return new WaitForSeconds(1.0f);
        _trigger.enabled = true;
        _particles.Play();
    }

}
