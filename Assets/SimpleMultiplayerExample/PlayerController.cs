using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

   
    public float xSpeed = 2.0f;
    public float zSpeed = 2.0f;
	public Camera playerCam;


    private Transform body;
    private Transform gun;
	private float rotationY = 0.0f;
	private float rotationX = 0.0f;


    public override void OnStartLocalPlayer() {
        body = transform.FindChild("Body");
        body.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    void Start() {
        gun = transform.FindChild("Gun");
		Cursor.visible = false;
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer) return; // only update on local client (object owner)

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * xSpeed;
        var z = Input.GetAxis("Vertical") *  Time.deltaTime * zSpeed;
        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);
		rotationX += Input.GetAxis ("Mouse X") * 5.0f;
		rotationY += Input.GetAxis ("Mouse Y") * 5.0f;
		transform.eulerAngles = new Vector3 (-rotationY, rotationX, 0.0f);
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
