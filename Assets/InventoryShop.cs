using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Linq;
using UnityEngine.UI;

public class InventoryShop : MonoBehaviour
{
    public Datastore datastore;

    public GameObject cropCardPrefab;
    public GameObject layoutGroupObject;

    void ClickCard(CropType crop) {
        int currentCount;
        datastore.SeedInventory.TryGetValue(crop, out currentCount);
        datastore.SeedInventory[crop] = currentCount + 1;
        Debug.Log($"Purchased {crop.name} for total of {datastore.SeedInventory[crop]} seeds.");
    }

    // Start is called before the first frame update
    void Start()
    {
        cropCardPrefab = (GameObject) Resources.Load("Prefabs/UI/CropCard");
        datastore.storeInventory.ForEach(cropType => {
            var cropCard = Object.Instantiate(cropCardPrefab, layoutGroupObject.transform);
            cropCard.transform.localScale = new Vector3(.25f, .25f, 1);
            cropCard.transform.Find("SelectedIndicators").gameObject.Children().ForEach(indicator => indicator.SetActive(false));
            cropCard.transform.Find("DurationText").GetComponent<Text>().text = $"{cropType.turnsToHarvest}";
            cropCard.transform.Find("BuyText").GetComponent<Text>().text = $"${cropType.buyPrice}";
            var button = cropCard.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => ClickCard(cropType));
        });
    }
}
