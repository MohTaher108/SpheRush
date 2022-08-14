using UnityEngine;

public class TurretBlueprint : MonoBehaviour
{
    public GameObject prefab;
    public int cost;
    public int value;

    public TurretBlueprint upgradedTurretBlueprint;
    public bool maxUpgrade { get { return upgradedTurretBlueprint == null; } }
    public bool isUpgradable { get { return !maxUpgrade && PlayerStats.Money >= upgradedTurretBlueprint.cost; } }

    // Return 70% of the original value
    public int GetSellAmount()
    {
        return (value * 7) / 10;
    }
}
