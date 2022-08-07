using UnityEngine;

public class BuildManager : MonoBehaviour
{

    // Singleton
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

    // Particle effects when turrets are built/upgraded
    public GameObject buildEffect;
    // Particle effects when turrets are sold
    public GameObject sellEffect;

    // Turret we will build, and selected Node for upgrades
    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    // The nodeUI for selling and upgrading
    public NodeUI nodeUI;

    // Variable to keep track of whether a turret to build is selected and if the user has enough money
    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

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

    // Hide the UI
    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SetTurretToBuilt(TurretBlueprint turret) 
    {
        turretToBuild = turret;
        // Deselect any nodes to disable any nodeUI's
        DeselectNode();
    }

    public TurretBlueprint getTurretToBuild() 
    {
        return turretToBuild;
    }
    
}
