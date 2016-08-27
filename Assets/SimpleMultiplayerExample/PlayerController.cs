using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

   
    public float moveSpeed = 2.0f;
    public float rotateSpeed = 5.0f;

    private Transform body;
    private Transform head;
    private GameObject weapon;
	private float rotationY = 0.0f;
	private float rotationX = 0.0f;
    private Vector3 movementCommand;


    void Start() {
        head = transform.FindChild("Head");
        // TODO(MDB): Make weapons droppable and equippable
        weapon = head.transform.FindChild("Gun").gameObject;
		Cursor.visible = false;
        if (isLocalPlayer) {
            body = transform.FindChild("Body");
            body.GetComponent<MeshRenderer>().material.color = Color.blue;
            head.FindChild("Camera").gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer) return; // only update on local client (object owner)

        var x = Input.GetAxis("Horizontal") * Time.deltaTime;
        var z = Input.GetAxis("Vertical") *  Time.deltaTime;
        movementCommand = (transform.forward * z + transform.right * x).normalized * moveSpeed;
		rotationX += Input.GetAxis ("Mouse X") * rotateSpeed;
		rotationY += Input.GetAxis ("Mouse Y") * rotateSpeed;
        transform.eulerAngles = new Vector3 (-rotationY, rotationX, 0.0f);
        if(Input.GetButton("Fire1")) {
            CmdFire();
        }
	}

    void FixedUpdate() {
        GetComponent<Rigidbody>().AddForce(movementCommand);
    }

    // Execute on server
    [Command]
    void CmdFire() {
        weapon.SendMessage("Fire");
    }
}
