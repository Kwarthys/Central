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

    private bool goingToRest = false;

    public CharacterManager manager;

    public CharacterBody associatedBody = null;


    public Character(CharacterManager manager)
    {
        this.manager = manager;
    }

    public void live()
    {
        if(frameCounter++ >= framesPerEnergyLost)
        {
            frameCounter = 0;
            if(energy > 0)energy--;

            Debug.Log("Energy: " + energy);
        }

        if(!goingToRest)
        {
            if(needsRest())
            {
                goingToRest = true;
                requestPathTo(house);
            }
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
}
