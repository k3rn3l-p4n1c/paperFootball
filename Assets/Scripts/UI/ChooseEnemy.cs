using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChooseEnemy : MonoBehaviour {
    public Text enemyUsername;
    public Text enemyCoin;
    public Text enemyLevel;
    public RestClient rest;
	// Use this for initialization
	void Start () {
        
        rest.GetEnemyInfo();
	}
	
	// Update is called once per frame
	void Update () {
        if (rest.enemyCoin != null)
        {
            enemyCoin.text = rest.enemyCoin;
        }
        if (rest.enemyLevel != null)
        {
            enemyLevel.text = rest.enemyLevel;
        }
        if (rest != null)
        {
            enemyUsername.text = rest.enemyUserName;
        }
	
	}
}
