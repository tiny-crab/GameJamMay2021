using System.Collections.Generic;
using System;
using UniRx;

public class CropType {
    private static RandomShapeGenerator randomShapeGeneratorInstance;
    public string name;
    public int turnsToHarvest;
    public int spritePathCount;
    public IntReactiveProperty shapeType = new IntReactiveProperty();
    private Random random;

    public CropType() {
        random = new Random((int) DateTimeOffset.Now.ToUnixTimeMilliseconds());
        shuffleShapeType();
    }

    // If we want to persist the shapetype of the order as they are purchased we can keep a list of them and add them to the end
    // as they are bought and remove them from the front as they are planted.
    public void shuffleShapeType() {
        shapeType.Value = (int) CropType.getRandomShapeType();
    }

    public string getSpritePath(int index) {
        if (index < 1) {
            index = 1;
        } else if (index > spritePathCount) {
            index = spritePathCount;
        }

        return $"Crops/{name.ToLower()}_0{index}";
    }

    private static ShapeType getRandomShapeType() {
        if (randomShapeGeneratorInstance == null) {
            randomShapeGeneratorInstance = new RandomShapeGenerator();
        }
        return randomShapeGeneratorInstance.getRandomShapeType();
    }

    private class RandomShapeGenerator {

        private Random random;
        private List<ShapeType> shapeTypes = new List<ShapeType>() { ShapeType.T, ShapeType.O, ShapeType.L, ShapeType.I, ShapeType.S, ShapeType.Z, ShapeType.J };
        public RandomShapeGenerator() {
            this.random = new Random((int) DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }
        public ShapeType getRandomShapeType() {
            return (ShapeType) shapeTypes[random.Next(0, shapeTypes.Count)];
        }
    }
}

public static class CropTemplates {

    public static CropType Potato = new CropType() {
            name = "Potato",
            turnsToHarvest = 4,
            spritePathCount = 5
    };
    public static CropType Radish = new CropType() {
            name = "Radish",
            turnsToHarvest = 3,
            spritePathCount = 5
    };

    public static CropType Pumpkin = new CropType() {
            name = "Pumpkin",
            turnsToHarvest = 8,
            spritePathCount = 5
    };
    public static CropType Beetroot = new CropType() {
            name = "Beetroot",
            turnsToHarvest = 3,
            spritePathCount = 5
    };
    public static CropType Cabbage = new CropType() {
            name = "Cabbage",
            turnsToHarvest = 3,
            spritePathCount = 5
    };

    public static CropType Carrot = new CropType() {
            name = "Carrot",
            turnsToHarvest = 2,
            spritePathCount = 5
    };

    public static CropType Cauliflower = new CropType() {
            name = "Cauliflower",
            turnsToHarvest = 5,
            spritePathCount = 5
    };
    public static CropType Kale = new CropType() {
            name = "Kale",
            turnsToHarvest = 2,
            spritePathCount = 5
    };
    public static CropType Parsnip = new CropType() {
            name = "Parsnip",
            turnsToHarvest = 4,
            spritePathCount = 5
    };
    public static CropType Wheat = new CropType() {
            name = "Wheat",
            turnsToHarvest = 2,
            spritePathCount = 5
    };

    public static CropType Sunflower = new CropType() {
            name = "Sunflower",
            turnsToHarvest = 6,
            spritePathCount = 5
    };

    public static List<CropType> cropTypes = new List<CropType>() {
        CropTemplates.Potato, CropTemplates.Radish, CropTemplates.Pumpkin, CropTemplates.Beetroot, CropTemplates.Cabbage,
        CropTemplates.Carrot, CropTemplates.Cauliflower, CropTemplates.Kale, CropTemplates.Parsnip, CropTemplates.Wheat, CropTemplates.Sunflower
    };
}