
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

using System.Net;
using System.IO;
using LitJson;

public class RestClient : MonoBehaviour
{

    string Url;
    void Start()
    {
       
       // GetData();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //Invoke this function where to want to make request.
    public void GetData()
    {
        Url = "http://www.mocky.io/v2/5748604510000020163cb344";
        WWWForm form = new WWWForm();
        form.AddField("username", "koosha");
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
        Url = "http://www.mocky.io/v2/574acf4010000034131b2ae7";

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
        Debug.Log(data["leageBasie"]);
        Debug.Log(data["leageSemiPro"]);
        Debug.Log(data["leagePro"]);
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

    public void GetUserInfo(string username)
    {
        Url = "http://www.mocky.io/v2/574ad1aa100000ad131b2ae9";

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
        Url = "http://  hossein/getEnemyInfo?username=";

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
