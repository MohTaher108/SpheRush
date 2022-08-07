using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{

    // Color the node will change to when we hover over it
    public Color hoverColor;
    // Color the node will change to when we hover over it and can't afford the selected turret
    public Color notEnoughMoneyColor;
    // Offset of spawnpoint of turret so it doesn't spawn inside the node
    public Vector3 positionOffset;

    [HideInInspector] // The turret instance currently on this node (if there isn't a turret then turret = null)
    public GameObject turret;
    [HideInInspector] // The blueprint we want to use to build/upgrade on this node
    public TurretBlueprint turretBlueprint;
    [HideInInspector] // Is the turret upgraded already?
    public bool isUpgraded = false;

    // Keep track of the renderer so we don't need to refind it
    private Renderer rend;
    // Node's original color
    private Color startColor;

    // Instace of buildManager so we can instantiate turrets
    BuildManager buildManager;

    void Start() 
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    void OnMouseDown()
    {
        // If mouse is on top of NodeUI, ignore this function call
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        // If there's a turret, use the upgrade UI
        if(turret != null)
        {
            buildManager.selectNode(this);
            return;
        }

        // If we haven't chosen a turret, don't do anything
        if(!buildManager.CanBuild)
            return;

        // Build turret on this node
        BuildTurret(buildManager.getTurretToBuild());
    }

    void BuildTurret(TurretBlueprint blueprint) 
    {
        // If user is out of money, return
        if(PlayerStats.Money < blueprint.cost)
            return;

        PlayerStats.Money -= blueprint.cost;

        // Instantiate turret
        GameObject _turret = (GameObject) Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turretBlueprint = blueprint;

        // Instantiate particle effects for building a turret then delete them after 5 seconds
        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }

    public void UpgradeTurret() 
    {
        PlayerStats.Money -= turretBlueprint.upgradeCost;

        // Get rid of the old turret
        Destroy(turret);

        // Instantiate upgraded turret
        GameObject _turret = (GameObject) Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        isUpgraded = true;

        // Instantiate particle effects for upgrading a turret then delete them after 5 seconds
        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount(isUpgraded);

        // Instantiate particle effects for selling a turret then delete them after 5 seconds
        GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        // Get rid of turret
        Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;
    }

    // Change the node's color when it's hovered over
    void OnMouseEnter()
    {
        // If we're hovering over the nodeUI, don't account for hovering over the node
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        // If the user hasn't chosen a turret, don't change the color
        if(!buildManager.CanBuild)
            return;

        if(buildManager.HasMoney)
            rend.material.color = hoverColor;
        else
            rend.material.color = notEnoughMoneyColor;
    }

    // Change back the node's color when it's not hovered over anymore
    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

}
