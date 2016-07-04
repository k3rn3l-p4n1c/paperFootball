using UnityEngine;
using System.Collections;
using Abrio;

public class BallLogic : MonoBehaviour
{

	const float MAX_SHOOT = 15;
	const float MIN_SHOOT = 1;

	//added for arrow
	public GameObject arrow;
	public float horizontalSpeed = 1F;
	public float verticalSpeed = 1F;

	public GameStateMachine StateMachine = GameStateMachine.i ();

	public GameObject goalRight;

	private GameLogic gameLogic;
	private Rigidbody2D rb;
	// Use this for initialization
	void Start ()
	{
		gameLogic = transform.parent.GetComponent<GameLogic> ();
		rb = GetComponent<Rigidbody2D> ();

		//added for arrow
		arrow.GetComponent<Renderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (goalRight.GetComponent<Renderer> ().bounds.Contains (new Vector3 (transform.position.x, transform.position.y, 0))) {
			gameLogic.StateMachine.Goal ();
			AbrioClient.instance.Send (OutterWorldState.i ().turn.ToString (), "ChTurn");


		}

		//StateMachine.Current () == GameStateMachine.State.DRAGGING ||
		if (gameLogic.ShootingBallName == name) {
			if (StateMachine.Current () == GameStateMachine.State.START) {
				//float h = horizontalSpeed * Camera.main.ScreenToWorldPoint (Input.mousePosition).x * Time.deltaTime;
				//float v = verticalSpeed * Camera.main.ScreenToWorldPoint (Input.mousePosition).y * Time.deltaTime;
				//Debug.Log ("h:" + h.ToString ());
				//Debug.Log ("v:" + v.ToString ());
				//Vector3 forArrow = new Vector3 (v, h, 0);
				//float nz = verticalSpeed*Camera.main.ScreenToWorldPoint (Input.mousePosition).z * Time.deltaTime;
				//Vector3 forArrow = new Vector3 (0, 0, nz);
				//arrow.transform.localRotation = Quaternion.Euler (forArrow);

				//Vector3 mousePos = Input.mousePosition;
				//mousePos.z = -(arrow.transform.position.x - Camera.main.transform.position.x);
				//mousePos.x = 0;
				//mousePos.y = 0;

				//Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
				//arrow.transform.LookAt (worldPos);

				Vector3 mousePos = Input.mousePosition;
				mousePos.z = 5.23f;

				Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
				mousePos.x = mousePos.x - objectPos.x;
				mousePos.y = mousePos.y - objectPos.y;

				float angle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;
				arrow.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));

				Vector2 rp = new Vector2 (rb.position.x - Camera.main.ScreenToWorldPoint (Input.mousePosition).x,
					            rb.position.y - Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
				arrow.transform.position = rb.position + rp / rp.magnitude;
			
				Vector3 forScale = new Vector3 (rp.magnitude, 1, 1);
				arrow.transform.localScale = forScale;
							


			}
		}

		if (rb.velocity.magnitude > 0.001)
			gameLogic.StateMachine.Move ();
	}

	void OnMouseDown ()
	{
		
		gameLogic.ShootingBallName = name;
		gameLogic.StateMachine.Drag ();

		//added for arrow

		Vector2 arrowPosition = new Vector2 ();
		//arrowPosition.x = rb.position.x;
		//arrowPosition.y = rb.position.y;
		//arrow.transform.position = arrowPosition;
		arrow.GetComponent<Renderer> ().enabled = true;
		//arrow.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);






	}

	void OnMouseUp ()
	{

		//added for arrow
		arrow.GetComponent<Renderer> ().enabled = false;


		switch (gameLogic.StateMachine.Current ()) {
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
				rb.AddForce (shoot * 2, ForceMode2D.Force);
				gameLogic.StateMachine.Shoot ();
				//audio.Play ();
			}
			break;
		}
	}
}
