using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino {

    private static Dictionary<Rotation, Rotation> rotationMapping = new Dictionary<Rotation, Rotation>() {
        {Rotation.ZERO, Rotation.NINETY}, {Rotation.NINETY, Rotation.ONEEIGHTY}, {Rotation.ONEEIGHTY, Rotation.TWOSEVENTY}, {Rotation.TWOSEVENTY, Rotation.ZERO}
    };

    private List<Vector2> coordinates;
    public CropType cropType;
    public Rotation rotation;

    public Tetromino(List<Vector2> coordinates, CropType cropType) {
        this.coordinates = coordinates;
        this.cropType = cropType;
        this.rotation = Rotation.ZERO;
    }

    public void rotate() {
        this.rotation = rotationMapping[this.rotation];
        //Debug.Log(this.rotation);
        Vector2 modificationVector = new Vector2(-1, 1);
        for (int i = 0; i < this.coordinates.Count; i++) {
            Vector2 coordinate = this.coordinates[i];
            float x = coordinate.x;
            float y = coordinate.y;
            coordinate.x = y;
            coordinate.y = x * -1;
            this.coordinates[i] = coordinate;
        }
        
    }

    public List<Vector2> getCoordinates() {
        return this.coordinates;
    }

}

public static class TetrominoTemplates {
    private readonly static List<Vector2> sShapeCoordinates = new List<Vector2>() {
        new Vector2(0, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 1)
    };
    private readonly static List<Vector2> sShapeMirrorCoordinates = new List<Vector2>() {
        new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 1)
    };

    private readonly static List<Vector2> lShapeCoordinates = new List<Vector2>() {
        new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 2)
    };
    private readonly static List<Vector2> lShapeMirrorCoordinates = new List<Vector2>() {
        new Vector2(0, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, 2)
    };
    private readonly static List<Vector2> squareShapeCoordinates = new List<Vector2>() {
        new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1)
    };

    private readonly static List<Vector2> iShapeCoordinates = new List<Vector2>() {
        new Vector2(0, 0), new Vector2(0, -1), new Vector2(0, -2), new Vector2(0, -3)
    };

    private readonly static List<Vector2> tShapeCoordinates = new List<Vector2>() {
        new Vector2(0, 0), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0)
    };

    private static Dictionary<ShapeType, List<Vector2>> shapeCoordinatesDict = new Dictionary<ShapeType, List<Vector2>>() {
        {ShapeType.S, sShapeCoordinates}, {ShapeType.L, lShapeCoordinates}, {ShapeType.Square, squareShapeCoordinates},
        {ShapeType.I, iShapeCoordinates}, {ShapeType.T, tShapeCoordinates}, {ShapeType.SMirror, sShapeMirrorCoordinates},
        {ShapeType.LMirror, lShapeMirrorCoordinates}
    };
    
    public static Tetromino createShapeWithCropType(ShapeType shapeType, CropType cropType) { 
        return new Tetromino(shapeCoordinatesDict[shapeType], cropType); 
    }
    
    
}