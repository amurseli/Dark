using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISingleton : MonoBehaviour
{
    private static UISingleton _instance;
    public TextMeshProUGUI scoreText;

    public static UISingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                // Busca una instancia en la escena
                _instance = FindObjectOfType<UISingleton>();

                // Si no existe, crea un nuevo GameObject con el singleton
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SimpleSingleton");
                    _instance = singletonObject.AddComponent<UISingleton>();
                }
            }

            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (_instance != this)
        {
            Destroy(gameObject);  
        }
    }

    public void addScore()
    {
        scoreText.text = (int.Parse(scoreText.text) + 1).ToString();
    }
}
