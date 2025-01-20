using UnityEngine;
using UnityEngine.SceneManagement;

public class UserManager : MonoBehaviour
{
    private static UserManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("id_user"))
        {
            Debug.Log("Utilisateur non connecté, redirection vers la scène de login.");
            SceneManager.LoadScene("LoginScene"); 
        }
        else
        {
            Debug.Log("Utilisateur connecté.");
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("id_user");
        Debug.Log("id_user supprimé.");
    }
}