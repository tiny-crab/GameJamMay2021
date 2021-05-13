using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class HoverInfo : MonoBehaviour
{
    public Datastore datastore;

    public BoolReactiveProperty hoveredObjectChanged;
    private GameObject hoveredObject;
    private Text textComponent;

    void Awake() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        textComponent = GetComponent<Text>();
    }
    void Start()
    {
        datastore.turnCount.Subscribe(_ => {
            GameObject tempObj = hoveredObject;
            hoveredObject = null;
            setHoveredObject(tempObj);
        });
    }

    public void setHoveredObject(GameObject obj) {
        if (obj == hoveredObject) {
            return;
        } else if (obj == null) {
            setText("");
        } else if (obj.GetComponent<GardenTile>() != null) {
            string textString = "";
            GardenTile tile = obj.GetComponent<GardenTile>();
            if (!tile.tilled) {
                textString = "Untilled Garden Tile";
            } else if (tile.crop == null) {
                textString = "A Tilled Garden Tile Ready For Planting";
            } else if (tile.crop != null && tile.crop.isMature) {
                textString = $"{tile.crop.cropType.name} - Click to Harvest";
            } else if (tile.crop != null && !tile.crop.isMature) {
                textString = $"{tile.crop.cropType.name} - {tile.crop.turnsUntilMature} Turns Until Harvestable";
            }
            setText(textString);
        }
        hoveredObject = obj;
    }

    private void setText(string text) {
        textComponent.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
