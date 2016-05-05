using System;
using UnityEngine;


public class GameStateMachine{
	//private State mCurrentState = State.RESETTING;
    private State mCurrentState = State.OPP_TURN;
	public State Current(){
		return mCurrentState;
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
			mCurrentState = State.RESETTING;
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

    

	public enum State{
		RESETTING,
		KICK_OFF,
		DRAGGING,
		SHOOTING,
		FIRST_CYCLE_OF_SHOOT,
		OK_SHOOTING,
		OPP_TURN
	}
}