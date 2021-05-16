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

    public Texture2D pickaxeTexture;

    public IntReactiveProperty plantedSeeds = new IntReactiveProperty(0);
    public IntReactiveProperty harvestedCrops = new IntReactiveProperty(0);
    public IntReactiveProperty tilledSoil = new IntReactiveProperty(0);

    // Start is called before the first frame update
    void Start()
    {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        datastore.mouseState.Subscribe(state => {
            datastore.hoverInfo.setHoveredObject(null);
            if (state == (int) MouseState.DEFAULT) {
                dropShape();
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            } else if (state == (int) MouseState.PLANTING) {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            } else if (state == (int) MouseState.TILLING) {
                dropShape();
                Cursor.SetCursor(pickaxeTexture, Vector2.zero, CursorMode.Auto);
            }
        });

        pickaxeTexture = Resources.Load<Texture2D>("UISprites/pickaxe");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t")) {
            datastore.mouseState.Value = (int) MouseState.TILLING;
        }

        if (Input.GetKeyDown("r")) {
            datastore.heldShape.rotate();
            if (hitTile != null) {
                validPlacement = datastore.garden.checkShapeValidOnGarden(datastore.heldShape, new Vector2(hitTile.x, hitTile.y));
            }
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
                    datastore.hoverInfo.setHoveredObject(hitTile.gameObject);
                }
            } else {
                datastore.hoverInfo.setHoveredObject(null);
                hitTile = null;
                validPlacement = false;
            }

            if (Input.GetMouseButtonDown(0)) {
                if (hitTile != null) {
                    Crop grabbedCrop = hitTile.grab();
                    if (grabbedCrop != null) {
                        hitTile.harvest();
                        harvestedCrops.Value++;
                    }
                }
            }
        } else if (datastore.mouseState.Value == (int) MouseState.TILLING) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << 6);
                if (rayHit.collider != null && rayHit.collider.GetComponent<GardenTile>() != null) {
                    GardenTile tile = rayHit.collider.GetComponent<GardenTile>();
                    if (datastore.tillCount.Value > 0 && !tile.tilled) {
                        tile.till();
                        datastore.tillCount.Value -= 1;
                        tilledSoil.Value++;
                    }
                } else {
                    datastore.mouseState.Value = (int) MouseState.DEFAULT;
                }
                if (datastore.tillCount.Value == 0) {
                    datastore.mouseState.Value = (int) MouseState.DEFAULT;
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
            datastore.heldShape.cropType.shuffleShapeType();
            hitTile = null;
            datastore.heldShape = null;
            for (int i = 0; i < this.heldShapeTiles.Count; i++) {
                Destroy(this.heldShapeTiles[i]);
            }
            this.heldShapeTiles.Clear();
            datastore.mouseState.Value = (int) MouseState.DEFAULT;
            plantedSeeds.Value++;
        } else if (!validPlacement && hitTile == null) {
            dropShape();
            datastore.mouseState.Value = (int) MouseState.DEFAULT;
        } else {
            // Play invalid placement beeping sound
        }
    }

}
