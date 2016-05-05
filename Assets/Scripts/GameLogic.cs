using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {
	const int RESET_SPEED = 6;

	public GameObject ball1,ball2,ball3;

	public GameStateMachine StateMachine = new GameStateMachine ();
	public string ShootingBallName;

	// Use this for initialization
	void Start () {
		SocketHandler socket = new SocketHandler (this);
	}

    public void moveOpponent(Vector3 newPosition, int ballIndex,Vector3 newVelocity)
    {
        switch (ballIndex)
        {
            case 1:
                ball1.transform.position = newPosition;
                ball1.GetComponent<Rigidbody>().velocity = newVelocity;
                break;
            case 2:
                ball2.transform.position = newPosition;
                ball2.GetComponent<Rigidbody>().velocity = newVelocity;
                break;
            case 3:
                ball3.transform.position =newPosition;
                ball3.GetComponent<Rigidbody>().velocity = newVelocity;
                break;

        }
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
		if (!checkBallMoving ())
			StateMachine.Stop ();
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
