using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class coinCollect : MonoBehaviour {
    public Tilemap tilemap; // Referenz auf die Tilemap, die die M�nzen enth�lt
    public TileBase goldCoin; // Die Tile, die bei Ber�hrung entfernt werden soll
    public TileBase copperCoin; // Die Tile, die bei Ber�hrung entfernt werden soll
    public TileBase SilverCoin; // Die Tile, die bei Ber�hrung entfernt werden soll

    private void OnTriggerStay2D(Collider2D other) {

        // �berpr�fe, ob der Trigger mit dem Spieler kollidiert ist
        if (other.CompareTag("Player"))
        {
            // Ermittle die Tile-Koordinaten des Spieler-Objekts in der Tilemap
            Vector3Int cellPosition = tilemap.WorldToCell(other.transform.position);

            // �berpr�fe, ob sich auf dieser Tile ein M�nz-Tile befindet
            if (tilemap.GetTile(cellPosition) == goldCoin || tilemap.GetTile(cellPosition) == copperCoin || tilemap.GetTile(cellPosition) == SilverCoin)
            {
                // Entferne die Tile aus der Tilemap
                tilemap.SetTile(cellPosition, null);
                CoinManager.AddMoney();
                FindObjectOfType<AudioManager>().Play("coin");
            }
        }
    }
}