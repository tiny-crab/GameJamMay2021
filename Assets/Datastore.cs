using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datastore : MonoBehaviour
{
    public Dictionary<CropType, int> SeedInventory;
    public Dictionary<CropType, int> testSeedInventory = new Dictionary<CropType, int>() {
        {CropTemplates.Radish, 4},
        {CropTemplates.Potato, 3}
    };

    public List<GameObject> gardenGrid;

    public Garden garden;

    public Tetromino heldShape;
}
