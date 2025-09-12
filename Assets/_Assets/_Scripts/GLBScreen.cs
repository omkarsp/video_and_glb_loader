using UnityEngine;
using UnityEngine.UI;

public class GLBScreen : MonoBehaviour
{
    [SerializeField] Button closeButton;

    void Start()
    {
        closeButton.onClick.AddListener(ExitToMainMenu);
    }

    void ExitToMainMenu()
    {
        GameManager.instance.ChangeGameState(GameState.Home);
    }

    void OnDestroy()
    {
        closeButton.onClick.RemoveListener(ExitToMainMenu);
    }
}
