using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino {

    public List<Vector2> coordinates;
    public CropType cropType;

    public Tetromino(List<Vector2> coordinates, CropType cropType) {
        this.coordinates = coordinates;
        this.cropType = cropType;
    }

}

public static class TetrominoTemplates {
    private readonly static List<Vector2> sShapeCoordinates = new List<Vector2>() {
        new Vector2(0, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 1)
    };

    private readonly static List<Vector2> lShapeCoordinates = new List<Vector2>() {
        new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 2)
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
        {ShapeType.I, iShapeCoordinates}, {ShapeType.T, tShapeCoordinates}
    };
    
    public static Tetromino createShapeWithCropType(ShapeType shapeType, CropType cropType) { 
        return new Tetromino(shapeCoordinatesDict[shapeType], cropType); 
    }
    
    
}