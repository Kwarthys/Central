using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine
{
    private List<State> states = new List<State>();

    private State currentState = null;

    public bool go { get; private set; } = false;

    public void startMachine()
    {
        go = true;
        if(currentState != null)
        {
            currentState.enter();
        }
    }

    public void registerState(State s)
    {
        if (states.Contains(s)) return;

        states.Add(s);
    }

    public void registerState(State s, bool makeItCurrent)
    {
        registerState(s);
        if(makeItCurrent) currentState = s;
    }

    public void setCurrentState(State newState)
    {
        if(currentState != null)
        {
            currentState.exit();
        }

        currentState = newState;
        currentState.enter();
    }

    public void updateStateMachine()
    {
        if (!go) return;

        if(currentState != null)
        {
            if(currentState.checkTransitions(out State newState))
            {
                //Debug.Log("Switch state from " + currentState.stateName + " to " + newState.stateName);
                setCurrentState(newState);
            }

            currentState.behave();
        }
    }
}
