using System.Collections;
using System.Collections.Generic;

public class Transition
{
    private State to;

    public delegate bool conditionCallback();

    private conditionCallback condition;

    public Transition(State to, conditionCallback condition)
    {
        this.to = to;
        this.condition = condition;
    }

    public bool evaluate()
    {
        return condition();
    }

    public State getNextState() { return to; }
}
