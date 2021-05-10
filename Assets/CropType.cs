using System.Collections.Generic;

public class CropType {
    public string name;
    public int buyPrice;
    public int sellPrice;
    public int turnsToHarvest;
    public List<string> spritePaths;
}

public static class CropTemplates {
    public static CropType Radish = new CropType() {
            name = "Radish",
            turnsToHarvest = 3,
            buyPrice = 6,
            sellPrice = 10,
            spritePaths = new List<string>() {
                "Crops/potato_01",
                "Crops/potato_02",
                "Crops/potato_03",
                "Crops/potato_04",
                "Crops/potato_05",
            }
    };
    public static CropType Potato = new CropType() {
            name = "Potato",
            turnsToHarvest = 5,
            buyPrice = 2,
            sellPrice = 5,
            spritePaths = new List<string>() {
                "Crops/potato_01",
                "Crops/potato_02",
                "Crops/potato_03",
                "Crops/potato_04",
                "Crops/potato_05",
            }
    };
}