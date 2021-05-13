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

    public Dictionary<CropType, GameObject> inventoryCards = new Dictionary<CropType, GameObject>();
    public ReactiveProperty<GameObject> selectedCard = new ReactiveProperty<GameObject>();

    void clickInventoryCard(GameObject cropCard, CropType cropType) {
        selectedCard.SetValueAndForceNotify(cropCard);
        datastore.mouseController.holdShape(cropType);
        datastore.mouseState.Value = (int) MouseState.PLANTING;
    }

    void fillInventory() {
        inventoryCards.Values.ToList().ForEach(card => card.SetActive(true));
        datastore.seedInventory
            .Where(entry => !inventoryCards.Keys.Contains(entry.Key))
            .OrderByDescending(entry => entry.Key.name).ToList()
            .ForEach(entry => {
                var cropCard = Object.Instantiate(cropInventoryCardPrefab, layoutGroupObject.transform);

                inventoryCards[entry.Key] = cropCard;
                cropCard.transform.localScale = new Vector3(.25f, .25f, 1);
                cropCard.transform.Find("CropIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>(entry.Key.getSpritePath(entry.Key.spritePathCount));
                cropCard.transform.Find("DurationText").GetComponent<Text>().text = $"{entry.Key.turnsToHarvest}";
                entry.Key.shapeType.Subscribe(shapeType => {
                    cropCard.transform.Find("ShapeIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>(ShapeTypeImages.ShapeTypeToSpriteLocation[(ShapeType) shapeType]);
                });

                var selectedIndicators = cropCard.transform.Find("SelectedIndicators").gameObject.Children();
                selectedIndicators.ForEach(indicator => indicator.SetActive(false));
                selectedCard.Subscribe(selectedCard => {
                    selectedIndicators.ForEach(indicator => indicator.SetActive(selectedCard == cropCard));
                });

                var button = cropCard.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => clickInventoryCard(cropCard, entry.Key));
        });
    }

    void Start() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        cropInventoryCardPrefab = datastore.prefabManager.cropInventoryCardPrefab;
        fillInventory();
    }
}
