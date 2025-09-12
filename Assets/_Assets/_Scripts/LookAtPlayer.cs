using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        // Calculate direction from canvas to camera
        Vector3 direction =  transform.position - mainCam.transform.position;
        
        // Only rotate around Y axis to prevent flipping
        direction.y = 0;
        
        // Apply rotation if there's a valid direction
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
