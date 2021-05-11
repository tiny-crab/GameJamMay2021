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
                int invertedIndex = Convert.ToInt32(Math.Floor(cropType.spritePathCount * percentageComplete));
                int index = cropType.spritePathCount - invertedIndex;
                int clampedIndex = Math.Min(index, cropType.spritePathCount);

                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                Sprite sprite = Resources.Load(cropType.getSpritePath(clampedIndex), typeof(Sprite)) as Sprite;
                spriteRenderer.sprite = sprite;
            }    
        });
        this.transform.localScale = new Vector2(5,5);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Sprite sprite = Resources.Load(cropType.getSpritePath(1), typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
