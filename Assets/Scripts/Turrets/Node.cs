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

    // The turret instance currently on this node (if there isn't a turret then turret == null)
    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretShooting turretScript;
    [HideInInspector] // The blueprint we want to use to build/upgrade on this node
    public TurretBlueprint turretBlueprint;

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
        AudioManager.instance.Play("TurretPlaced");
        GameObject _turret = (GameObject) Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turretScript = _turret.GetComponent<TurretShooting>();
        turretBlueprint = blueprint;

        // Instantiate particle effects for building a turret then delete them after 5 seconds
        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }

    public void UpgradeTurret() 
    {
        PlayerStats.Money -= turretBlueprint.upgradedTurretBlueprint.cost;

        // Get rid of the old turret
        Destroy(turret);

        // Instantiate upgraded turret
        AudioManager.instance.Play("TurretUpgraded");
        GameObject _turret = (GameObject) Instantiate(turretBlueprint.upgradedTurretBlueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turretScript = _turret.GetComponent<TurretShooting>();
        turretBlueprint = turretBlueprint.upgradedTurretBlueprint;

        // Instantiate particle effects for upgrading a turret then delete them after 5 seconds
        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount();

        // Instantiate particle effects for selling a turret then delete them after 5 seconds'
        GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        // Get rid of turret
        AudioManager.instance.Play("TurretSold");
        Destroy(turret);
        turretBlueprint = null;
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
