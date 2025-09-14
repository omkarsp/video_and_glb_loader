using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public enum GameState
{
    Login,
    Home,
    Video,
    Glb
}

public class GameManager : MonoBehaviour
{
    GameState currentGameState;

    Dictionary<GameState, GameObject> screens;

    public static GameManager instance;

    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject videoScreen;
    [SerializeField] private GameObject glbView;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        
        screens = new Dictionary<GameState, GameObject>()
        {
            { GameState.Login, loginScreen },
            { GameState.Home, homeScreen },
            { GameState.Video, videoScreen },
            { GameState.Glb, glbView }
        };
    }

    private void Start()
    {
        ChangeGameState(GameState.Login);
    }

    public void ChangeGameState(GameState newState)
    {
        OnGameStateChanged(newState);
    }

    private void OnGameStateChanged(GameState newState)
    {
        currentGameState = newState;

        switch (currentGameState)
        {
            case GameState.Login:
            case GameState.Home:
            case GameState.Video:
            case GameState.Glb:
                ActivateScreen(currentGameState);
                break;
            default:
                Debug.LogError("Invalid game state: " + currentGameState);
                break;
        }
    }

    private void ActivateScreen(GameState state)
    {
        foreach (var screen in screens.Values) screen?.SetActive(false);
        if(screens.TryGetValue(state, out var scr) && scr) scr.SetActive(true);
    }
    
    public GameState GetCurrentGameState() => currentGameState;
}