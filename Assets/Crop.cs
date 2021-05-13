using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class Crop : MonoBehaviour
{
    public Datastore datastore;
    public int turnPlanted;
    public CropType cropType;
    public bool isMature = false;
    public int turnsUntilMature;
    private bool runningCoroutine = false;
    private IEnumerator spriteChangeCoroutine = null;
    private IDisposable turnSubscription;

    void Start() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        turnPlanted = datastore.turnCount.Value;
        turnSubscription = datastore.turnCount.Subscribe(currentTurn => {
            int turnsAlive = currentTurn - turnPlanted;
            if (turnsAlive > 0) {
                turnsUntilMature = cropType.turnsToHarvest - turnsAlive;
                if (turnsUntilMature <= 0) {
                    isMature = true;
                }
                int clampedIndex = getIndexForSpriteByTurnsUntilMature(turnsUntilMature);

                //Start Coroutine to set new sprite with random delay. If crop is mature we want the sprite to change immediately
                //If this subscription is invoked before a coroutine is finished. We perform the logic that the running coroutine 
                //would execute immediately (update the sprite) and start a new coroutine for this turn. 
                //When running Coroutines finish waiting their specified delay time they break early if they determine the curent turnCount
                //is not the same as their specified turn.
                float delay = 0;
                if (clampedIndex != cropType.spritePathCount) {
                    int maxDelay = datastore.turnLength.Value;
                    delay = UnityEngine.Random.Range(0.0f, (float) maxDelay * 0.75f);
                }
                
                if (runningCoroutine) {   
                    int indexFromLastTurn = getIndexForSpriteByTurnsUntilMature(turnsUntilMature + 1);
                    setSpriteByIndex(indexFromLastTurn);
                    runningCoroutine = false;
                }
                spriteChangeCoroutine = setSpriteAfterDelay(delay, clampedIndex, datastore.turnCount.Value);
                StartCoroutine(spriteChangeCoroutine);
            }    
        });
        this.transform.localScale = new Vector2(5,5);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Sprite sprite = Resources.Load(cropType.getSpritePath(1), typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;

        turnsUntilMature = cropType.turnsToHarvest;
    }

    private int getIndexForSpriteByTurnsUntilMature(int turnsUntilMature) {
        float percentageComplete = 1f - ((float) turnsUntilMature) / ((float) cropType.turnsToHarvest);
        int index = Convert.ToInt32(Math.Floor(((float) cropType.spritePathCount) * percentageComplete));
        int clampedIndex = Math.Min(index, cropType.spritePathCount);
        return clampedIndex;
    }

    public void setAlpha(float alpha) {
        SpriteRenderer cropRenderer = GetComponent<SpriteRenderer>();
        Color cropRendererColor = new Color(cropRenderer.color.r, cropRenderer.color.g, cropRenderer.color.b, alpha);
        cropRenderer.color = cropRendererColor;
    }

    IEnumerator setSpriteAfterDelay(float delay, int index, int desiredTurn) {
        runningCoroutine = true;

        yield return new WaitForSeconds(delay);

        if (desiredTurn != datastore.turnCount.Value) {
            yield break;
        }

        setSpriteByIndex(index);

        runningCoroutine = false;
    }

    private void setSpriteByIndex(int index) {
        Utils.assignSpriteFromPath(this.gameObject, cropType.getSpritePath(index));
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
