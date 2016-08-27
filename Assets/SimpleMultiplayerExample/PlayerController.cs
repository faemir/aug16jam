using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

   
    public float xSpeed = 150.0f;
    public float zSpeed = 3.0f;


    private Transform body;
    private Transform gun;


    public override void OnStartLocalPlayer() {
        body = transform.FindChild("Body");
        body.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    void Start() {
        gun = transform.FindChild("Gun");
        
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer) return; // only update on local client (object owner)

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * xSpeed;
        var z = Input.GetAxis("Vertical") *  Time.deltaTime * zSpeed;
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
        if(Input.GetButton("Fire1")) {
            CmdFire();
        }
	}

    // Execute on server
    [Command]
    void CmdFire() {
        gun.SendMessage("Fire");
    }
}
