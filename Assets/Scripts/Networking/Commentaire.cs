using System.Collections.Generic;

[System.Serializable]
public class Comment
{
    public int commentaire_id;  // ID du commentaire
    public int user_id;  // ID de l'utilisateur
    public string user_login;  // Login de l'utilisateur
    public string commentaire;  // Texte du commentaire
}

[System.Serializable]
public class CommentsResponse
{
    public string status; // Statut de la requête
    public string message; // Message d'erreur ou succès
    public List<Comment> comments; // Liste des commentaires
}