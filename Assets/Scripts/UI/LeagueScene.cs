using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeagueScene : MonoBehaviour {
    public Text usernameText;
    public Text coinNumber;
    public RestClient rest;
    public Button basicLeagueBtn;
    public Button semiProLeagueBtn;
    public Button proLeagueBtn;
	// Use this for initialization
	void Start () {
        usernameText.text = MenuBeginBtn.inputUserName;
        //rest = GameObject.Find("GameObject").GetComponent<RestClient>();
        
        rest.GetOpenLeags(MenuBeginBtn.inputUserName);
        //if (rest.leagueBasicState != null)
        
            rest.GetUserInfo(MenuBeginBtn.inputUserName);
        
	}
	
	// Update is called once per frame
	void Update () {
        if (rest.leagueBasicState.Equals("close"))
        {
            //Debug.Log("basic");
            basicLeagueBtn.interactable = false;
        }
        if (rest.leageSemiProState.Equals("close"))
        {
            //Debug.Log("pro");
            semiProLeagueBtn.interactable = false;
        }
        if (rest.leagueProState.Equals("close"))
        {
          //  Debug.Log("expert");
            proLeagueBtn.interactable = false;
        }
        if (rest.userCoin != null)
        {
            coinNumber.text = rest.userCoin;
        }

	
	}
}
