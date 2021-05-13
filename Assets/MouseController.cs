using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MouseController : MonoBehaviour
{
    public Datastore datastore;

    public GameObject gardenTilePrefab;
    public GameObject heldCropPrefab;

    
    private List<GameObject> heldShapeTiles = new List<GameObject>();
    private GardenTile hitTile;
    private bool validPlacement;
    public GameObject locallyHeldCrop;
    


    // Start is called before the first frame update
    void Start()
    {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        datastore.mouseState.Subscribe(state => {
            if (state == (int) MouseState.DEFAULT) {
                dropShape();
            } else if (state == (int) MouseState.PLANTING) {

            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r")) {
            datastore.heldShape.rotate();
            if (hitTile != null) {
                validPlacement = datastore.garden.checkShapeValidOnGarden(datastore.heldShape, new Vector2(hitTile.x, hitTile.y));
            }
        }

        if (Input.GetKeyDown("w")) {
            datastore.turnCount.Value++;
            datastore.countdown.Value = datastore.turnLength.Value;
            Debug.Log("NewTurn");
        }

        if (datastore.mouseState.Value == (int) MouseState.PLANTING) {
            bool newTileHit = false;

            //Detect if mouse is hovering over a gardenTile
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << 6);
            if (rayHit.collider != null && rayHit.collider.GetComponent<GardenTile>() != null) {
                //Debug.Log("Hit Garden Tile");
                GardenTile newTile = rayHit.collider.GetComponent<GardenTile>();
                if (hitTile == null || hitTile.x != newTile.x || hitTile.y != newTile.y) {
                    validPlacement = datastore.garden.checkShapeValidOnGarden(datastore.heldShape, new Vector2(newTile.x, newTile.y));
                    hitTile = newTile;
                    newTileHit = true;
                }
            } else {
                hitTile = null;
                validPlacement = false;
            }

            //Set position of held Tetromino based on mouse position or hovered garden tile
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < datastore.heldShape.getCoordinates().Count; i++) {
                GameObject tile = this.heldShapeTiles[i];
                var spriteWidth = tile.GetComponent<SpriteRenderer>().bounds.size.x;
                var spriteHeight = tile.GetComponent<SpriteRenderer>().bounds.size.y;
                GardenTile gardenTile = tile.GetComponent<GardenTile>();
                Vector2 coordinate = datastore.heldShape.getCoordinates()[i];
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
        } else if (datastore.mouseState.Value == (int) MouseState.DEFAULT) {

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
                        hitTile.harvest();
                        datastore.storage[grabbedCrop.cropType].Value += 1;
                    }
                }        
            }
        }
    }

    private void dropShape() {
        for (int i = 0; i < this.heldShapeTiles.Count; i++) {
            Destroy(this.heldShapeTiles[i]);
        }
        this.heldShapeTiles.Clear();
        datastore.heldShape = null;
        this.hitTile = null;
    }

    public void holdShape(CropType cropType) {
        for (int i = 0; i < this.heldShapeTiles.Count; i++) {
            Destroy(this.heldShapeTiles[i]);
        }
        this.heldShapeTiles.Clear();

        datastore.heldShape = TetrominoTemplates.createShapeWithCropType((ShapeType) cropType.shapeType.Value, cropType);

        for (int i = 0; i < datastore.heldShape.getCoordinates().Count; i++) {
            GameObject tile = Object.Instantiate(gardenTilePrefab, this.transform);
            heldShapeTiles.Add(tile);
        }
    }

    public void plantSeeds() {
        if (validPlacement) {
            datastore.garden.placeTiles(datastore.heldShape, new Vector2(hitTile.x, hitTile.y));
            int currentCount = datastore.seedInventory[datastore.heldShape.cropType].Value;
            datastore.seedInventory[datastore.heldShape.cropType].SetValueAndForceNotify(currentCount - 1);
            datastore.heldShape.cropType.shuffleShapeType();
            hitTile = null;
            datastore.heldShape = null;
            for (int i = 0; i < this.heldShapeTiles.Count; i++) {
                Destroy(this.heldShapeTiles[i]);
            } 
            this.heldShapeTiles.Clear();
            datastore.mouseState.Value = (int) MouseState.DEFAULT;
        } else if (!validPlacement && hitTile == null) {
            dropShape();
            datastore.mouseState.Value = (int) MouseState.DEFAULT;
        } else {
            // Play invalid placement beeping sound
        }

        
    }
}
