using UnityEngine;

public class Shop : MonoBehaviour
{

    // Our turret blueprints
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;

    // Instance of BuildManager so we can interact with the turret prefabsj
    BuildManager buildManager;
    
    void Start() 
    {
        // Initalize the buildManager
        buildManager = BuildManager.instance;
    }

    // Select the standard turret
    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuilt(standardTurret);
    }

    // Select the missile launcher
    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected");
        buildManager.SelectTurretToBuilt(missileLauncher);
    }

    // Select the laser beamer
    public void SelectLaserBeamer()
    {
        Debug.Log("Laser Beamer Selected");
        buildManager.SelectTurretToBuilt(laserBeamer);
    }
}
