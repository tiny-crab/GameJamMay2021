using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    public Datastore datastore;
    public GameObject gardenTilePrefab;
    public int width;
    public int height;
    public int tilledWidth;
    public int tilledHeight;

    // Start is called before the first frame update
    void Start() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        Debug.Log(datastore.prefabManager);
        Debug.Log(datastore.prefabManager.cropPrefab);
        gardenTilePrefab = datastore.prefabManager.gardenTilePrefab;
        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                GameObject tile = (GameObject) Object.Instantiate(gardenTilePrefab, this.transform);
                GardenTile gardenTile = tile.GetComponent<GardenTile>();
                gardenTile.x = x;
                gardenTile.y = y;
                if (x < tilledWidth && y < tilledHeight) {
                    gardenTile.till();
                }

                var spriteWidth = tile.GetComponent<SpriteRenderer>().bounds.size.x;
                var spriteHeight = tile.GetComponent<SpriteRenderer>().bounds.size.y;
                if ((x + y) % 2 == 0) {
                    if (gardenTile.tilled) {
                        tile.GetComponent<SpriteRenderer>().color = datastore.colors["DARK_GROUND"];
                    } else {
                        tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                    }
                } else {
                    if (gardenTile.tilled) {
                        tile.GetComponent<SpriteRenderer>().color = datastore.colors["GROUND"];
                    } else {
                        tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                    }
                }
                tile.transform.position = new Vector2((x * spriteWidth) + datastore.garden.transform.position.x, (y * spriteHeight) + datastore.garden.transform.position.y);
                tile.layer = 6;
                datastore.gardenGrid.Add(tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool checkShapeValidOnGarden(Tetromino tet, Vector2 mouseCoords) {
        for (int t = 0; t < tet.getCoordinates().Count; t++) {
            bool foundTile = false;
            Vector2 tetRelativeCoordinate = tet.getCoordinates()[t];
            Vector2 mouseRelativeCoordinate = new Vector2(tetRelativeCoordinate.x + mouseCoords.x, tetRelativeCoordinate.y + mouseCoords.y);
            for (int i = 0; i < datastore.gardenGrid.Count; i++) {
                GardenTile tile = datastore.gardenGrid[i].GetComponent<GardenTile>();
                if (tile.x == mouseRelativeCoordinate.x && tile.y == mouseRelativeCoordinate.y) {
                    foundTile = true;
                    if (tile.crop != null || !tile.tilled) {
                        return false;
                    } else {
                        if (t == tet.getCoordinates().Count - 1) {
                            return true;
                        }
                        break;
                    }
                }
            }
            //Tile wasn't found at all and coordinates do not exist on grid
            if (!foundTile) {
                return false;
            }
        }
        return true;
    }

    public void placeTiles(Tetromino tet, Vector2 mouseCoords) {
        for (int t = 0; t < tet.getCoordinates().Count; t++) {
            Vector2 tetRelativeCoordinate = tet.getCoordinates()[t];
            Vector2 mouseRelativeCoordinate = new Vector2(tetRelativeCoordinate.x + mouseCoords.x, tetRelativeCoordinate.y + mouseCoords.y);
            for (int i = 0; i < datastore.gardenGrid.Count; i++) {
                GardenTile tile = datastore.gardenGrid[i].GetComponent<GardenTile>();
                if (tile.x == mouseRelativeCoordinate.x && tile.y == mouseRelativeCoordinate.y) {
                    GameObject crop = (GameObject) Object.Instantiate(datastore.prefabManager.cropPrefab, tile.transform.position, Quaternion.identity, tile.transform);
                    crop.transform.position = new Vector3(crop.transform.position.x, crop.transform.position.y, -5);
                    crop.GetComponent<SpriteRenderer>().sortingOrder = 10;
                    Crop cropClass = crop.GetComponent<Crop>();
                    cropClass.cropType = tet.cropType;
                    tile.crop = cropClass;
                }
            }
        }
    }
}
