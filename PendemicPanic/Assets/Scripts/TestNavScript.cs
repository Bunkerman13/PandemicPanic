﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavScript : MonoBehaviour
{
    //
    // data structures used to storing location data
    public List<Vector3> test;
    public List<string> textIDs;

    // maintains immediate location history
    public string currentLocation;
    public string previousLocation;

    public int numOfSupplies;

    // counts population currently in the store
    // NOTE: currently doesn't count right as it is counting the walls and ground as people as well due to the box colliders
    public StorePopulation pop;

    //
    NavMeshAgent pathing;
    Dictionary<string, Vector3> locations;
    PlayerValues player;

    bool canGoToStore;
    bool isGoingToSleep;
    float startTime;

    void Awake()
    {
        pathing = GetComponent<NavMeshAgent>();
        locations = new Dictionary<string, Vector3>();

        for (int x = 0; x < test.Count; x++)
            locations.Add(textIDs[x], test[x]);

        previousLocation = "Home";
        currentLocation = "Home";
        canGoToStore = true;
        isGoingToSleep = false;

        player = GetComponent<PlayerValues>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // set start position
        pathing.SetDestination(locations[currentLocation]);
        pathing.destination = locations[currentLocation];
    }

    // Update is called once per frame
    void Update()
    {
        // if player CAN go to store, don't apply this extra detriment
        // if player CANNOT go to store, this means they are already going and, therefore, apply the detriment
        if (!canGoToStore)
            player.energy -= .01f;
        // check if the player IS sleeping
        else if (isGoingToSleep)
            // if the player is sleeping, recover energy
            // NOTE: player sleeps for 8 'hours' (seconds) nonstop, and cannot do any other activities
            if (Time.time < startTime + 8f)
              player.energy += .1f;
            else // after alloted time, 'wake up'
                isGoingToSleep = !isGoingToSleep;
    }

    // sends player to the store location
    // cannot do this if player IS sleeping
    public void GoToStore()
    {
        if (canGoToStore && !isGoingToSleep)
        {
            canGoToStore = false;
            previousLocation = "Home";
            currentLocation = "Store";
            pathing.SetDestination(locations[currentLocation]);
        }
    }

    // going to sleep will replenish energy
    // NOTE: once asleep, you CANNOT force yourself to wake up and will sleep for 8 'hours' (seconds)
    public void GoToSleep()
    {
        if(!isGoingToSleep)
        {
            isGoingToSleep = true;
            previousLocation = "Home";
            currentLocation = "Home";
            startTime = Time.time;
        }
    }

    // 'uses' supplies to heal health and hunger
    // cannot do this if player IS sleeping
    public void UseSupplies()
    {
        if(numOfSupplies > 0 && !isGoingToSleep)
        {
            numOfSupplies--;
            player.health += 8;
            player.hunger += 28;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // checks if store has been entered
        if (collision.transform.tag == "Store")
        {
            // if so, send back home
            previousLocation = "Store";
            currentLocation = "Home";
            pathing.SetDestination(locations[currentLocation]);
        } // checks if player made it home
        else if(collision.transform.tag == "Home")
        {
            // if so, add random number of supplies to supply cache
            // NOTE: currently a max of 5 supplies at a time allowed
            previousLocation = "Home";
            canGoToStore = true;
            numOfSupplies += Random.Range(0, 5);
            if (numOfSupplies > 5)
                numOfSupplies = 5;
        }
        
        // currently under construction
        // functionality for possibility of player getting infected for entering the 'store'
        if(collision.transform.tag == "InfectionZone")
        {
            //if (Random.Range(1, 21) + pop.population > 16 && player.infected != true)
                //player.infected = true;
        }
    }
}
