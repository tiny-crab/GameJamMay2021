using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

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
            if (turnsAlive > 0) {
                int turnsUntilMature = cropType.turnsToHarvest - turnsAlive;
                float percentageComplete = ((float) turnsUntilMature) / ((float) cropType.turnsToHarvest);
                int index = Convert.ToInt32(Math.Floor(cropType.spritePaths.Count * percentageComplete));
                Debug.Log($"Index: {index}");
                int clampedIndex = Math.Min(index, cropType.spritePaths.Count - 1);
                Debug.Log($"ClampedIndex: {clampedIndex}");

                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                Sprite sprite = Resources.Load(cropType.spritePaths[index], typeof(Sprite)) as Sprite;
                Debug.Log($"Maturing: {turnsUntilMature}");
            }    
            
        });
        this.transform.localScale = new Vector2(7,7);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Sprite sprite = Resources.Load(cropType.spritePaths[0], typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
