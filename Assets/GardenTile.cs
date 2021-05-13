using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenTile : MonoBehaviour {

    public Datastore datastore;
    public Crop crop;
    public bool tilled;
    public int x;
    public int y;

    private GameObject tilledSoil;

    void Awake() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
    }

    public Crop grab() {
        if (crop != null && crop.isMature) {
            return crop;
        } else {
            return null;
        }
    }

    public void harvest() {
        Destroy(crop.gameObject);
        crop = null;
        if (tilledSoil != null) {
            tilledSoil.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Crops/soil_04");
        }
    }

    public void till() {
        tilled = true;
        tilledSoil = GameObject.Instantiate(datastore.prefabManager.tilledSoilPrefab, this.transform);
        tilledSoil.transform.position = new Vector3(tilledSoil.transform.position.x, tilledSoil.transform.position.y - 0.3f, 0);
        tilledSoil.GetComponent<SpriteRenderer>().sortingOrder = 5;
        if ((x + y) % 2 == 0) {
            this.GetComponent<SpriteRenderer>().color = datastore.colors["DARK_GROUND"];
        } else {
            this.GetComponent<SpriteRenderer>().color = datastore.colors["GROUND"];
        }
    }
}
