using UnityEngine;
using System.Collections;

public class BallLogic : MonoBehaviour {

	const float MAX_SHOOT = 15;
	const float MIN_SHOOT = 1;

	public GameStateMachine StateMachine = GameStateMachine.i();

	public GameObject goalRight;

	private GameLogic gameLogic;
	private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		gameLogic = transform.parent.GetComponent<GameLogic>();
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (goalRight.GetComponent<Renderer> ().bounds.Contains (new Vector3 (transform.position.x, transform.position.y, 0))) {
			gameLogic.StateMachine.Goal ();
			SocketHandler.instance.Send (OutterWorldState.i().turn.ToString(), "ChTurn");
		}
		if (rb.velocity.magnitude > 0.001)
			gameLogic.StateMachine.Move ();
	}

	void OnMouseDown() {
		gameLogic.ShootingBallName = name;
		gameLogic.StateMachine.Drag ();
	}

	void OnMouseUp(){

		switch (gameLogic.StateMachine.Current()) {
		case GameStateMachine.State.DRAGGING:
			Vector2 shoot = new Vector2 ();
			shoot.x = rb.position.x - Camera.main.ScreenToWorldPoint (Input.mousePosition).x;
			shoot.y = rb.position.y - Camera.main.ScreenToWorldPoint (Input.mousePosition).y;

			if (shoot.magnitude > MAX_SHOOT)
				shoot = shoot / shoot.magnitude * MAX_SHOOT;
			if (shoot.magnitude < MIN_SHOOT) {
				gameLogic.StateMachine.Ignore ();
				Debug.Log ("Ignore shoot");
			} else {
				rb.AddForce (shoot*2, ForceMode2D.Force);
				gameLogic.StateMachine.Shoot ();
				//audio.Play ();
			}
			break;
		}
	}
}
