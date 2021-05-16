using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Linq;
using UnityEngine.UI;
using UniRx;

public class InventoryShop : MonoBehaviour
{
    public Datastore datastore;
    public GameObject cropInventoryCardPrefab;
    public GameObject layoutGroupObject;

    public Dictionary<CropType, GameObject> visibleCards = new Dictionary<CropType, GameObject>();
    public BoolReactiveProperty cardClicked = new BoolReactiveProperty(true);

    void clickInventoryCard(GameObject cropCard, CropType cropType) {
        datastore.mouseController.holdShape(cropType);
        datastore.mouseState.Value = (int) MouseState.PLANTING;
        cardClicked.Value = !cardClicked.Value;
    }

    void fillInventory() {
        datastore.possibleCrops
            .Where(cropType => !visibleCards.Keys.Contains(cropType)).ToList()
            .ForEach(entry => {
                var cropCard = Object.Instantiate(cropInventoryCardPrefab, layoutGroupObject.transform);

                visibleCards[entry] = cropCard;
                cropCard.transform.localScale = new Vector3(.2f, .2f, 1);
                cropCard.transform.Find("CropIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>(entry.getSpritePath(entry.spritePathCount));
                cropCard.transform.Find("DurationText").GetComponent<Text>().text = $"{entry.turnsToHarvest}";
                entry.shapeType.Subscribe(shapeType => {
                    cropCard.transform.Find("ShapeIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>(ShapeTypeImages.ShapeTypeToSpriteLocation[(ShapeType) shapeType]);
                });

                var selectedIndicators = cropCard.transform.Find("SelectedIndicators").gameObject.Children();

                var button = cropCard.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => clickInventoryCard(cropCard, entry));
        });
    }

    private void createTillingCard() {
        GameObject tillingCard = GameObject.Find("TillingCard");
        var button = tillingCard.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => {
            if (datastore.tillCount.Value > 0) {
                datastore.mouseState.Value = (int) MouseState.TILLING;
            }
        });

        tillingCard.transform.localScale = new Vector3(.5f, .5f, 1);

        datastore.tillCount.Subscribe(newCount => {
            tillingCard.transform.Find("TillingCount").GetComponent<Text>().text = $"{newCount}";
        });
    }

    void Start() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        cropInventoryCardPrefab = datastore.prefabManager.cropInventoryCardPrefab;
        datastore.newCropNotifier.Subscribe(_ => fillInventory());
        createTillingCard();
    }
}
