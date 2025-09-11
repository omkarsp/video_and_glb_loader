using System;
using UnityEngine;

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
    GameState gameState;

    private void Awake()
    {
        gameState = GameState.Login;
    }

    public void ChangeGameState(GameState newState)
    {
        OnGameStateChanged(newState);
    }

    private void OnGameStateChanged(GameState newState)
    {
        gameState = newState;

        switch (gameState)
        {
            case GameState.Login:
            case GameState.Home:
            case GameState.Video:
            case GameState.Glb:
                Debug.Log("Game State Changed to: "  + gameState);
                break;
            default:
                Debug.Log("Game state is wrong");
                break;
        }
    }
}
