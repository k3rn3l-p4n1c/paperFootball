
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
    IEnumerator SendingRequest(WWW www)
    {
        Debug.Log("Before yield");
        yield return www;

        Debug.Log(www.error);
        Debug.Log(www.text);

        if (www.error == null)
        {
            string msg = www.text;
            Debug.Log("»[[[[[[ۀۀۀۀۀۀ" + msg);



            // ParseServerSendingValidate(www.text);
        }

    }

}
