using UnityEngine;

public class BuildManager : MonoBehaviour
{

    // A static reference to the BuildManager so we don't reference a new BuildManager from every Node
    public static BuildManager instance;

    // Awake() happens right before Start(), where we set the BuildManager's instance equal to itself
    void Awake()
    {
        // Print an error if a second BuildManager is ever made accidentally
        if(instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }

        instance = this;
    }

    // Particle effects when turrets are built
    public GameObject buildEffect;

    // Turret we will build, and selected Node for upgrades
    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    // The upgrade node UI
    public NodeUI nodeUI;

    // Variable to keep track of whether a turret to build is selected
    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

    // Build a turret on a node
    public void BuildTurretOn(Node node)
    {
        // Don't allow building if not enough money
        if(PlayerStats.Money < turretToBuild.cost)
        {
            Debug.Log("Not enough money");
            return;
        }

        // TAKE THE MONEY
        PlayerStats.Money -= turretToBuild.cost;

        // Instantiate turret and save it at the node
        GameObject turret = (GameObject) Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        // Instantiate particle effects for building a turret then delete them after 5 seconds
        GameObject effect = Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Debug.Log("Turret built! Money left: " + PlayerStats.Money);
    }

    // Select a node
    public void selectNode(Node node) 
    {
        // If this node's already selected, deselect it
        if(selectedNode == node) 
        {
            DeselectNode();
            return;
        }

        // Set the selectedNode and enable the nodeUI (also disable the turretToBuild)
        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    // Deselect the node by hiding the UI
    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    // Setter of the turret to build
    public void SelectTurretToBuilt(TurretBlueprint turret) 
    {
        turretToBuild = turret;
        // Deselect Node and disable upgrade UI
        DeselectNode();
    }
}
