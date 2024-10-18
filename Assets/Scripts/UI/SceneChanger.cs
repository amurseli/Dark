using UnityEngine;
using UnityEngine.SceneManagement; // Para manejar las escenas
using UnityEngine.UI; // Para manejar la UI

public class SceneChanger : MonoBehaviour
{
    public Button changeSceneButton; 
    public string sceneName; 

    private void Start()
    {
        changeSceneButton.onClick.AddListener(ChangeScene);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
