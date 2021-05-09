using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CropType {
    public string name;
    public int buyPrice;
    public int sellPrice;
    public int turnsToHarvest;

}

public static class CropTemplates {
    public static CropType Potato = new CropType() {
            name = "Potato",
            turnsToHarvest = 4,
            buyPrice = 4,
            sellPrice = 6
    };
}