using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginUI : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TextMeshProUGUI feedbackText;
    [SerializeField] string homeSceneName = "Home";
    [SerializeField] Button loginButton;
    [SerializeField] Button signUpButton;
    
    void Start()
    {
        loginButton.onClick.AddListener(OnLoginPressed);
        signUpButton.onClick.AddListener(OnSignUpPressed);
    }

    void OnLoginPressed()
    {
        var u = usernameField.text.Trim();
        var p = passwordField.text;
        if (AccountManager.Login(u, p))
        {
            feedbackText.text = "Login successful.";
            GameManager.instance.ChangeGameState(GameState.Home);
        }
        else
        {
            feedbackText.text = "Login failed.";
        }
    }

    void OnSignUpPressed()
    {
        var u = usernameField.text.Trim();
        var p = passwordField.text;
        if (string.IsNullOrWhiteSpace(u) || string.IsNullOrEmpty(p))
        {
            feedbackText.text = "Enter username and password.";
            return;
        }

        if (AccountManager.UserExists(u))
        {
            feedbackText.text = "User already exists.";
            return;
        }

        if (AccountManager.SignUp(u, p))
        {
            feedbackText.text = "Account created. Logging in...";
            GameManager.instance.ChangeGameState(GameState.Home);
        }
        else
        {
            feedbackText.text = "Sign-up failed.";
        }
    }

    void ResetUI()
    {
        usernameField.text = "";
        passwordField.text = "";
        feedbackText.text = "";
    }

    void OnDisable() => ResetUI();

    void OnDestroy()
    {
        loginButton.onClick.RemoveListener(OnLoginPressed);
        signUpButton.onClick.RemoveListener(OnSignUpPressed);
    }
}
