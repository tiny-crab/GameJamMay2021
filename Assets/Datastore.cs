using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datastore : MonoBehaviour
{
    public Dictionary<CropType, int> SeedInventory;
    public List<CropType> storeInventory = new List<CropType>() {
        CropTemplates.Radish, CropTemplates.Potato, CropTemplates.Tomato
    };

    public List<GameObject> gardenGrid;

    public Garden garden;

    public Tetromino heldShape;
}
