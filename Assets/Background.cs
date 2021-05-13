using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    public Datastore datastore;
    
    void Awake() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
    }
    void Start()
    {
        for (int x = -5; x < 7; x++) {
            for (int y = -4; y < 3; y++) {
                GameObject tile = (GameObject) Object.Instantiate(datastore.prefabManager.backgroundTile, Vector3.zero, Quaternion.identity);
                SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
                int spriteWidth = (int) spriteRenderer.bounds.size.x;
                int spriteHeight = (int) spriteRenderer.bounds.size.y;
                tile.transform.position = new Vector3(x * spriteWidth, y * spriteHeight, 0);
                if ((x + y) % 2 == 0) {
                    spriteRenderer.color = datastore.colors["DARK_GREEN"];
                } else {
                    spriteRenderer.color = datastore.colors["GREEN"];
                }
                spriteRenderer.sortingOrder = -10;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
