using System.Collections.Generic;

public class CropType {
    public string name;
    public int buyPrice;
    public int sellPrice;
    public int turnsToHarvest;
    public List<string> spritePaths;
}

public static class CropTemplates {
    
    public static CropType Potato = new CropType() {
            name = "Potato",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };
    public static CropType Radish = new CropType() {
            name = "Radish",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };

    public static CropType Pumpkin = new CropType() {
            name = "Pumpkin",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };
    public static CropType Beetroot = new CropType() {
            name = "Beetroot",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };
    public static CropType Cabbage = new CropType() {
            name = "Cabbage",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };

    public static CropType Carrot = new CropType() {
            name = "Carrot",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };

    public static CropType Cauliflower = new CropType() {
            name = "Cauliflower",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };
    public static CropType Kale = new CropType() {
            name = "Kale",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };
    public static CropType Parsnip = new CropType() {
            name = "Parsnip",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };
    public static CropType Wheat = new CropType() {
            name = "Wheat",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };

    public static CropType Sunflower = new CropType() {
            name = "Sunflower",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6,
            spritePathCount = 5
    };

    public static List<CropType> cropTypes = new List<CropType>() {
        CropTemplates.Potato, CropTemplates.Radish, CropTemplates.Pumpkin, CropTemplates.Beetroot, CropTemplates.Cabbage,
        CropTemplates.Carrot, CropTemplates.Cauliflower, CropTemplates.Kale, CropTemplates.Parsnip, CropTemplates.Wheat, CropTemplates.Sunflower
    };
}