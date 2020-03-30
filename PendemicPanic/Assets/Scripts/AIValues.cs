using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIValues : MonoBehaviour
{
    // public values
    public float health;
    public float energy;
    public float hunger;

    //
    public bool infected;

    // infected level determines how strongly the infection affects the AI attributes
    int infectedLevel;
    bool incrementor;

    // values calculated to change player attributes
    float decrementHealth;
    float decrementEnergy;
    float decrementHunger;

    // Start is called before the first frame update
    void Start()
    {
        infected = false;
        infectedLevel = 0;
        incrementor = true;

        //Randomize initial Values
        health = 100f;
        energy = Random.Range(30,70);
        hunger = Random.Range(50,100);

        decrementHunger = .04f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateValues();
        if (infected)
            InfectedValues();
    }

    void UpdateValues()
    {
        // calculate values to affect AI attributes
        decrementEnergy = .003f * ((10 - ((int)(hunger / 100f))) + 1);
        decrementHealth = .002f * (((10 - ((int)(hunger / 10f))) + 1) + (Mathf.Abs(energy - 50f) / 10f));

        // apply decrements
        hunger -= decrementHunger;
        energy -= decrementEnergy;
        health -= decrementHealth;

        // When status below 0, revive
        if (health <= 0 || energy <= 0 || hunger <= 0)
        {
            health = 100f;
            energy = Random.Range(30, 70);
            hunger = Random.Range(50, 100);
            infected = false;
        }

        // makes sure the values do NOT go over 100
        if (health >= 100)
            health = 100;
        if (energy >= 100)
            energy = 100;
        if (hunger >= 100)
            hunger = 100;
    }

    void InfectedValues()
    {
        // checks if day ticks over
        if ((int)Time.time % 24 == 0 && incrementor)
        {
            // increments level every day
            infectedLevel++;

            // while under level 7, apply normally
            if (infectedLevel < 7)
            {
                health -= infectedLevel / 100f - 0.2f;
                energy -= infectedLevel / 100f - 0.2f;
                hunger -= infectedLevel / 100f - 0.2f;
            }
            else // while above level 7, subtract by 14 to show the infection going down
            {
                health -= (14 - infectedLevel) / 100f;
                energy -= (14 - infectedLevel) / 100f;
                hunger -= (14 - infectedLevel) / 100f;
            }
            incrementor = false;
        }

        // changes incrementor when it is NOT the tick over day
        // NOTE: this code prevents the above 'if statement' from getting applied more than once per day
        if ((int)Time.time % 24 != 0)
            incrementor = true;

        if (infectedLevel == -1)
        { infected = false; infectedLevel = 0; }

        // debug purposes
        if (infectedLevel < 7)
            Debug.Log(infectedLevel);
        else
            Debug.Log(14 - infectedLevel);
    }

}
