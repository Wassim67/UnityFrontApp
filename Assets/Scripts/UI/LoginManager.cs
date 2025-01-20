using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_Text info;
    public TMP_InputField identifiantField; // Champ de texte pour le login
    public TMP_InputField passwordField;   // Champ de texte pour le mot de passe
    public Button loginButton;             // Bouton de connexion
    public Button registerButton;          // Bouton d'inscription

    [SerializeField] private string loginUrl = "http://127.0.0.1:5000/login";
    [SerializeField] private string registerUrl = "http://127.0.0.1:5000/register";

    private NetworkManager networkManager = new NetworkManager();

    void Start()
    {
        // Vérifie si les références sont assignées
        ValidateReferences();

        // Ajoute les listeners
        loginButton?.onClick.AddListener(OnLoginButtonClicked);
        registerButton?.onClick.AddListener(OnRegisterButtonClicked);
    }
    

    private void ValidateReferences()
    {
        if (identifiantField == null)
            Debug.LogError("identifiantField n'est pas assigné !");
        if (passwordField == null)
            Debug.LogError("passwordField n'est pas assigné !");
        if (loginButton == null)
            Debug.LogError("loginButton n'est pas assigné !");
        if (registerButton == null)
            Debug.LogError("registerButton n'est pas assigné !");
        if (info == null)
            Debug.LogError("Information n'est pas assigné !");
    }

    private void OnLoginButtonClicked()
    {
        string login = identifiantField.text;
        string password = passwordField.text;

        if (ValidateInputs(login, password))
        {
            LoginData data = new LoginData { login = login, password = password };
            string jsonData = JsonUtility.ToJson(data);

            StartCoroutine(networkManager.SendJsonRequest(loginUrl, jsonData, OnLoginSuccess, OnLoginError));
        }
    }

    private void OnRegisterButtonClicked()
    {
        string login = identifiantField.text;
        string password = passwordField.text;

        if (ValidateInputs(login, password))
        {
            LoginData data = new LoginData { login = login, password = password };
            string jsonData = JsonUtility.ToJson(data);

            StartCoroutine(networkManager.SendJsonRequest(registerUrl, jsonData, OnRegisterSuccess, OnRegisterError));
        }
    }

    private bool ValidateInputs(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            info.text = "<color=red>Veuillez remplir tous les champs.</color>";
            return false;
        }
        return true;
    }

    private void OnLoginSuccess(string responseText)
    {
        Debug.Log("Connexion réussie !");
        var response = JsonUtility.FromJson<LoginResponse>(responseText);
        if (response.status == "success")
        {
            PlayerPrefs.SetInt("id_user", response.id_user);
            PlayerPrefs.Save();
            info.text = "<color=green>Connexion réussie !</color>";
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            info.text = $"<color=red>{response.message}</color>";
        }
    }

    private void OnLoginError(string error)
    {
        Debug.LogError("Erreur de connexion : " + error);
        info.text = "<color=red>Erreur réseau. Veuillez réessayer.</color>";
    }

    private void OnRegisterSuccess(string responseText)
    {
        Debug.Log("Inscription réussie !");
        info.text = "<color=green>Inscription réussie ! Connectez-vous !</color>";
        //SceneManager.LoadScene("LoginScene");
    }

    private void OnRegisterError(string error)
    {
        Debug.LogError("Erreur d'inscription : " + error);
        if (error.Contains("409"))
        {
            info.text = "<color=red>Identifiant déjà utilisé. Veuillez en choisir un autre.</color>";
        }
        else
        {
            info.text = "<color=red>Erreur réseau. Veuillez réessayer.</color>";
        }
    }

}
