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
    public bool isMature = false;

    void Start() {
        dataStore = GameObject.Find("Datastore").GetComponent<Datastore>();
        turnPlanted = dataStore.turnCount.Value;
        dataStore.turnCount.Subscribe(currentTurn => {
            int turnsAlive = currentTurn - turnPlanted;
            if (turnsAlive > 0) {
                int turnsUntilMature = cropType.turnsToHarvest - turnsAlive;
                if (turnsUntilMature <= 0) {
                    isMature = true;
                }
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

    public void setAlpha(float alpha) {
        SpriteRenderer cropRenderer = GetComponent<SpriteRenderer>();
        Color cropRendererColor = new Color(cropRenderer.color.r, cropRenderer.color.g, cropRenderer.color.b, alpha);
        cropRenderer.color = cropRendererColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
