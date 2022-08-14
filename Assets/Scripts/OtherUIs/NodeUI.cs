using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class NodeUI : MonoBehaviour
{
    
    // Reference to canvas so we can disable/enable it easily
    public GameObject ui;

    public TextMeshProUGUI upgradeCost;
    public Button upgradeButton;
    public TextMeshProUGUI sellAmount;

    // Reference to the node the upgrade UI needs to hover over
    private Node target;

    private bool hideCoroutineCheck;

    void Update()
    {
        if(target == null)
            return;

        if(Input.GetKeyDown(KeyCode.Comma) && target.turretBlueprint.isUpgradable)
            Upgrade();
        
        if(Input.GetKeyDown(KeyCode.Period))
            Sell();
    }

    // Set the target we're pointing at, and put the upgradeUI above them
    public void SetTarget(Node _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();

        // If target isn't fully upgraded, display cost and UI
        if(!target.turretBlueprint.maxUpgrade) 
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradedTurretBlueprint.cost;
            // Check if the user has enough money to upgrade the turret
            StartCoroutine(checkMoney());
        } else // If target is upgraded, display "DONE" and disallow clicking the upgradeButton
        {
            upgradeCost.text = "DONE";
            upgradeButton.interactable = false;
        }

        // Display sell amount
        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

        ui.SetActive(true);
    }

    // Check the user's money count every frame to see if they can upgrade the turret while the UI is active
    IEnumerator checkMoney()
    {
        upgradeButton.interactable = false;
        hideCoroutineCheck = false;

        while(!hideCoroutineCheck)
        {
            if(target.turretBlueprint.isUpgradable)
            {
                upgradeButton.interactable = true;
                hideCoroutineCheck = true;
            }

            yield return null;
        }
    }

    public void Hide()
    {
        hideCoroutineCheck = true;
        ui.SetActive(false);
    }

    public void Upgrade() 
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        target = null;
        BuildManager.instance.DeselectNode();
    }

}
