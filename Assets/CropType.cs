using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
            spritePaths = new List<string>() {
                "Crops/potato_01",
                "Crops/potato_02",
                "Crops/potato_03",
                "Crops/potato_04",
                "Crops/potato_05",
                
            }
    };
}