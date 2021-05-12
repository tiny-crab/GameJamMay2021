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
    private bool runningCoroutine = false;
    private IEnumerator desiredCoroutine = null;

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
                // Get index for sprite location based on turns until ready for harvest
                int clampedIndex = getIndexForSpriteByTimeAlive(turnsUntilMature);

                //Start Coroutine to set new sprite with random delay. If crop is mature we want the sprite to change immediately
                //If this subscription is invoked before a coroutine is finished. We perform the logic of the running coroutine 
                //and start a new coroutine for this turn. A flag is set so the unfinished old coroutine becomes a no-op so if someone spams
                //pass turn then sprites will not be set to a previous stage of growth by old coroutines.
                float delay = 0;
                if (clampedIndex != cropType.spritePathCount) {
                    int maxDelay = dataStore.turnLength.Value;
                    delay = UnityEngine.Random.Range(0.0f, (float) maxDelay * 0.75f);
                }
                
                if (runningCoroutine) {   
                    int indexFromLastTurn = getIndexForSpriteByTimeAlive(turnsUntilMature + 1);
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Sprite sprite = Resources.Load(cropType.getSpritePath(indexFromLastTurn), typeof(Sprite)) as Sprite;
                    spriteRenderer.sprite = sprite;
                    runningCoroutine = false;
                }
                desiredCoroutine = setSpriteAfterDelay(delay, clampedIndex, dataStore.turnCount.Value);
                StartCoroutine(desiredCoroutine);
            }    
        });
        this.transform.localScale = new Vector2(5,5);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Sprite sprite = Resources.Load(cropType.getSpritePath(1), typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
    }

    private int getIndexForSpriteByTimeAlive(int turnsUntilMature) {
        float percentageComplete = 1f - ((float) turnsUntilMature) / ((float) cropType.turnsToHarvest);
        // float floater = ((float) cropType.spritePathCount) * percentageComplete;
        int index = Convert.ToInt32(Math.Floor(((float) cropType.spritePathCount) * percentageComplete));
        int clampedIndex = Math.Min(index, cropType.spritePathCount);
        // Debug.Log($"Percent{percentageComplete}, floater: {floater}, index: {clampedIndex}");
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

        if (desiredTurn != dataStore.turnCount.Value) {
            yield break;
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load(cropType.getSpritePath(index), typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;

        runningCoroutine = false;
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
