using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public GameObject orderPrefab;
    public GameObject customerPrefab;
    public GameObject cropPrefab;
    public GameObject gardenTilePrefab;
    public GameObject cropInventoryCardPrefab;
    public GameObject timePanelPrefab;
    public GameObject tilledSoilPrefab;
    public GameObject backgroundTile;

    // Start is called before the first frame update
    void Awake()
    {
        orderPrefab = Resources.Load<GameObject>("Prefabs/Order");
        customerPrefab = Resources.Load<GameObject>("Prefabs/Customer");
        cropPrefab = Resources.Load<GameObject>("Prefabs/Crop");
        gardenTilePrefab = Resources.Load<GameObject>("Prefabs/GardenTile");
        backgroundTile = Resources.Load<GameObject>("Prefabs/BackgroundTile");
        cropInventoryCardPrefab = Resources.Load<GameObject>("Prefabs/UI/CropInventoryCard");
        timePanelPrefab = Resources.Load<GameObject>("Prefabs/UI/TimePanel");
        tilledSoilPrefab = Resources.Load<GameObject>("Prefabs/TilledSoil");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
