using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySupplies : MonoBehaviour
{
    public Text suppliesDisplay;
    public PlayerNavScript supplies;

    // Start is called before the first frame update
    void Start()
    {
        suppliesDisplay = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        suppliesDisplay.text = "Supplies: " + supplies.numOfSupplies;
    }
}
