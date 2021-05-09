using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Datastore dataStore;

    public GameObject gardenTilePrefab;

    
    private List<GameObject> heldShapeTiles = new List<GameObject>();

    private GardenTile hitTile;
    private bool validPlacement;
    


    // Start is called before the first frame update
    void Start()
    {
        dataStore.heldShape = TetrominoTemplates.createShapeWithCropType(ShapeType.T, CropTemplates.Potato);

        for (int i = 0; i < dataStore.heldShape.getCoordinates().Count; i++) {
            GameObject tile = Object.Instantiate(gardenTilePrefab, this.transform);
            heldShapeTiles.Add(tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            for (int i = 0; i < this.heldShapeTiles.Count; i++) {
                Destroy(this.heldShapeTiles[i]);
            }

            List<ShapeType> shapeTypes = new List<ShapeType>() { ShapeType.T, ShapeType.Square, ShapeType.L, ShapeType.I, ShapeType.S, ShapeType.SMirror, ShapeType.LMirror };
            ShapeType shapeType = (ShapeType) shapeTypes[(int) Random.Range(0, shapeTypes.Count)];
            dataStore.heldShape = TetrominoTemplates.createShapeWithCropType(shapeType, CropTemplates.Potato);

            this.heldShapeTiles.Clear();

            for (int i = 0; i < dataStore.heldShape.getCoordinates().Count; i++) {
                GameObject tile = Object.Instantiate(gardenTilePrefab, this.transform);
                heldShapeTiles.Add(tile);
            }

            if (hitTile != null) {
                validPlacement = dataStore.garden.checkShapeValidOnGarden(dataStore.heldShape, new Vector2(hitTile.x, hitTile.y));
            }
        }

        if (Input.GetKeyDown("r")) {
            dataStore.heldShape.rotate();
            if (hitTile != null) {
                validPlacement = dataStore.garden.checkShapeValidOnGarden(dataStore.heldShape, new Vector2(hitTile.x, hitTile.y));
            }
        }

        if (Input.GetKeyDown("w")) {
            dataStore.turnCount.Value++;
        }

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
            if (validPlacement) {
                dataStore.garden.placeTiles(dataStore.heldShape, new Vector2(hitTile.x, hitTile.y));
            }        
        }
    }
}
