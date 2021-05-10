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

    public GameObject cropShopCardPrefab;
    public GameObject cropInventoryCardPrefab;
    public bool inventoryOpen = false;
    public Button inventoryToggleButton;
    public GameObject layoutGroupObject;

    public Dictionary<CropType, GameObject> shopCards = new Dictionary<CropType, GameObject>();
    public Dictionary<CropType, GameObject> inventoryCards = new Dictionary<CropType, GameObject>();

    void clickShopCard(GameObject cropCard, CropType crop) {
        if (datastore.seedInventory.Keys.Contains(crop)) {
            var currentCount = datastore.seedInventory[crop].Value;
            datastore.seedInventory[crop].SetValueAndForceNotify(currentCount + 1);
        } else {
            datastore.seedInventory[crop] = new IntReactiveProperty(1);
        }
        Debug.Log($"Purchased {crop.name} for total {datastore.seedInventory[crop].Value} seeds.");
    }

    void clickInventoryCard(GameObject cropCard, CropType crop) {
        if (datastore.seedInventory.Keys.Contains(crop)) {
            var currentCount = datastore.seedInventory[crop].Value;
            if (currentCount > 0) {
                datastore.seedInventory[crop].SetValueAndForceNotify(currentCount - 1);
                Debug.Log($"Planted {crop.name} with only {datastore.seedInventory[crop].Value} seeds remaining.");
            }
        }
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
                cropCard.transform.Find("CropIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>(entry.Key.spritePaths.Last());
                cropCard.transform.Find("SelectedIndicators").gameObject.Children().ForEach(indicator => indicator.SetActive(false));
                cropCard.transform.Find("DurationText").GetComponent<Text>().text = $"{entry.Key.turnsToHarvest}";
                entry.Value.SubscribeToText(cropCard.transform.Find("QuantityText").GetComponent<Text>(), quant => $"x{quant}");
                entry.Value.Subscribe(quant => {
                    cropCard.transform.Find("QuantityText").GetComponent<Text>().color = quant == 0 ? Color.red : Color.black;
                });


                var button = cropCard.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => clickInventoryCard(cropCard, entry.Key));
        });
    }

    void fillShop() {
        shopCards.Values.ToList().ForEach(card => card.SetActive(true));
        datastore.storeInventory
            .Where(cropType => !shopCards.Keys.Contains(cropType))
            .OrderByDescending(cropType => cropType.name).ToList()
            .ForEach(cropType => {
                var cropCard = Object.Instantiate(cropShopCardPrefab, layoutGroupObject.transform);
                shopCards[cropType] = cropCard;
                cropCard.transform.localScale = new Vector3(.25f, .25f, 1);
                cropCard.transform.Find("CropIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>(cropType.spritePaths.Last());
                cropCard.transform.Find("SelectedIndicators").gameObject.Children().ForEach(indicator => indicator.SetActive(false));
                cropCard.transform.Find("DurationText").GetComponent<Text>().text = $"{cropType.turnsToHarvest}";
                cropCard.transform.Find("BuyText").GetComponent<Text>().text = $"${cropType.buyPrice}";
                var button = cropCard.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => clickShopCard(cropCard, cropType));
        });
    }

    // Start is called before the first frame update
    void Start() {
        inventoryToggleButton.GetComponentInChildren<Text>().text = inventoryOpen ? "Inventory" : "Shop";
        if (inventoryOpen) { fillInventory(); } else { fillShop(); }

        inventoryToggleButton.OnClickAsObservable().Subscribe(_ => {
            inventoryOpen = !inventoryOpen;
            inventoryToggleButton.GetComponentInChildren<Text>().text = inventoryOpen ? "Inventory" : "Shop";
            if (inventoryOpen) {
                shopCards.Values.ToList().ForEach(card => card.SetActive(false));
                fillInventory();
            } else {
                inventoryCards.Values.ToList().ForEach(card => card.SetActive(false));
                fillShop();
            }
        });
    }
}
