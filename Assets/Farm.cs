using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public Datastore dataStore;

    public GameObject gardenTilePrefab;

    // Start is called before the first frame update
    void Start() {
        for (var x = 0; x < 4; x++) {
            for (var y = 0; y < 4; x++) {
                // gardenTilePrefab = (GameObject) Resources.Load("Prefabs/GardenTile");
                // var tile = Object.Instantiate(gardenTilePrefab, this.transform);
                // tile.GetComponent<GardenTile>().x = x;
                // tile.GetComponent<GardenTile>().y = y;
                // dataStore.garden.Add(tile);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        // dataStore.SeedInventory["Tomatoes"] = 1;
        // var tomato = CropTemplates.Tomato;
    }
}
