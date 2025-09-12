using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] List<DescriptionConfig> descriptionConfigs;
    
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    
    [Header("Populate UI")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image image;
    
    [Header("Camera Movement")]
    [SerializeField] CameraMovementManager cameraMovementManager;
    [SerializeField] Vector3 cameraOffset = new Vector3(0, 1.5f, -3f);
    
    int currentIndex = 0;
    int maxIndex = 0;

    private void Start()
    {
        maxIndex = descriptionConfigs.Count - 1;
        
        nextButton.onClick.AddListener(TeleportToNext);
        prevButton.onClick.AddListener(TeleportToPrev);
        
        // Move to initial position
        UpdateUI();
        MoveToCurrentPosition();
    }

    public void TeleportToNext()
    {
        if (currentIndex < maxIndex && !IsTransitioning())
        {
            currentIndex++;
            MoveToCurrentPosition();
            UpdateUI();
        }
    }

    public void TeleportToPrev()
    {
        if (currentIndex > 0 && !IsTransitioning())
        {
            currentIndex--;
            MoveToCurrentPosition();
            UpdateUI();
        }
    }
    
    private void MoveToCurrentPosition()
    {
        // Move the canvas/object to the new location
        if (descriptionConfigs[currentIndex].location != null)
        {
            Vector3 targetPosition = descriptionConfigs[currentIndex].location.position;
            transform.position = targetPosition;
            
            // Move camera to the corresponding position
            if (cameraMovementManager != null)
            {
                cameraMovementManager.MoveCameraToPosition(targetPosition, cameraOffset);
            }
        }
    }
    
    private void UpdateUI()
    {
        if (descriptionConfigs[currentIndex] != null)
        {
            var config = descriptionConfigs[currentIndex];
            
            if (title != null) title.text = config.title;
            if (description != null) description.text = config.description;
            if (image != null && config.subjectImage != null) image.sprite = config.subjectImage;
        }
        
        nextButton.interactable = currentIndex < maxIndex;
        prevButton.interactable = currentIndex > 0;
    }
    
    private bool IsTransitioning()
    {
        return cameraMovementManager != null && cameraMovementManager.IsTransitioning;
    }

    private void OnDestroy()
    {
        nextButton.onClick.RemoveListener(TeleportToNext);
        prevButton.onClick.RemoveListener(TeleportToPrev);
    }
}
