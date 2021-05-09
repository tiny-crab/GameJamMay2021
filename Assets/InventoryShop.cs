using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Linq;

public class InventoryShop : MonoBehaviour
{

    public GameObject cropCardPrefab;
    public GameObject layoutGroupObject;

    // Start is called before the first frame update
    void Start()
    {
        cropCardPrefab = (GameObject) Resources.Load("Prefabs/UI/CropCard");
        for(var i = 0; i < 6; i++) {
            var cropCard = Object.Instantiate(cropCardPrefab, layoutGroupObject.transform);
            cropCard.transform.localScale = new Vector3(.25f, .25f, 1);
            cropCard.transform.Find("SelectedIndicators").gameObject.Children().ForEach(indicator => indicator.SetActive(false));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
