using System.Collections;
using System.Collections.Generic;

public class State
{
    public delegate void stateBehaviour();

    private stateBehaviour onEnter;
    private stateBehaviour onExit;
    private stateBehaviour behaviour;

    private List<Transition> transitions = new List<Transition>();

    public void setOnEnterBehaviour(stateBehaviour callback)
    {
        onEnter = callback;
    }

    public void setOnExitBehaviour(stateBehaviour callback)
    {
        onExit = callback;
    }

    public void setBehaviour(stateBehaviour callback)
    {
        behaviour = callback;
    }

    public void enter() { onEnter?.Invoke(); }
    public void exit() { onExit?.Invoke(); }
    public void behave() { behaviour?.Invoke(); }

    public void addTransition(Transition t)
    {
        if(!transitions.Contains(t))
        {
            transitions.Add(t);
        }
    }

    public bool checkTransitions(out State newState)
    {
        for(int i = 0; i < transitions.Count; ++i)
        {
            if(transitions[i].evaluate())
            {
                newState = transitions[i].getNextState();
                return true;
            }
        }

        newState = null;
        return false;
    }
}
