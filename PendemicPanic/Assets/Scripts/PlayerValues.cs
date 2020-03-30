using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerValues : MonoBehaviour
{
    // public values
    public float health;
    public float energy;
    public float hunger;

    // UI bars for the main player attributes
    public List<Slider> sliders;
    public bool infected;

    // infected level determines how strongly the infection affects the player attributes
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

        health = 100f;
        energy = 50f;
        hunger = 100f;

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
        // calculate values to affect player attributes
        decrementEnergy = .003f * ((10 - ((int)(hunger / 100f))) + 1);
        decrementHealth = .002f * (((10 - ((int)(hunger / 10f))) + 1) + (Mathf.Abs(energy - 50f) / 10f));

        // apply decrements
        hunger -= decrementHunger;
        energy -= decrementEnergy;
        health -= decrementHealth;

        // make sure the values do NOT go below zero
        // NOTE: will need to change to 'game over' functionality
        //if (health <= 0)
            //health = 0;
        //if (energy <= 0)
            //energy = 0;
        //if (hunger <= 0)
            //hunger = 0;

        if(health <= 0 || energy <= 0 || hunger <= 0)
        {
            GameOver();
        }


        // makes sure the values do NOT go over 100
        if (health >= 100)
            health = 100;
        if (energy >= 100)
            energy = 100;
        if (hunger >= 100)
            hunger = 100;

        // apply changes to sliders on UI
        sliders[0].value = health;
        sliders[1].value = energy;
        sliders[2].value = hunger;
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

    //When game over, display game over scene
   void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
