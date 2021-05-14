using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GardenTile : MonoBehaviour {

    public Datastore datastore;
    public Crop crop;
    public bool tilled;
    public int x;
    public int y;

    private GameObject tilledSoil;

    void Awake() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
    }

    public Crop grab() {
        if (crop != null && crop.isMature) {
            return crop;
        } else {
            return null;
        }
    }

    public void harvest() {
        datastore.storage[crop.cropType].Value += 1;
        Tween tween = DOTween.To(()=> crop.gameObject.transform.position, x=> crop.gameObject.transform.position = x, datastore.farmStand.transform.position, 0.3f)
        .OnComplete(() => {
            Destroy(crop.gameObject);
            crop = null;
        });
        tween.Play();
        // crop.transform.DOScaleX(0.5f, 0.3f);
        // crop.transform.DOScaleY(0.5f, 0.3f);
    }

    public void till() {
        tilled = true;
        tilledSoil = GameObject.Instantiate(datastore.prefabManager.tilledSoilPrefab, this.transform);
        tilledSoil.transform.position = new Vector3(tilledSoil.transform.position.x, tilledSoil.transform.position.y - 0.3f, 0);
        tilledSoil.GetComponent<SpriteRenderer>().sortingOrder = 5;
        if ((x + y) % 2 == 0) {
            this.GetComponent<SpriteRenderer>().color = datastore.colors["DARK_GROUND"];
        } else {
            this.GetComponent<SpriteRenderer>().color = datastore.colors["GROUND"];
        }
    }
}
