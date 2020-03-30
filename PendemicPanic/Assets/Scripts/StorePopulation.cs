using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePopulation : MonoBehaviour
{
    //how many people in shop
    public int population;
    //how much increase in chance of infection when 1 more people enter shop
    public int infectionIncremental;
    //current chance of infection when going to the shop
    //1 = 10% chance of infection, 2 = 20%
    public int chanceOfInfection;

    // Start is called before the first frame update
    void Start()
    {
        population = 0;
    }

    // Update is called once per frame
    void Update()
    {
        chanceOfInfection = infectionIncremental * population;
    }

    private void OnCollisionEnter(Collision collision)
    {
        population++;
    }

    private void OnCollisionExit(Collision collision)
    {
        population--;
    }

}
