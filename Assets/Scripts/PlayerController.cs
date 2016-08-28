using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    // organise variables for boosting in a class
    [System.Serializable]
    public class BoostControl {
        public float maxFuel = 2.0f;                // maximum boost time
        public float speed = 1000.0f;               // target speed while boosting
        public float cooldown = 1.0f;               // time between starting boosts
        public float rechargeRate = 1.0f;           // rate to recharge fuel
        public bool boosting { get; set; }
        public float fuel { get; set; }
        public float lastStartTime { get; set; }
        public bool ready {
            get {
                return lastStartTime + cooldown < Time.time &&
                    fuel > rechargeRate;
            }
        }
    }

    public float cruiseSpeed = 500.0f;
    public float rotateSpeed = 5.0f;
    public BoostControl boost = new BoostControl();
    public GameObject HudPrefab;
    private Transform body;
    private Transform head;
    private Transform hudAnchor;
    public Equippable equippedItem { get; set; }
    private GameObject hud;
    private float rotationY = 0.0f;
    private float rotationX = 0.0f;
    private Vector3 movementCommand;


    void Start() {
        head = transform.FindChild("Head");
        hudAnchor = transform.FindChild("HUD Anchor");
        Cursor.visible = false;
        if (isLocalPlayer) {
            body = transform.FindChild("Body");
            body.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
            head.FindChild("Camera").gameObject.SetActive(true);
            hud = (GameObject)Instantiate(HudPrefab);
            hud.GetComponent<HUD>().parent = hudAnchor.transform;
        } else {
            head.FindChild("Camera").gameObject.SetActive(false);
        }
        boost.fuel = boost.maxFuel;
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer) return; // only update on local client (object owner)

        var x = Input.GetAxis("Horizontal") * Time.deltaTime;
        var y = Input.GetAxis("Vertical") * Time.deltaTime;
        var z = Input.GetAxis("Forward") * Time.deltaTime;
        // movement is applied in physics update
        movementCommand = (
            transform.right * x +
            transform.up * y +
            transform.forward * z
            ).normalized;

        // player rotation
        rotationX += Input.GetAxis ("Mouse X") * rotateSpeed;
        rotationY += Input.GetAxis ("Mouse Y") * rotateSpeed;
        transform.eulerAngles = new Vector3 (-rotationY, rotationX, 0.0f);

        // Send the fire command to server
        if(Input.GetButton("Fire1")) {
            CmdFire();
        }
        // Drop item TODO(MDB): Change to InputGetButton
        if(Input.GetKeyDown(KeyCode.G)) {
            if (equippedItem) {
                equippedItem.CmdDrop();
                equippedItem = null;
            }
        }

        // Try to start boosting
        if (Input.GetButton("Boost") && !boost.boosting && boost.ready) {
            boost.boosting = true;
            boost.lastStartTime = Time.time;
            // BroadcastMessage("OnBoostStart") -- could do something like this to trigger sfx/vfx
        }
        // While boosting
        if (boost.boosting) {
            // If we have fuel and player still wants to boost
            if (Input.GetButton("Boost") && boost.fuel > 0.0f) {
                boost.fuel -= Time.deltaTime; // deplete fuel
            } else {
                boost.boosting = false;
                // BroadcastMessage("OnBoostStop") -- could do something like this to trigger sfx/vfx
            }
        } else {
            boost.fuel += boost.rechargeRate * Time.deltaTime;  // refuel
            boost.fuel = Mathf.Min(boost.fuel, boost.maxFuel);  // do not exceed fuel capacity
        }
        // UpdateHUD
        if (hud) {
            hud.GetComponent<HUD>().OnChangeEnergy(Mathf.FloorToInt(100.0f * boost.fuel/boost.maxFuel));
        }
	}

    // Physics update
    void FixedUpdate() {
        var rb = GetComponent<Rigidbody>();
        Vector3 force = Vector3.zero;
        if (boost.boosting) {
            force = transform.forward * rb.drag * rb.mass * boost.speed;
        } else {
            force = movementCommand * rb.drag * rb.mass * cruiseSpeed;
        }
        rb.AddForce(force);
    }

    // Execute on server
    [Command]
    void CmdFire() {
        if (equippedItem) {
            equippedItem.SendMessage("Fire");
        }
    }

    // Health change message
    void OnChangeHealth(int currentHealth) {
        if (hud) {
            hud.GetComponent<HUD>().OnChangeHealth(currentHealth);
        }
    }


} 
