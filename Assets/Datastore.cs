using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Datastore : MonoBehaviour
{
    public Dictionary<CropType, IntReactiveProperty> seedInventory = new Dictionary<CropType, IntReactiveProperty>();
    public List<CropType> storeInventory = new List<CropType>() {
        CropTemplates.Radish, CropTemplates.Potato
    };

    public List<GameObject> gardenGrid;

    public Garden garden;

    public MouseController mouseController;

    public Tetromino heldShape;
    public Crop heldCrop;

    public Dictionary<CropType, IntReactiveProperty> storage = new Dictionary<CropType, IntReactiveProperty>();

    public IntReactiveProperty mouseState = new IntReactiveProperty(0);

    public Dictionary<string, Color> colors = new Dictionary<string, Color>() {
        {"GREEN", new Color(125/255f, 197/255f, 94/255f)}, {"DARK_GREEN", new Color(121/255f, 191/255f, 92/255f)},
        {"GROUND", new Color(218/255f, 169/255f, 122/255f)}, {"WATER", new Color(67/255f, 151/255f, 213/255f)}
    };

    public IntReactiveProperty turnCount = new IntReactiveProperty(0);
}
