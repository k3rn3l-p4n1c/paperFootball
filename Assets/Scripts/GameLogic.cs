using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {
	const int RESET_SPEED = 6;
	const float SEND_EVENT_DELAY = 0.0f;

	public GameObject ball1,ball2,ball3,leftGoalKeeper,rightGoalKeeper;
	public GameStateMachine StateMachine = GameStateMachine.i();
	public string ShootingBallName;

	private float lastSendEventTime = 0.0f;
	private OutterWorldState outterWorldState;
	private SocketHandler socket;
    private RestClient rest;
    
	// Use this for initialization
	void Start () {
        rest = GameObject.Find("WS_Client").GetComponent<RestClient>();
        Debug.Log("Before START");
        rest.GetData();
		outterWorldState = new OutterWorldState ();
		socket = new SocketHandler (outterWorldState,this);
	}

	private void updateOppenent()
    {
		Debug.Log ("Debug");
		outterWorldState.Read ();
		ball1.transform.position = outterWorldState.ball1Pos;
		ball1.GetComponent<Rigidbody2D>().velocity = outterWorldState.ball1Vel;
		ball2.transform.position = outterWorldState.ball2Pos;
		ball2.GetComponent<Rigidbody2D>().velocity = outterWorldState.ball2Vel;
		ball3.transform.position =outterWorldState.ball3Pos;
		ball3.GetComponent<Rigidbody2D>().velocity = outterWorldState.ball3Vel;
    }
	
	// Update is called once per frame
	void Update () {
		switch (StateMachine.Current ()) {
		case GameStateMachine.State.SHOOTING:
			if (checkBallPassed ())
				StateMachine.BallPass ();
			break;
		case GameStateMachine.State.RESETTING:
            
			ball1.transform.position = Vector2.MoveTowards (ball1.transform.position, ResetPoints.i ().pos1, RESET_SPEED * Time.deltaTime);
			ball2.transform.position = Vector2.MoveTowards (ball2.transform.position, ResetPoints.i ().pos2, RESET_SPEED * Time.deltaTime);
			ball3.transform.position = Vector2.MoveTowards (ball3.transform.position, ResetPoints.i ().pos3, RESET_SPEED * Time.deltaTime);
			if ((ball1.transform.position - ResetPoints.i ().pos1).magnitude < 0.5f
			   && (ball2.transform.position - ResetPoints.i ().pos2).magnitude < 0.5f
			   && (ball3.transform.position - ResetPoints.i ().pos3).magnitude < 0.5f)
				StateMachine.Reach ();
			break;
		}

		leftGoalKeeper.SetActive (StateMachine.Current() == GameStateMachine.State.OPP_TURN);
		rightGoalKeeper.SetActive (StateMachine.Current() != GameStateMachine.State.START && StateMachine.Current() != GameStateMachine.State.OPP_TURN);
			

		if (!checkBallMoving ()) {
			if (StateMachine.Current () == GameStateMachine.State.SHOOTING) {
				socket.Send (outterWorldState.turn.ToString(), "ChTurn");
			}
			StateMachine.Stop ();
		}

		//send to server
		if (StateMachine.Current () != GameStateMachine.State.OPP_TURN) {
			if (Time.time > lastSendEventTime + SEND_EVENT_DELAY) {
				lastSendEventTime = Time.time;
				socket.Send (outterWorldState.turn.ToString() ,string.Format("Ball {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}"
					,ball1.transform.position.x,ball1.transform.position.y, ball1.GetComponent<Rigidbody2D>().velocity.x,ball1.GetComponent<Rigidbody2D>().velocity.y
					,ball2.transform.position.x,ball2.transform.position.y, ball2.GetComponent<Rigidbody2D>().velocity.x,ball2.GetComponent<Rigidbody2D>().velocity.y
					,ball3.transform.position.x,ball3.transform.position.y, ball3.GetComponent<Rigidbody2D>().velocity.x,ball3.GetComponent<Rigidbody2D>().velocity.y));
			} 
		}
		// receive from server
		else{
			//if (outterWorldState.IsReady () ) 
				//updateOppenent ();
		}
        
		//Debug.Log (checkBallMoving ());
	}

	private bool checkBallPassed(){
		Vector3 A = ball1.transform.position;
		Vector3 B = ball2.transform.position;
		Vector3 C = ball3.transform.position;

		float AB = (A - B).magnitude;
		float AC = (A - C).magnitude;
		float BC = (B - C).magnitude;
		if (Mathf.Abs (AB + AC - BC) < 0.01f)
			return true;
		if (Mathf.Abs (AB + BC - AC) < 0.01f)
			return true;
		if (Mathf.Abs (BC + AC - AB) < 0.01f)
			return true;
		return false;
	}

	private bool checkBallMoving(){
		const float MIN_SPEED = 0.05f;
		if (ball1.GetComponent<Rigidbody2D> ().velocity.magnitude > MIN_SPEED)
			return true;
		if (ball2.GetComponent<Rigidbody2D> ().velocity.magnitude > MIN_SPEED)
			return true;
		if (ball3.GetComponent<Rigidbody2D> ().velocity.magnitude > MIN_SPEED)
			return true;
		return false;
	}

	public class ResetPoints{
		public Vector3 pos1, pos2, pos3;
		static private ResetPoints instance=null;
		private ResetPoints(){
			pos1 = new Vector3(Random.Range (-6, 0), Random.Range (-3, 3), 0);
			pos2 = new Vector3(Random.Range (-6, 0), Random.Range (-3, 3), 0);
			pos3 = new Vector3(Random.Range (-6, 0), Random.Range (-3, 3), 0);
		}
		static public void clear(){
			instance = null;
		}
		static public ResetPoints i(){
			if (instance == null)
				instance = new ResetPoints ();
			return instance;
		}
	}
}
