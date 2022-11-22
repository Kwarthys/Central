using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string name = "Name";

    public string action = "";
    public string target = "";

    public float energy { get; private set; } = 100;

    public CharacterRoles.Role role;

    public int lostEnergyPerSec = 50;
    public int restingEnergyGainPerSec = 40;

    public short stamina = 50;
    public float staminaPotential = 1;

    public short strength= 50;
    public float strengthPotential = 1;

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

    public void startStateMachine()
    {
        stateMachine.startMachine();
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
        travelling = false;
    }

    public void updateEnergy(bool resting)
    {
        if(resting)
        {
            energy += Time.deltaTime * restingEnergyGainPerSec;
            if (energy > 100) energy = 100;
        }
        else
        {
            energy -= Time.deltaTime * lostEnergyPerSec;
            if (energy < 0) energy = 0;
        }
    }

    /* will use this one in interaction with the buildings, where changePerSec will be define per Building
    public void updateEnergy(float changePerSec)
    {
        energy += Time.deltaTime * changePerSec;
        energy = Mathf.Clamp(energy, 0, 100);
    }
    */

    public void live()
    {
        stateMachine.updateStateMachine();
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

    private bool askForWorkplace()
    {
        workplace = manager.askForWorkPlace();
        return workplace != null;
    }

    private void setupStateMachine()
    {
        stateMachine = new StateMachine();

        State resting = new State("Resting");
        State travellingToWork = new State("TravellingToWork");
        State working = new State("Working");
        State travellingToRest = new State("TravellingToRest");

        //State idle = new State("Idle");

        travellingToRest.setOnEnterBehaviour(travellingToRestEnterBehaviour);
        travellingToRest.addTransition(new Transition(resting, delegate () { return travelling == false; }));

        resting.setBehaviour(delegate () { updateEnergy(true); });
        resting.addTransition(new Transition(travellingToWork, delegate() { return energy > 90; }));

        travellingToWork.setOnEnterBehaviour(travellingToWorkEnterBehaviour);
        travellingToWork.addTransition(new Transition(working, delegate () { return travelling == false; }));

        working.setBehaviour(delegate () { updateEnergy(false); });
        working.addTransition(new Transition(travellingToRest, delegate () { return energy < 20; }));

        stateMachine.registerState(resting);
        stateMachine.registerState(working);
        stateMachine.registerState(travellingToRest, true);
        stateMachine.registerState(travellingToWork);
        //stateMachine.registerState(idle);
    }

    private void travellingToWorkEnterBehaviour()
    {
        if (askForWorkplace())
        {
            workplace.addUser(this);
            requestPathTo(workplace);
            travelling = true;
        }
        else
        {
            Debug.Log("Could not find any workplace");
        }
    }

    private void travellingToRestEnterBehaviour()
    {
        requestPathTo(house);
        travelling = true;

        // freeworkingbuilding
        if(workplace != null)
        {
            workplace.removeUser(this);
            workplace = null;
        }
    }
}
