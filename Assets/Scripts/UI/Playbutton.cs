using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Playbutton : MonoBehaviour
{

    Button myButton;

    void Awake()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(() => { OpenSceneOnClickEvent(); });

    }

    void OpenSceneOnClickEvent()
    {
        SceneManager.LoadScene("Arena");

    }


}
