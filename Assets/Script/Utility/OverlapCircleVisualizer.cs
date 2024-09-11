using UnityEngine;

public class OverlapCircleVisualizer : MonoBehaviour
{
    public float radius = 2f;  // The radius of the OverlapCircleAll
    public Vector2 position = Vector2.zero;   // The position from where to check

    void OnDrawGizmos()
    {
        // Set the color of the gizmo
        Gizmos.color = Color.red;
        
        // Draw a wireframe sphere at the specified position with the given radius
        Gizmos.DrawWireSphere(position, radius / 2);
    }

    void Update()
    {
        // Example of how OverlapCircleAll might be used in your game
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius / 2);

        // You can loop through the colliders to see what was detected
        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Detected: " + collider.name);
        }
    }
}