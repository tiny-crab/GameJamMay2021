using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    public Datastore dataStore;

    public GameObject gardenTilePrefab;
    public GameObject cropPrefab;
    public int width;
    public int height;

    // Start is called before the first frame update
    void Start() {
        cropPrefab = (GameObject) Resources.Load("Prefabs/Crop");
        gardenTilePrefab = (GameObject) Resources.Load("Prefabs/GardenTile");
        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                GameObject tile = (GameObject) Object.Instantiate(gardenTilePrefab, this.transform);
                tile.GetComponent<GardenTile>().x = x;
                tile.GetComponent<GardenTile>().y = y;
                var spriteWidth = tile.GetComponent<SpriteRenderer>().bounds.size.x;
                var spriteHeight = tile.GetComponent<SpriteRenderer>().bounds.size.y;
                if ((x + y) % 2 == 0) {
                    tile.GetComponent<SpriteRenderer>().color = dataStore.colors["DARK_GREEN"];
                } else {
                    tile.GetComponent<SpriteRenderer>().color = dataStore.colors["GREEN"];
                }
                //tile.GetComponent<SpriteRenderer>().sprite = Resources.Load("Assets/Sprites/SUNNYSIDE_WORLD_CROPS_V0.01/ASSETS/soil_01.png", typeOf(Sprite));
                tile.transform.position = new Vector2((x * spriteWidth) + dataStore.garden.transform.position.x, (y * spriteHeight) + dataStore.garden.transform.position.y);
                tile.layer = 6;
                dataStore.gardenGrid.Add(tile);
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
            for (int i = 0; i < dataStore.gardenGrid.Count; i++) {
                GardenTile tile = dataStore.gardenGrid[i].GetComponent<GardenTile>();
                if (tile.x == mouseRelativeCoordinate.x && tile.y == mouseRelativeCoordinate.y) {
                    foundTile = true;
                    if (tile.crop != null) {
                        return false;
                    } else {
                        if (t == tet.getCoordinates().Count - 1) {
                            return true;
                        }
                        break;
                    }
                }
            }
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
            for (int i = 0; i < dataStore.gardenGrid.Count; i++) {
                GardenTile tile = dataStore.gardenGrid[i].GetComponent<GardenTile>();
                if (tile.x == mouseRelativeCoordinate.x && tile.y == mouseRelativeCoordinate.y) {
                    GameObject crop = (GameObject) Object.Instantiate(cropPrefab, tile.transform.position, Quaternion.identity, tile.transform);
                    crop.transform.position = new Vector3(crop.transform.position.x, crop.transform.position.y, -5);
                    Crop cropClass = crop.GetComponent<Crop>();
                    cropClass.cropType = CropTemplates.Potato;
                    tile.crop = cropClass;
                    tile.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
            }
        }
    }
}
