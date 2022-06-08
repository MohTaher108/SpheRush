using UnityEngine;

public class NodeUI : MonoBehaviour
{
    
    // Reference to canvas so we can disable/enable it easily
    public GameObject ui;

    // Reference to the node the upgrade UI needs to hover over
    private Node target;

    // Set the target we're pointing at, and put the upgradeUI above them
    public void SetTarget(Node _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();

        ui.SetActive(true);
    }

    // Hide the canvas
    public void Hide()
    {
        ui.SetActive(false);
    }
}
