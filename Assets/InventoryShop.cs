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

    void ClickCard(int index) {
        Debug.Log($"Clicked button {index}");
    }

    // Start is called before the first frame update
    void Start()
    {
        cropCardPrefab = (GameObject) Resources.Load("Prefabs/UI/CropCard");
        var indexCache = 0;
        datastore.testSeedInventory.Keys.ToList().ForEach(seedType => {
            var cropCard = Object.Instantiate(cropCardPrefab, layoutGroupObject.transform);
            cropCard.transform.localScale = new Vector3(.25f, .25f, 1);
            cropCard.transform.Find("SelectedIndicators").gameObject.Children().ForEach(indicator => indicator.SetActive(false));
            var button = cropCard.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => ClickCard(indexCache));
            indexCache++;
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
