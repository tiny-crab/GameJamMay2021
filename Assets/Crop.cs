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

    private IDisposable turnSubscription;

    void Start() {
        dataStore = GameObject.Find("Datastore").GetComponent<Datastore>();
        turnPlanted = dataStore.turnCount.Value;
        turnSubscription = dataStore.turnCount.Subscribe(currentTurn => {
            int turnsAlive = currentTurn - turnPlanted;
            if (turnsAlive > 0) {
                int turnsUntilMature = cropType.turnsToHarvest - turnsAlive;
                if (turnsUntilMature <= 0) {
                    isMature = true;
                }
                float percentageComplete = 1f - ((float) turnsUntilMature) / ((float) cropType.turnsToHarvest);
                float floater = ((float) cropType.spritePathCount) * percentageComplete;
                int index = Convert.ToInt32(Math.Floor(((float) cropType.spritePathCount) * percentageComplete));
                int clampedIndex = Math.Min(index, cropType.spritePathCount);
                // Debug.Log($"Percent{percentageComplete}, floater: {floater}, index: {clampedIndex}");

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

    void OnDestroy()
    {
        turnSubscription.Dispose();
    }


}
