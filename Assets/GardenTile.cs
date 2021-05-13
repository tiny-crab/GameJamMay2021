using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenTile : MonoBehaviour {
    public Crop crop;
    public int x;
    public int y;

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
    }
}
