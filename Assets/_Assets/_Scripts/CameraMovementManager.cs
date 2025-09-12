using UnityEngine;
using System.Collections;

public class CameraMovementManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private bool isTransitioning = false;
    
    public void MoveCameraToPosition(Vector3 targetPosition, Vector3 offset)
    {
        if (isTransitioning) return;
        
        // Calculate final camera position and rotation
        Vector3 cameraPosition = targetPosition + offset;
        Vector3 lookDirection = targetPosition - cameraPosition;
        lookDirection.y = 0; // Keep camera level
        Quaternion cameraRotation = Quaternion.LookRotation(lookDirection);
        
        StartCoroutine(SmoothCameraTransition(cameraPosition, cameraRotation));
    }
    
    private IEnumerator SmoothCameraTransition(Vector3 targetPosition, Quaternion targetRotation)
    {
        isTransitioning = true;
        
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / transitionDuration;
            float curveValue = transitionCurve.Evaluate(progress);
            
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, curveValue);
            
            yield return null;
        }
        
        // Ensure final position is exact
        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
        
        isTransitioning = false;
    }
    
    public void SetTransitionDuration(float duration)
    {
        transitionDuration = duration;
    }
    
    public bool IsTransitioning => isTransitioning;
}
