using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class expertLeague : MonoBehaviour
{

    Button myButton;

    void Awake()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(() => { OpenSceneOnClickEvent(); });

    }

    void OpenSceneOnClickEvent()
    {
        SceneManager.LoadScene("ChooseEnemy");

    }


}