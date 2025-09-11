using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoControls : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private VideoPlayer videoPlayer;

    private void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
    }

    private void OnEnable()
    {
        if (playButton != null)  playButton.onClick.AddListener(OnPlayClicked);
        if (pauseButton != null) pauseButton.onClick.AddListener(OnPauseClicked);
        if (stopButton != null)  stopButton.onClick.AddListener(OnStopClicked);
    }

    private void OnDisable()
    {
        if (playButton != null)  playButton.onClick.RemoveListener(OnPlayClicked);
        if (pauseButton != null) pauseButton.onClick.RemoveListener(OnPauseClicked);
        if (stopButton != null)  stopButton.onClick.RemoveListener(OnStopClicked);
    }

    private void OnPlayClicked()
    {
        if (videoPlayer) videoPlayer.Play();
    }

    private void OnPauseClicked()
    {
        if (videoPlayer) videoPlayer.Pause();
    }

    private void OnStopClicked()
    {
        if (videoPlayer) videoPlayer.Stop();
    }
}
