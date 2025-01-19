using TMPro; // Nécessaire pour utiliser TextMeshPro
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField identifiantField; // Champ de texte pour le login (TextMeshPro)
    public TMP_InputField passwordField;   // Champ de texte pour le mot de passe (TextMeshPro)
    public Button loginButton;             // Bouton de connexion
    public Button registerButton;          // Bouton d'inscription

    private string loginUrl = "http://127.0.0.1:5000/login";
    private string registerUrl = "http://127.0.0.1:5000/register";

    void Start()
    {
        // Vérifie si les références sont assignées
        if (identifiantField == null)
            Debug.LogError("identifiantField n'est pas assigné !");
        if (passwordField == null)
            Debug.LogError("passwordField n'est pas assigné !");
        if (loginButton == null)
            Debug.LogError("loginButton n'est pas assigné !");
        if (registerButton == null)
            Debug.LogError("registerButton n'est pas assigné !");

        // Ajoute les listeners
        loginButton?.onClick.AddListener(OnLoginButtonClicked);
        registerButton?.onClick.AddListener(OnRegisterButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        string login = identifiantField.text;       // Récupère le texte du champ de login
        string password = passwordField.text;       // Récupère le texte du champ de mot de passe
        StartCoroutine(LoginRequest(login, password));
    }

    private void OnRegisterButtonClicked()
    {
        string login = identifiantField.text;
        string password = passwordField.text;
        StartCoroutine(RegisterRequest(login, password));
    }

    private IEnumerator LoginRequest(string login, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Connexion réussie !");
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                Debug.LogError("Erreur de connexion : " + www.error);
            }
        }
    }

    private IEnumerator RegisterRequest(string login, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Inscription réussie !");
                SceneManager.LoadScene("LoginScene");
            }
            else
            {
                Debug.LogError("Erreur d'inscription : " + www.error);
            }
        }
    }
}
