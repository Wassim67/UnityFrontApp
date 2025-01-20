using UnityEngine;
using TMPro; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataGridManager : MonoBehaviour
{
    public GameObject rowPrefab; // Le Prefab pour une ligne
    public Transform content; // Le conteneur "Content" dans la Scroll View
    public TMP_Text playerInfoText; // Texte pour afficher des informations (ex : erreurs ou messages de succès)
    public Button mainMenuButton;
    public Button formulaireButton;
    private NetworkManager networkManager = new NetworkManager();

    void Start()
    {
        // Récupérer tous les commentaires dès le démarrage
        StartCoroutine(GetAllComments());
        mainMenuButton.onClick.AddListener(LoadMainMenuScene);
        formulaireButton.onClick.AddListener(LoadAddCommentScene);

    }

    // Fonction pour récupérer tous les commentaires depuis l'API
    private IEnumerator GetAllComments()
    {
        string url = "http://127.0.0.1:5000/getAllComments"; // URL de l'API

        yield return networkManager.GetRequest(url, OnGetAllCommentsSuccess, OnGetAllCommentsError);
    }

    // Fonction appelée en cas de succès de la requête
    private void OnGetAllCommentsSuccess(string responseText)
    {
        Debug.Log("Réponse du serveur : " + responseText);

        // Parser la réponse JSON
        var response = JsonUtility.FromJson<CommentsResponse>(responseText);

        if (response.status == "success")
        {
            playerInfoText.text = "Commentaires récupérés avec succès!";
            PopulateTable(response.comments); // Remplir le tableau avec les commentaires récupérés
        }
        else
        {
            playerInfoText.text = "Erreur : " + response.message;
        }
    }

    // Fonction appelée en cas d'erreur de la requête
    private void OnGetAllCommentsError(string error)
    {
        Debug.LogError("Erreur lors de la récupération des commentaires : " + error);
        playerInfoText.text = "Erreur de connexion.";
    }

    // Fonction pour remplir le tableau avec les commentaires
    void PopulateTable(List<Comment> comments)
    {
        // Supprimer les anciennes lignes (si nécessaire)
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Ajouter chaque ligne au tableau
        foreach (Comment comment in comments)
        {
            GameObject newRow = Instantiate(rowPrefab, content); // Instancier le Prefab

            // Remplir les colonnes avec les données
            TextMeshProUGUI[] columns = newRow.GetComponentsInChildren<TextMeshProUGUI>(); // Récupérer les colonnes

            columns[0].text = comment.user_login;
            columns[1].text = comment.commentaire;
        }
    }
    
    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    private void LoadAddCommentScene()
    {
        SceneManager.LoadScene("AddComment");
    }


}


