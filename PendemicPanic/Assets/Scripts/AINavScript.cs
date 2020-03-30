using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavScript : MonoBehaviour
{
    public List<Vector3> loc;
    public List<string> locIDs;

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
    AIValues AI;

    bool canGoToStore;
    bool isGoingToSleep;
    float startTime;

    void Awake()
    {
        pathing = GetComponent<NavMeshAgent>();
        locations = new Dictionary<string, Vector3>();

        for (int x = 0; x < loc.Count; x++)
            locations.Add(locIDs[x], loc[x]);

        previousLocation = "Home";
        currentLocation = "Home";
        canGoToStore = true;
        isGoingToSleep = false;

        AI = GetComponent<AIValues>();

        //Initial supplies
        numOfSupplies = Random.Range(1, 5);
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
        // if AI CAN go to store, don't apply this extra detriment
        // if AI CANNOT go to store, this means they are already going and, therefore, apply the detriment
        if (!canGoToStore)
            AI.energy -= .01f;
        // check if the AI IS sleeping
        else if (isGoingToSleep)
            // if the AI is sleeping, recover energy
            // NOTE: AI sleeps for 8 'hours' (seconds) nonstop, and cannot do any other activities
            if (Time.time < startTime + 8f)
                AI.energy += .1f;
            else // after alloted time, 'wake up'
                isGoingToSleep = !isGoingToSleep;

        //AI behavior
        Automatic();
    }

    void Automatic()
    {
        //Auto use supply
        if(AI.hunger <= 30f)
        {
            UseSupplies();
        }

        //Auto go to sleep
        if(AI.energy <= 30f)
        {
            GoToSleep();
        }

        //Auto go to store
        if(numOfSupplies <= 1)
        {
            GoToStore();
        }

    }

    // sends AI to the store location
    // cannot do this if AI IS sleeping
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
        if (!isGoingToSleep)
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
        if (numOfSupplies > 0 && !isGoingToSleep)
        {
            numOfSupplies--;
            AI.health += 8;
            AI.hunger += 28;
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
        } // checks if AI made it home
        else if (collision.transform.tag == "Home")
        {
            // if so, add random number of supplies to supply cache
            // NOTE: currently a max of 5 supplies at a time allowed
            previousLocation = "Home";
            canGoToStore = true;
            numOfSupplies += Random.Range(1, 5);
            if (numOfSupplies > 5)
                numOfSupplies = 5;
        }

        // currently under construction
        // functionality for possibility of player getting infected for entering the 'store'
        if (collision.transform.tag == "InfectionZone")
        {
            //chanceOfInfection is an int number determined by population in store, 1 = 10%
            //Randomize a number between 0-10 to represent 0% to 100%
            if (Random.Range(0, 10) <= pop.chanceOfInfection)
            {
                GetComponent<AIValues>().infected = true;
            }
            //if (Random.Range(1, 21) + pop.population > 16 && player.infected != true)
            //player.infected = true;
        }
    }
}
