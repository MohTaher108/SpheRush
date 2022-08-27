using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{

    // The turret blueprints
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;

    [Header("Buttons")]
    // The turret's buttons
    public Button standardTurretButton;
    public Button missileLauncherButton;
    public Button laserBeamerButton;

    // Instance of BuildManager so the script can set the turret to build to one of the blueprints
    private BuildManager buildManager;
    
    void Start() 
    {
        buildManager = BuildManager.instance;
        
        // Set up all the turret costs
        TextMeshProUGUI standardTurretCost = standardTurretButton.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI missileLauncherCost = missileLauncherButton.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI laserBeamerCost = laserBeamerButton.GetComponentInChildren<TextMeshProUGUI>();

        standardTurretCost.text = "$" + standardTurret.cost;
        missileLauncherCost.text = "$" + missileLauncher.cost;
        laserBeamerCost.text = "$" + laserBeamer.cost;
    }

    // If a turret can't be afforded, disable its button
    void Update()
    {
        if(PlayerStats.Money < standardTurret.cost)
        {
            standardTurretButton.interactable = false;
        } else
        {
            standardTurretButton.interactable = true;
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectStandardTurret();
            }
        }

        if(PlayerStats.Money < missileLauncher.cost)
        {
            missileLauncherButton.interactable = false;
        } else
        {
            missileLauncherButton.interactable = true;
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectMissileLauncher();
            }
        }

        if(PlayerStats.Money < laserBeamer.cost)
        {
            laserBeamerButton.interactable = false;
        } else
        {
            laserBeamerButton.interactable = true;
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectLaserBeamer();
            }
        }
    }

    public void SelectStandardTurret()
    {
        buildManager.SetTurretToBuilt(standardTurret);
        AudioManager.instance.Play("SelectStandardTurret");
    }

    public void SelectMissileLauncher()
    {
        buildManager.SetTurretToBuilt(missileLauncher);
        AudioManager.instance.Play("SelectMissileLauncher");
    }

    public void SelectLaserBeamer()
    {
        buildManager.SetTurretToBuilt(laserBeamer);
        AudioManager.instance.Play("SelectLaserBeamer");
    }
}
