using UnityEngine;
using System.Collections;

//Observer for Opp movements which comes from server
public class OpponentMoveLogic : MonoBehaviour {

    public GameObject[] balls;
   
	// Use this for initialization
	void Start () {
        
	//register it self to socket
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void moveOpponent(float newX, float newY, int ballIndex,float speed)
    {

        balls[ballIndex].transform.position = new Vector3(newX, newY);
       // balls[ballIndex].transform.position = Vector2.MoveTowards(balls[ballIndex].transform.position,newPos,speed * Time.deltaTime);

    }


}
