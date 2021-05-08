public class CropType {
    public string name;
    public int turnsToHarvest;
    public int buyPrice;
    public int sellPrice;
    public Tetromino shape;
}

public static class CropTemplates {
    public static CropType Tomato = new CropType() {
            name = "Tomato",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6
    };
}