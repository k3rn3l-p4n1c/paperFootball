using UnityEngine;
using System.Collections;
using Abrio;

public class BallLogic : MonoBehaviour {

	const float MAX_SHOOT = 15;
	const float MIN_SHOOT = 1;

	//added for arrow
	public GameObject arrow;
	public float horizontalSpeed = 2.0F;
	public float verticalSpeed = 2.0F;

	public GameStateMachine StateMachine = GameStateMachine.i();

	public GameObject goalRight;

	private GameLogic gameLogic;
	private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		gameLogic = transform.parent.GetComponent<GameLogic>();
		rb = GetComponent<Rigidbody2D> ();

		//added for arrow
		arrow.GetComponent<Renderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (goalRight.GetComponent<Renderer> ().bounds.Contains (new Vector3 (transform.position.x, transform.position.y, 0))) {
			gameLogic.StateMachine.Goal ();

			if (StateMachine.Current () == GameStateMachine.State.SHOOTING) {
				AbrioClient.instance.Send (OutterWorldState.i().turn.ToString(), "ChTurn");
			}

		}

		if (StateMachine.Current () == GameStateMachine.State.DRAGGING) {
			float h = horizontalSpeed * Camera.main.ScreenToWorldPoint (Input.mousePosition).x;
			float v = verticalSpeed * Camera.main.ScreenToWorldPoint (Input.mousePosition).y;
			Debug.Log ("h:" + h.ToString ());
			arrow.transform.Rotate(v, h, 0);
		}

		if (rb.velocity.magnitude > 0.001)
			gameLogic.StateMachine.Move ();
	}

	void OnMouseDown() {
		gameLogic.ShootingBallName = name;
		gameLogic.StateMachine.Drag ();


		//added for arrow

		Vector2 arrowPosition = new Vector2 ();
		arrowPosition.x = rb.position.x + 1;
		arrowPosition.y = rb.position.y;
		arrow.transform.position = arrowPosition;
		arrow.GetComponent<Renderer> ().enabled = true;
		//arrow.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);





	}

	void OnMouseUp(){

		//added for arrow
		arrow.GetComponent<Renderer> ().enabled = false;


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
