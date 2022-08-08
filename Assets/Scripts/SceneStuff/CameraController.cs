using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    // The camera panning speed and the tolerable border thickness for panning with the mouse on the edge of the screen
    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    // How fast we zoom in and out with the scroll wheel
    public float scrollSpeed = 5f;

    // All the clamping values to keep the camera centered
    [Header("Min/Max Panning and Scrolling")]
    public float minX = 1.5f;
    public float maxX = 73f;
    public float minY = 10f;
    public float maxY = 80f;
    public float minZ = -110f;
    public float maxZ = -3f;
    
    // Update is called once per frame
    void Update()
    {
        // If game over, disable camera controls
        if(GameManager.GameIsOver) 
        {
            this.enabled = false;
            return;
        }

        // Save the transform to edit it without affecting the game till we're done
        Vector3 pos = transform.position;

        // 'w' key pushed or mouse at the top of the screen (within the panBorderThickness)
        if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness) 
        {
            // Translate by {0f, 0f, 1f}
            pos.z += panSpeed * Time.deltaTime;
        }
        // 's' key pushed or mouse at the bottom of the screen (within the panBorderThickness)
        if(Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness) 
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        // 'd' key pushed or mouse on the right edge of the screen (within the panBorderThickness)
        if(Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness) 
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        // 'a' key pushed or mouse on the left edge of the screen (within the panBorderThickness)
        if(Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness) 
        {
            pos.x -= panSpeed * Time.deltaTime;
        }    

        // Scrolling = zooming
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        // Multiply by 1000 since the values are incredibly small by default
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;

        // Clamp all positional axes values to keep the camera inbounds
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        // Copy over the new transform
        transform.position = pos;
    }
    
}
