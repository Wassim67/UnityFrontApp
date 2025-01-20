using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddComment : MonoBehaviour
{
    public TMP_Text infoText;
    public TMP_InputField inputField;  // Champ pour saisir le commentaire
    public Button sendButton;
    public Button mainMenuButton;
    public Button viewComment;

    private int userId;  // ID utilisateur
    private NetworkManager networkManager = new NetworkManager();

    void Start()
    {
        // Vérifier si l'utilisateur est connecté
        if (!PlayerPrefs.HasKey("id_user"))
        {
            // Si l'utilisateur n'est pas connecté, redirigez-le vers la scène de login
            Debug.Log("Utilisateur non connecté, redirection vers la scène de login.");
            SceneManager.LoadScene("LoginScene");
        }
        else
        {
            // Récupérer l'ID utilisateur depuis PlayerPrefs
            userId = PlayerPrefs.GetInt("id_user");
            Debug.Log("ID utilisateur récupéré : " + userId);
        }

        // Ajouter un écouteur au bouton d'envoi
        sendButton.onClick.AddListener(OnSendButtonClick);
        mainMenuButton.onClick.AddListener(LoadMainMenuScene);
        viewComment.onClick.AddListener(LoadViewCommentScene);
    }

    private void OnSendButtonClick()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("Le champ de commentaire est vide.");
            return;
        }

        StartCoroutine(AddCommentCoroutine(inputField.text));
    }

    private IEnumerator AddCommentCoroutine(string comment)
    {
        string url = "http://127.0.0.1:5000/addComment";  // URL de l'API

        RequestData requestData = new RequestData { id_user = userId, commentaire = comment };
        string jsonData = JsonUtility.ToJson(requestData);

        Debug.Log("Voici le json data : " + jsonData);

        yield return networkManager.SendJsonRequest(url, jsonData, OnAddCommentSuccess, OnAddCommentError);
    }

    private void OnAddCommentSuccess(string responseText)
    {
        Debug.Log("Réponse du serveur : " + responseText);
        var response = JsonUtility.FromJson<CommentCountResponse>(responseText);

        if (response.status == "success")
        {
            Debug.Log("Commentaire ajouté avec succès.");
            infoText.text = "Commentaire ajouté avec succès.";
            inputField.text = ""; // Effacer le champ de texte après envoi
        }
        else
        {
            infoText.text = "Erreur lors de l'ajout.";
            Debug.LogError("Erreur : " + response.message);
        }
    }

    private void OnAddCommentError(string error)
    {
        infoText.text = "Erreur serveur.";
        Debug.LogError("Erreur : " + error);
    }
    
    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    private void LoadViewCommentScene()
    {
        SceneManager.LoadScene("ViewComment");
    }
}
