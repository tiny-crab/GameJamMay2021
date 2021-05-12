using System.Collections.Generic;

public enum ShapeType {
    S,
    T,
    O,
    L,
    I,
    J,
    Z
}

public static class ShapeTypeImages {
    public static Dictionary<ShapeType, string> ShapeTypeToSpriteLocation = new Dictionary<ShapeType, string>() {
        {ShapeType.S, "Tetrominos/s_shape"}, {ShapeType.T, "Tetrominos/t_shape"}, {ShapeType.O, "Tetrominos/o_shape"}, 
        {ShapeType.L, "Tetrominos/l_shape"}, {ShapeType.I, "Tetrominos/i_shape"}, {ShapeType.J, "Tetrominos/j_shape"}, 
        {ShapeType.Z, "Tetrominos/z_shape"}
    };
}