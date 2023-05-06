using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessGameManager : MonoBehaviour {
    [SerializeField] GameObject[] tilesDifficult1;
    [SerializeField] GameObject[] tilesDifficult2;
    [SerializeField] GameObject[] tilesDifficult3;
    [SerializeField] GameObject[] tilesDifficult4;
    [SerializeField] GameObject[] tilesDifficult5;
    [SerializeField] float spawnDistanceThreshold = 20f;
    [SerializeField] Transform player;
    public Text meterAnzeige;
    public GameObject zone;

    private List<GameObject> spawnedTiles = new List<GameObject>();
    private float nextSpawnPosition = 4f;

    private int hightScore = 0;

    void Start() {
        SpawnTile();
    }

    public float moveSpeed = 5f; // die Geschwindigkeit der Bewegung

    void Update() {

        if (player != null)
        {

            // aktuelle Position des Objekts speichern
            Vector3 currentPosition = zone.transform.position;

            // neue Position berechnen (x-Bewegung basierend auf Spielerposition, y-Position beibehalten)
            float newX = player.transform.position.x + 2f; // füge 2 zur x-Position des Spielers hinzu, um die Bewegung zu bestimmen
            Vector3 newPosition = new Vector3(newX, player.position.y, currentPosition.z);

            float newMoveSpeed = moveSpeed;

            float distance = Mathf.Abs(zone.transform.position.x - player.transform.position.x) - 21.3f;

            if (distance > 25)
            {
                newMoveSpeed *= 3;
            }

            // das Objekt zu seiner neuen Position bewegen
            zone.transform.position = Vector3.MoveTowards(currentPosition, newPosition, newMoveSpeed * Time.deltaTime);

            if ((int)player.position.x - 4 > hightScore)
            {
                hightScore = (int)player.position.x - 4;

                if(PlayerPrefs.GetInt("livetimeHighScore", 0) < hightScore)
                {
                    PlayerPrefs.SetInt("livetimeHighScore", hightScore);
                }

                if(hightScore == 250)
                {
                    System.Array.Copy(tilesDifficult2, 0, tilesDifficult1, 0, tilesDifficult2.Length);
                }

                if(hightScore == 750)
                {
                    System.Array.Copy(tilesDifficult3, 0, tilesDifficult1, 0, tilesDifficult3.Length);
                }

                if(hightScore == 1500)
                {
                    System.Array.Copy(tilesDifficult4, 0, tilesDifficult1, 0, tilesDifficult4.Length);
                }

                if(hightScore == 2500)
                {
                    System.Array.Copy(tilesDifficult5, 0, tilesDifficult1, 0, tilesDifficult5.Length);
                }

            }

            meterAnzeige.text = hightScore + " Meter";

            if (player.position.x >= nextSpawnPosition - spawnDistanceThreshold)
            {
                SpawnTile();
            }

            foreach (var tile in spawnedTiles)
            {
                if (tile.transform.Find("EndPoint").position.x < -10f)
                {
                    Destroy(tile);
                }
            }


        }
        
    }

    void SpawnTile() {
        int index = Random.Range(0, tilesDifficult1.Length);
        GameObject tile = Instantiate(tilesDifficult1[index], new Vector3(nextSpawnPosition, 0f, 0f), Quaternion.identity);
        nextSpawnPosition = tile.transform.Find("StartPoint").position.x + (tile.transform.Find("EndPoint").position.x - tile.transform.Find("StartPoint").position.x);
        spawnedTiles.Add(tile);

        if (spawnedTiles.Count > 10)
        {
            GameObject oldTile = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            Destroy(oldTile);
        }
    }
}