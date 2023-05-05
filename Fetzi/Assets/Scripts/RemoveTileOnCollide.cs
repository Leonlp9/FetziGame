using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RemoveTileOnCollide : MonoBehaviour {
    public Tilemap coinTilemap;

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision");
        // Find the tile coordinate of the collision point
        Vector3Int cellPosition = coinTilemap.WorldToCell(collision.transform.position);

        // Remove the tile at the coordinate
        coinTilemap.SetTile(cellPosition, null);
    }
}