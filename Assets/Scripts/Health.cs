using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    public int initialHealth = 100;
    public bool destroyOnDeath;
    // Sync health
    // When server tells client that currentHealth has changed, OnChangeHealthSync is executed
    [SyncVar(hook = "OnChangeHealthSync")]
    public int currentHealth;
    
    public RectTransform healthBar;

    private NetworkStartPosition[] spawnPoints;

    void Start() {
        currentHealth = initialHealth;
        if (isLocalPlayer) {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    [ClientRpc] // server tells client to respawn
    void RpcRespawn() {
        if (isLocalPlayer) { // player gameobject has localauthority on client
            Vector3 spawnPoint = Vector3.zero;
            if (spawnPoints != null && spawnPoints.Length > 0) {
                spawnPoint= spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }
            transform.position = spawnPoint;
            SendMessage("OnRespawn");
        }
    }

    public void TakeDamage(int amount) {
        if (!isServer) return;  // server has authority over currentHealth
        currentHealth -= amount;
        if (currentHealth <= 0) {
            if (destroyOnDeath) {
                Destroy(gameObject);
            } else {
                currentHealth = initialHealth;
                RpcRespawn();
            }
        }
    }

    void OnChangeHealthSync(int currentHealth) {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        SendMessage("OnChangeHealth", currentHealth);
    }


}
