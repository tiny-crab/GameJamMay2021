public class CropType {
    public string name;
    public int turnsToHarvest;
    public int buyPrice;
    public int sellPrice;
    public Tetromino shape;
}

public static class CropTemplates {
    public static CropType Potato = new CropType() {
            name = "Potato",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
    };
    public static CropType Radish = new CropType() {
            name = "Radish",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
    };
}