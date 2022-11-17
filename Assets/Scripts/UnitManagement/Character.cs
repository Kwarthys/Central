using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string name = "Name";

    public string action = "";
    public string target = "";

    public int energy { get; private set; } = 50;

    public int framesPerEnergyLost = 100;
    private int frameCounter = 0;

    public short stamina = 50;
    public float staminaPotential = 1;

    public short spirit = 50;
    public float spiritPotential = 1;

    public short agility = 50;
    public float agilityPotential = 1;

    public short courage = 50;
    public float couragePotential = 1;

    public Building workplace;
    public Building house;

    private bool travelling = false;

    public CharacterManager manager;

    private CharacterBody associatedBody = null;

    private StateMachine stateMachine;

    public Character(CharacterManager manager)
    {
        this.manager = manager;
        setupStateMachine();
    }

    public void setBody(CharacterBody associatedBody)
    {
        this.associatedBody = associatedBody;
        this.associatedBody.registerNewReachDestinationCallback(destinationReachedCallBack);
    }

    public CharacterBody getBody()
    {
        return associatedBody;
    }

    private void destinationReachedCallBack()
    {
        Debug.Log("Char-Has Arrived !");
        travelling = false;
    }

    public void live()
    {
        if(frameCounter++ >= framesPerEnergyLost)
        {
            frameCounter = 0;
            if(energy > 0)energy--;

            Debug.Log("Energy: " + energy);
        }
    }

    public bool needsRest()
    {
        return energy < 20;
    }

    private bool requestPathTo(Building b)
    {
        Vector3[] path = manager.requestPathTo(b, this);

        if(path!=null)
        {
            for(int i = 0; i < path.Length; ++i)
            {
                Debug.DrawLine(path[i], path[i] + Vector3.up * i * 0.5f, Color.red, 10);
            }

            associatedBody.registerNewPathToFollow(path);
        }

        return path != null;
    }

    private void setupStateMachine()
    {
        stateMachine = new StateMachine();

        State resting = new State();
        State travellingToWork = new State();
        State working = new State();
        State travellingToRest = new State();

        resting.setBehaviour(delegate () { energy += 1; });
        resting.addTransition(new Transition(travellingToWork, delegate() { return energy > 95; }));

        travellingToRest.setOnEnterBehaviour(delegate () { requestPathTo(house); travelling = true;/*freeworkingbuilding*/});
        travellingToRest.addTransition(new Transition(resting, delegate () { return travelling == false; }));

        working.setBehaviour(delegate () { stamina++; });
        working.addTransition(new Transition(travellingToRest, delegate () { return needsRest(); }));

        travellingToWork.setOnEnterBehaviour(delegate () { requestPathTo(workplace); travelling = true; });
        travellingToWork.addTransition(new Transition(working, delegate () { return travelling == false; }));
    }
}
