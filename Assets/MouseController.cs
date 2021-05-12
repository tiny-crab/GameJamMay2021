using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MouseController : MonoBehaviour
{
    public Datastore dataStore;

    public GameObject gardenTilePrefab;
    public GameObject heldCropPrefab;

    
    private List<GameObject> heldShapeTiles = new List<GameObject>();
    private GardenTile hitTile;
    private bool validPlacement;
    public GameObject locallyHeldCrop;
    


    // Start is called before the first frame update
    void Start()
    {
        dataStore.mouseState.Subscribe(state => {
            if (state == (int) MouseState.DEFAULT) {
                dropShape();
            } else if (state == (int) MouseState.PLANTING) {

            } else if (state == (int) MouseState.HOLDING) {

            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("d")) {
            dataStore.mouseState.Value = (int) MouseState.DEFAULT;
        }

        if (Input.GetKeyDown("s")) {
            dataStore.mouseState.Value = (int) MouseState.PLANTING;
        }

        // if (Input.GetKeyDown("space")) {
        //     getNewShape();
        // }

        if (Input.GetKeyDown("r")) {
            dataStore.heldShape.rotate();
            if (hitTile != null) {
                validPlacement = dataStore.garden.checkShapeValidOnGarden(dataStore.heldShape, new Vector2(hitTile.x, hitTile.y));
            }
        }

        if (Input.GetKeyDown("w")) {
            dataStore.turnCount.Value++;
            dataStore.countdown.Value = dataStore.turnLength.Value;
            Debug.Log("NewTurn");
        }

        if (dataStore.mouseState.Value == (int) MouseState.PLANTING) {
            bool newTileHit = false;

            //Detect if mouse is hovering over a gardenTile
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << 6);
            if (rayHit.collider != null && rayHit.collider.GetComponent<GardenTile>() != null) {
                //Debug.Log("Hit Garden Tile");
                GardenTile newTile = rayHit.collider.GetComponent<GardenTile>();
                if (hitTile == null || hitTile.x != newTile.x || hitTile.y != newTile.y) {
                    validPlacement = dataStore.garden.checkShapeValidOnGarden(dataStore.heldShape, new Vector2(newTile.x, newTile.y));
                    //Debug.Log("Valid?:" + validPlacement);
                    hitTile = newTile;
                    newTileHit = true;
                }
            } else {
                hitTile = null;
                validPlacement = false;
            }

            //Set position of held Tetromino based on mouse position or hovered garden tile
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < dataStore.heldShape.getCoordinates().Count; i++) {
                GameObject tile = this.heldShapeTiles[i];
                var spriteWidth = tile.GetComponent<SpriteRenderer>().bounds.size.x;
                var spriteHeight = tile.GetComponent<SpriteRenderer>().bounds.size.y;
                GardenTile gardenTile = tile.GetComponent<GardenTile>();
                Vector2 coordinate = dataStore.heldShape.getCoordinates()[i];
                Vector2 center;
                if (hitTile != null) {
                    center = hitTile.transform.position;
                } else {
                    center = mousePosition;
                }
                if (validPlacement) {
                    tile.GetComponent<SpriteRenderer>().color = Color.white;
                } else {
                    tile.GetComponent<SpriteRenderer>().color = Color.red;
                }
                if (!newTileHit || hitTile == null)  {
                    tile.transform.position = new Vector2((coordinate.x * spriteWidth) + center.x, (coordinate.y * spriteHeight) + center.y);
                }
            }
            
            if (Input.GetMouseButtonDown(0)) {
                    plantSeeds();
            }
        } else if (dataStore.mouseState.Value == (int) MouseState.DEFAULT) {

            //Detect if mouse is hovering over a gardenTile
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << 6);
            if (rayHit.collider != null && rayHit.collider.GetComponent<GardenTile>() != null) {
                GardenTile newTile = rayHit.collider.GetComponent<GardenTile>();
                if (hitTile == null || hitTile.x != newTile.x || hitTile.y != newTile.y) {
                    hitTile = newTile;
                }
            } else {
                hitTile = null;
                validPlacement = false;
            }

            if (Input.GetMouseButtonDown(0)) {
                if (hitTile != null) {
                    Crop grabbedCrop = hitTile.grab();
                    if (grabbedCrop != null) {
                        dataStore.heldCrop = grabbedCrop;
                        dataStore.heldCrop.setAlpha(0.5f);

                        locallyHeldCrop = Object.Instantiate(heldCropPrefab, Input.mousePosition, Quaternion.identity);
                        SpriteRenderer spriteRenderer = locallyHeldCrop.GetComponent<SpriteRenderer>();
                        Sprite sprite = Resources.Load(dataStore.heldCrop.cropType.getSpritePath(dataStore.heldCrop.cropType.spritePathCount), typeof(Sprite)) as Sprite;
                        spriteRenderer.sprite = sprite;
                        locallyHeldCrop.transform.localScale = new Vector2(5,5);
                        spriteRenderer.sortingOrder = 10;

                        dataStore.mouseState.Value = (int) MouseState.HOLDING;
                    }
                }        
            }
        } else if (dataStore.mouseState.Value == (int) MouseState.HOLDING) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << 7);
                if (rayHit.collider != null) {
                    hitTile.harvest();
                    dataStore.storage[dataStore.heldCrop.cropType].Value += 1;
                    Debug.Log($"CropType{dataStore.heldCrop.cropType.name}, StorageCount: {dataStore.storage[dataStore.heldCrop.cropType].Value}");
                }
                Destroy(locallyHeldCrop);
                dataStore.heldCrop.setAlpha(1f);
                dataStore.heldCrop = null;
                dataStore.mouseState.Value = (int) MouseState.DEFAULT;
            }

            if (Input.GetMouseButtonUp(0)) {
                RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << 7);
                if (rayHit.collider != null) {
                    hitTile.harvest();
                    dataStore.storage[dataStore.heldCrop.cropType].Value += 1;
                    Debug.Log($"CropType{dataStore.heldCrop.cropType.name}, StorageCount: {dataStore.storage[dataStore.heldCrop.cropType].Value}");

                    Destroy(locallyHeldCrop);
                    dataStore.heldCrop = null;

                    dataStore.mouseState.Value = (int) MouseState.DEFAULT;
                }
            }
        }
    }

    private void dropShape() {
        for (int i = 0; i < this.heldShapeTiles.Count; i++) {
            Destroy(this.heldShapeTiles[i]);
        }
        dataStore.heldShape = null;
        this.hitTile = null;
    }

    public void holdShape(CropType cropType) {
        for (int i = 0; i < this.heldShapeTiles.Count; i++) {
            Destroy(this.heldShapeTiles[i]);
        }
        this.heldShapeTiles.Clear();

        dataStore.heldShape = TetrominoTemplates.createShapeWithCropType((ShapeType) cropType.shapeType.Value, cropType);

        for (int i = 0; i < dataStore.heldShape.getCoordinates().Count; i++) {
            GameObject tile = Object.Instantiate(gardenTilePrefab, this.transform);
            heldShapeTiles.Add(tile);
        }
    }

    public void plantSeeds() {
        if (validPlacement) {
            dataStore.garden.placeTiles(dataStore.heldShape, new Vector2(hitTile.x, hitTile.y));
            int currentCount = dataStore.seedInventory[dataStore.heldShape.cropType].Value;
            dataStore.seedInventory[dataStore.heldShape.cropType].SetValueAndForceNotify(currentCount - 1);
            dataStore.heldShape.cropType.shuffleShapeType();
            hitTile = null;
            dataStore.heldShape = null;
            for (int i = 0; i < this.heldShapeTiles.Count; i++) {
                Destroy(this.heldShapeTiles[i]);
            } 
            this.heldShapeTiles.Clear();
            dataStore.mouseState.Value = (int) MouseState.DEFAULT;
        } else if (!validPlacement && hitTile == null) {
            dropShape();
            dataStore.mouseState.Value = (int) MouseState.DEFAULT;
        } else {
            // Play invalid placement beeping sound
        }

        
    }
}
