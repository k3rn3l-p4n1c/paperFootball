using System;
using UnityEngine;


public class GameStateMachine{


	static public GameStateMachine instance = null;
	private State mCurrentState;
	public GameStateMachine() {
		mCurrentState = State.START;
	}
	public State Current(){
		return mCurrentState;
	}

	public static GameStateMachine i(){
		if (instance == null)
			instance = new GameStateMachine ();
		return instance;
	}
		
	public void SetTurn(int turn){
		switch (mCurrentState) {
		case State.START:
			if (turn == 1) {
				Debug.Log ("Your turn");
				mCurrentState = State.RESETTING;
			} else {
				Debug.Log ("Opp turn");
				mCurrentState = State.OPP_TURN;
			}
			break;
		}
	}

	public void Drag(){
		switch(mCurrentState){
		case State.KICK_OFF:
			mCurrentState = State.DRAGGING;
			Debug.Log ("New state: "+mCurrentState.ToString());
			break;
		}
	}

	public void Ignore(){
		switch(mCurrentState){
		case State.DRAGGING:
			mCurrentState = State.KICK_OFF;
			Debug.Log ("New state: "+mCurrentState.ToString());
			break;
		}
	} 

	public void Shoot(){
		switch(mCurrentState){
		case State.DRAGGING:
			mCurrentState = State.FIRST_CYCLE_OF_SHOOT;
			Debug.Log ("New state: "+mCurrentState.ToString());
			break;
		}
	}

	public void Move(){
		switch(mCurrentState){
		case State.FIRST_CYCLE_OF_SHOOT:
			mCurrentState = State.SHOOTING;
			Debug.Log ("New state: "+mCurrentState.ToString());
			break;
		}
	}
		

	public void BallPass(){
		switch (mCurrentState) {
		case State.SHOOTING:
			mCurrentState = State.OK_SHOOTING;
			Debug.Log ("New state: " + mCurrentState.ToString ());
			break;
		}
	}

	public void Goal(){
		switch(mCurrentState){
		case State.OK_SHOOTING:
			mCurrentState = State.OPP_TURN;
			Debug.Log ("New state: "+mCurrentState.ToString());
			break;
		}
	}

	public void Reach(){
		switch(mCurrentState){
		case State.RESETTING:
			mCurrentState = State.KICK_OFF;
			Debug.Log ("New state: "+mCurrentState.ToString());
			break;
		}
	}

	public void Stop(){
		switch(mCurrentState){
		case State.SHOOTING:
			mCurrentState = State.OPP_TURN;
			Debug.Log ("New state: "+mCurrentState.ToString());
			break;
		case State.OK_SHOOTING:
			mCurrentState = State.KICK_OFF;
			Debug.Log ("New state: "+mCurrentState.ToString());
			break;
		}
	}

	public void YourTurn(){
		switch (mCurrentState) {
		case State.OPP_TURN:
			mCurrentState = State.RESETTING;
			Debug.Log ("New state: "+mCurrentState.ToString());
			break;
		}
	}
		
	public enum State {
		START,
		RESETTING,
		KICK_OFF,
		DRAGGING,
		SHOOTING,
		FIRST_CYCLE_OF_SHOOT,
		OK_SHOOTING,
		OPP_TURN
	}
}