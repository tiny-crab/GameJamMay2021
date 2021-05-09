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
        Debug.Log($"Clicked button {crop.name}");
    }

    // Start is called before the first frame update
    void Start()
    {
        cropCardPrefab = (GameObject) Resources.Load("Prefabs/UI/CropCard");
        datastore.storeInventory.ForEach(seedType => {
            var cropCard = Object.Instantiate(cropCardPrefab, layoutGroupObject.transform);
            cropCard.transform.localScale = new Vector3(.25f, .25f, 1);
            cropCard.transform.Find("SelectedIndicators").gameObject.Children().ForEach(indicator => indicator.SetActive(false));
            var button = cropCard.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => ClickCard(seedType));
        });
    }
}
