using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Datastore dataStore;

    public GameObject gardenTilePrefab;

    public Tetromino heldShape;
    private List<GameObject> heldShapeTiles = new List<GameObject>();

    private GardenTile hitTile;
    private bool validPlacement;
    


    // Start is called before the first frame update
    void Start()
    {
        this.heldShape = TetrominoTemplates.createShapeWithCropType(ShapeType.T, CropTemplates.Tomato);

        for (int i = 0; i < this.heldShape.coordinates.Count; i++) {
            GameObject tile = Object.Instantiate(gardenTilePrefab, this.transform);
            heldShapeTiles.Add(tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            for (int i = 0; i < this.heldShapeTiles.Count; i++) {
                Destroy(this.heldShapeTiles[i]);
            }

            List<ShapeType> shapeTypes = new List<ShapeType>() { ShapeType.T, ShapeType.Square, ShapeType.L, ShapeType.I };
            ShapeType shapeType = (ShapeType) shapeTypes[(int) Random.Range(0, shapeTypes.Count)];
            this.heldShape = TetrominoTemplates.createShapeWithCropType(shapeType, CropTemplates.Tomato);

            this.heldShapeTiles.Clear();

            

            for (int i = 0; i < this.heldShape.coordinates.Count; i++) {
                GameObject tile = Object.Instantiate(gardenTilePrefab, this.transform);
                heldShapeTiles.Add(tile);
            }
        }

        bool newTileHit = false;

        //Detect if mouse is hovering over a gardenTile
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, 1 << 6);
        if (rayHit.collider != null && rayHit.collider.GetComponent<GardenTile>() != null) {
            //Debug.Log("Hit Garden Tile");
            GardenTile newTile = rayHit.collider.GetComponent<GardenTile>();
            if (hitTile == null || hitTile.x != newTile.x || hitTile.y != newTile.y) {
                validPlacement = dataStore.garden.checkShapeValidOnGarden(heldShape, new Vector2(newTile.x, newTile.y));
                Debug.Log("Valid?:" + validPlacement);
                hitTile = newTile;
                newTileHit = true;
            }
        } else {
            hitTile = null;
            validPlacement = false;
        }

        //Set position of held Tetromino based on mouse position or hovered garden tile
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < this.heldShape.coordinates.Count; i++) {
            GameObject tile = this.heldShapeTiles[i];
            var spriteWidth = tile.GetComponent<SpriteRenderer>().bounds.size.x;
            var spriteHeight = tile.GetComponent<SpriteRenderer>().bounds.size.y;
            GardenTile gardenTile = tile.GetComponent<GardenTile>();
            Vector2 coordinate = this.heldShape.coordinates[i];
            Vector2 center;
            if (hitTile != null) {
                center = hitTile.transform.position;
            } else {
                center = mousePosition;
            }
            //Debug.Log("ValidPlacement?: " + validPlacement);
            if (validPlacement) {
                Debug.Log("Setting to Green");
                tile.GetComponent<SpriteRenderer>().color = Color.white;
            } else {
                //Debug.Log("Setting to Red");
                tile.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (!newTileHit || hitTile == null)  {
                tile.transform.position = new Vector2((coordinate.x * spriteWidth) + center.x, (coordinate.y * spriteHeight) + center.y);
            }
        }
    }
}
