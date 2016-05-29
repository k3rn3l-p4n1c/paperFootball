
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

using System.Net;
using System.IO;
using LitJson;
using UnityEngine.SceneManagement;

public class RestClient : MonoBehaviour
{

    string Url;
    public static string userNameState=null;
    public  string leagueBasicState=null;
    public  string leageSemiProState = null;
    public  string leagueProState = null;
    public  string userLevel = null;
    public  string userCoin = null;
    public  string enemyLevel = null;
    public  string enemyCoin = null;
    public  string playerRanking = null;
    public  string firstUsername = null;
    public  string secondUserName = null;
    public string enemyUserName = null;
    public static string thirdUsername = null;
    public static string allPalyerNumber = null;
    void Start()
    {
       
       // GetData();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //Invoke this function where to want to make request.
    public void sendUsernameReq(string username)
    {

        Url = "http://192.168.1.2:5000/sendUserName";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        //sending the request to url
        WWW www = new WWW(Url,form);
        Debug.Log("Befor Sending Req");
        //SendingRequest(www);
        // StartCoroutine("GetdataEnumerator",Url);
        StartCoroutine(GetdataEnumerator(www));
    }
    IEnumerator GetdataEnumerator(WWW www)
    {
        //Wait for request to complete
        yield return www;

        JsonData data = JsonMapper.ToObject(www.text);
        Debug.Log(data["state"]);
        userNameState = (string)data["state"];
        
        if (www.error != null)
        {
            string serviceData = www.text;
            //Data is in json format, we need to parse the Json.
            Debug.Log(serviceData);
        }
        else
        {
            Debug.Log("error" + www.error);
        }
    }

    public void GetOpenLeags(string username)
    {
        Url = "http://192.168.1.2:5000/getOpenLeagues?username="+username;

        WWW www = new WWW(Url);
        Debug.Log("Befor Sending Req");
        //SendingRequest(www);
        // StartCoroutine("GetdataEnumerator",Url);
        StartCoroutine(GetOpenLeagsdataEnumerator(www));
    }
    IEnumerator GetOpenLeagsdataEnumerator(WWW www)
    {
        //Wait for request to complete
        yield return www;
        
        JsonData data = JsonMapper.ToObject(www.text);

        Debug.Log(data["leagueBasic"]);
        Debug.Log(data["leagueSemiPro"]);
        Debug.Log(data["leaguePro"]);
        leagueBasicState = (string)data["leagueBasic"];
        leagueProState = (string)data["leaguePro"];
        leageSemiProState = (string)data["leagueSemiPro"];
        if (www.error != null)
        {
          //string serviceData = www.text;
          // Data is in json format, we need to parse the Json.
          // Debug.Log(serviceData);
        }
        else
        {
            Debug.Log("error" + www.error);
        }
    }

    public void GetUserInfo(string username)
    {
        Url = "http://192.168.1.2:5000/getUserInfo?username=" + username;

        WWW www = new WWW(Url);
        Debug.Log("Befor Sending Req");
        //SendingRequest(www);
        // StartCoroutine("GetdataEnumerator",Url);
        StartCoroutine(GetUserInfoEnumerator(www));
    }
    IEnumerator GetUserInfoEnumerator(WWW www)
    {
        //Wait for request to complete
        yield return www;

        JsonData data = JsonMapper.ToObject(www.text);
        Debug.Log(data["level"]);
        Debug.Log(data["coin"]);
        userLevel = (string)data["level"];
        userCoin = (string)data["coin"];
        if (www.error != null)
        {
            string serviceData = www.text;
            //Data is in json format, we need to parse the Json.
            Debug.Log(serviceData);
        }
        else
        {
            Debug.Log("error" + www.error);
        }
    }

    public void GetEnemyInfo()
    {
        Url = "http://192.168.1.2:5000/getEnemyInfo";

        WWW www = new WWW(Url);
        Debug.Log("Befor Sending Req");
        //SendingRequest(www);
        // StartCoroutine("GetdataEnumerator",Url);
        StartCoroutine(GetEnemyInfoEnumerator(www));
    }
    IEnumerator GetEnemyInfoEnumerator(WWW www)
    {
        //Wait for request to complete
        yield return www;

        JsonData data = JsonMapper.ToObject(www.text);
        Debug.Log(data["level"]);
        Debug.Log(data["coin"]);
        enemyLevel = (string)data["level"];
        enemyCoin = (string)data["coin"];
        enemyUserName = (string)data["username"];
        if (www.error != null)
        {
            string serviceData = www.text;
            //Data is in json format, we need to parse the Json.
            Debug.Log(serviceData);
        }
        else
        {
            Debug.Log("error" + www.error);
        }
    }

    public void GetLeaderBoardInfo(string username)
    {
        Url = "http://  hossein/getLeaderBoardInfo?username="+username;

        WWW www = new WWW(Url);
        Debug.Log("Befor Sending Req");
        //SendingRequest(www);
        // StartCoroutine("GetdataEnumerator",Url);
        StartCoroutine(GetLeaderBoardInfoEnumerator(www));
    }
    IEnumerator GetLeaderBoardInfoEnumerator(WWW www)
    {
        //Wait for request to complete
        yield return www;

        JsonData data = JsonMapper.ToObject(www.text);
        Debug.Log(data["1"]);
        Debug.Log(data["2"]);
        Debug.Log(data["3"]);
        Debug.Log(data["rank"]);
        Debug.Log(data["allPalyersNum"]);
        firstUsername = (string)data["1"];
        secondUserName = (string)data["2"];
        thirdUsername = (string)data["3"];
        playerRanking = (string)data["rank"];
        allPalyerNumber = (string)data["allPlayersNum"];
        if (www.error != null)
        {
            string serviceData = www.text;
            //Data is in json format, we need to parse the Json.
            Debug.Log(serviceData);
        }
        else
        {
            Debug.Log("error" + www.error);
        }
    }
    

}
