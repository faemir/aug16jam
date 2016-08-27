using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public GameObject enemyPrefab;
    public int numberOfEnemies;

    public override void OnStartServer() {
        for (int i = 0; i < numberOfEnemies; i++) {
            var spawnPosition = new Vector3(
                Random.Range(-8f,8f),
                0f,
                Random.Range(-8f,8f));
            var spawnRotation = Quaternion.Euler(
                0f,
                Random.RandomRange(0,180),
                0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }

}
