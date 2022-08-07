using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    // Our turret information
    public GameObject prefab;
    public int cost;

    // Our upgraded turret information
    public GameObject upgradedPrefab;
    public int upgradeCost;

    // Get how much the turret sells for (I kept this here, so we know where the sell price was modified)
    public int GetSellAmount(bool isUpgraded)
    {
        return (cost / 2) + ((isUpgraded) ? (upgradeCost / 2) : 0);
    }
}
