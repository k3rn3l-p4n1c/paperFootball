using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBeginBtn : MonoBehaviour {

    Button myButton;
    public InputField Username;
    public static string inputUserName;
    RestClient rest;
    void Start()
    {

    }
    void Update()
    {
		if (RestClient.userNameState != null && RestClient.userNameState !="fail" )
        {
            SceneManager.LoadScene("League");
        }
    }
    void Awake()
    {
        myButton = GetComponent<Button>();
       
        myButton.onClick.AddListener(() => { OpenSceneOnClickEvent(); });  
        
    }

    void OpenSceneOnClickEvent()
    {
       
        string userID = Username.text.ToString();
        inputUserName = userID;

        rest = GameObject.Find("GameObject").GetComponent<RestClient>();
        rest.sendUsernameReq(userID);
              
    }

   
}
