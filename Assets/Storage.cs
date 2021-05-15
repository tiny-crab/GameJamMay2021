using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Linq;
using UnityEngine.UI;
using UniRx;

public class Storage : MonoBehaviour
{
    public Datastore datastore;
    public GameObject storageCardPrefab;
    public GameObject layoutGroupObject;

    public Dictionary<CropType, GameObject> visibleCards = new Dictionary<CropType, GameObject>();

    void Start()
    {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        storageCardPrefab = datastore.prefabManager.cropStorageCardPrefab;
        layoutGroupObject = GameObject.Find("Canvas")
            .Children().Where(child => child.gameObject.name == "StorageScrollview").Single()
            .Descendants().Where(desc => desc.gameObject.name == "Content").Single();

        datastore.mouseController.harvestedCrops.Subscribe(_ => fillStorage());
    }

    void fillStorage() {
        datastore.storage
            .Where(entry => !visibleCards.Keys.Contains(entry.Key))
            .Where(entry => entry.Value.Value > 0).ToList()
            .ForEach(entry => {
                var cropCard = GameObject.Instantiate(storageCardPrefab, layoutGroupObject.transform);

                visibleCards[entry.Key] = cropCard;
                cropCard.transform.localScale = new Vector3(.25f, .25f, 1);
                cropCard.transform.Find("CropIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>(entry.Key.getSpritePath(entry.Key.spritePathCount));
                entry.Value.Subscribe(quantity => {
                    cropCard.transform.Find("Quantity").GetComponent<Text>().text = quantity.ToString();
                });

        });
    }
}
