using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenTile : MonoBehaviour {
    public Crop crop;
    public int x;
    public int y;

    public void harvest() {
        Debug.Log("Destroying Crop");
        if (crop != null) {
            Destroy(crop.GetComponent<GameObject>());
            crop = null;
        }
    }
}
