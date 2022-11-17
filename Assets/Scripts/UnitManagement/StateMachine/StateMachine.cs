using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
    private List<State> states = new List<State>();

    private State currentState = null;

    public void registerState(State s)
    {
        if (states.Contains(s)) return;

        states.Add(s);
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
        if(currentState != null)
        {
            if(currentState.checkTransitions(out State newState))
            {
                setCurrentState(newState);
            }

            currentState.behave();
        }
    }


}
