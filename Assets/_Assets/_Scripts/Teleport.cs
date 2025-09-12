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
    
    // Progressive state system: 0=text only, 1=text+image, 2=teleport
    int currentState = 0;
    const int MAX_STATE = 2;

    private void Start()
    {
        maxIndex = descriptionConfigs.Count - 1;
        
        nextButton.onClick.AddListener(HandleNext);
        prevButton.onClick.AddListener(HandlePrev);
        
        // Initialize with first config
        ResetToCurrentConfig();
    }

    public void HandleNext()
    {
        if (IsTransitioning()) return;
        
        if (currentState < MAX_STATE)
        {
            // Progress through states: text -> text+image -> teleport
            currentState++;
            UpdateUI();
        }
        else if (currentIndex < maxIndex)
        {
            // Move to next config and reset state
            currentIndex++;
            currentState = 0;
            ResetToCurrentConfig();
        }
    }

    public void HandlePrev()
    {
        if (IsTransitioning()) return;
        
        if (currentState > 0)
        {
            // Go back through states
            currentState--;
            UpdateUI();
        }
        else if (currentIndex > 0)
        {
            // Move to previous config and set to final state
            currentIndex--;
            currentState = MAX_STATE;
            ResetToCurrentConfig();
        }
    }
    
    private void ResetToCurrentConfig()
    {
        // Move to the current config's location and update UI
        if (descriptionConfigs[currentIndex] != null && descriptionConfigs[currentIndex].location != null)
        {
            Vector3 targetPosition = descriptionConfigs[currentIndex].location.position;
            transform.position = targetPosition;
            
            // Move camera to the corresponding position
            if (cameraMovementManager != null)
            {
                cameraMovementManager.MoveCameraToPosition(targetPosition, cameraOffset);
            }
        }
        
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (descriptionConfigs[currentIndex] != null)
        {
            var config = descriptionConfigs[currentIndex];
            
            // Always show title
            if (title != null) title.text = config.title;
            
            // Show description based on state
            if (description != null) 
            {
                description.text = currentState >= 0 ? config.description : "";
            }
            
            // Show image based on state
            if (image != null) 
            {
                image.gameObject.SetActive(currentState >= 1);
                if (currentState >= 1 && config.subjectImage != null) 
                {
                    image.sprite = config.subjectImage;
                }
            }
        }
        
        // Update button states
        bool canGoNext = (currentState < MAX_STATE) || (currentIndex < maxIndex);
        bool canGoPrev = (currentState > 0) || (currentIndex > 0);
        
        nextButton.interactable = canGoNext;
        prevButton.interactable = canGoPrev;
    }
    
    private bool IsTransitioning()
    {
        return cameraMovementManager != null && cameraMovementManager.IsTransitioning;
    }

    private void OnDestroy()
    {
        nextButton.onClick.RemoveListener(HandleNext);
        prevButton.onClick.RemoveListener(HandlePrev);
    }
}
