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

    [Header("Optional")]
    // The turret currently on this node (if there isn't a turret then turret = null)
    public GameObject turret;

    // Keep track of the renderer so we don't need to refind it
    private Renderer rend;
    // Original node's color
    private Color startColor;

    // Instace of buildManager so we can instantiate turrets
    BuildManager buildManager;

    void Start() {
        // Get the renderer and original color
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        // Initialize the buildManager
        buildManager = BuildManager.instance;
    }

    // Get the position where the turret should be placed
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    // If the user clicks the node, place a turret
    void OnMouseDown()
    {
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
        buildManager.BuildTurretOn(this);
    }

    // If the mouse hovers over the node, change it's color (unless a turret hasn't been chosen)
    void OnMouseEnter()
    {
        // If we're hovering over the turret icons, don't account for hovering over the node
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        // If the user hasn't chosen a turret, don't change the color
        if(!buildManager.CanBuild)
            return;

        // If the player has enough money, set the node's color to the hoverColor
        if(buildManager.HasMoney)
            rend.material.color = hoverColor;
        // Else, set the node's color to the notEnoughMoneyColor
        else
            rend.material.color = notEnoughMoneyColor;
    }

    // When the mouse stops hovering over the node, change the color back
    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
