using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public TMP_Text playerInfoText;  
    public Button formulaireButton;
    public Button viewComment;
    private int id_user;  
    private NetworkManager networkManager = new NetworkManager();

    void Start()
    {
        // Vérifier si l'utilisateur est connecté
        if (PlayerPrefs.HasKey("id_user"))
        {
            id_user = PlayerPrefs.GetInt("id_user");
            Debug.Log("ID utilisateur récupéré : " + id_user);
            StartCoroutine(GetCommentCount());
        }
        else
        {
            playerInfoText.text = "Utilisateur non connecté.";
        }

        formulaireButton.onClick.AddListener(LoadAddCommentScene);
        viewComment.onClick.AddListener(LoadViewCommentScene);
    }

    private IEnumerator GetCommentCount()
    {
        string url = "http://127.0.0.1:5000/getCommentCount";  
        
        RequestData requestData = new RequestData { id_user = id_user };
        string jsonData = JsonUtility.ToJson(requestData);

        Debug.Log("Voici le json data : " + jsonData);

        yield return networkManager.SendJsonRequest(url, jsonData, OnGetCommentSuccess, OnGetCommentError);
    }

    private void OnGetCommentSuccess(string responseText)
    {
        Debug.Log("Réponse du serveur : " + responseText);
        var response = JsonUtility.FromJson<CommentCountResponse>(responseText);

        if (response.status == "success")
        {
            playerInfoText.text = "Nombre de commentaires : " + response.comment_count;
        }
        else
        {
            playerInfoText.text = "Erreur : " + response.message;
        }
    }

    private void OnGetCommentError(string error)
    {
        Debug.LogError("Erreur lors de la récupération des commentaires : " + error);
        playerInfoText.text = "Erreur de connexion.";
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("id_user");
        Debug.Log("Clé 'id_user' supprimée à la fermeture de l'application.");
    }

    private void LoadAddCommentScene()
    {
        SceneManager.LoadScene("AddComment");
    }
    
    private void LoadViewCommentScene()
    {
        SceneManager.LoadScene("ViewComment");
    }
}