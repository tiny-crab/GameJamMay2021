using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Crop : MonoBehaviour
{
    public Datastore dataStore;
    public int turnPlanted;
    public CropType cropType;

    void Start() {
        dataStore = GameObject.Find("Datastore").GetComponent<Datastore>();
        turnPlanted = dataStore.turnCount.Value;
        dataStore.turnCount.Subscribe(currentTurn => {
            int turnsAlive = currentTurn - turnPlanted;
            int turnsUntilMature = cropType.turnsToHarvest - turnsAlive;
        
            Debug.Log($"Maturing: {turnsUntilMature}");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
