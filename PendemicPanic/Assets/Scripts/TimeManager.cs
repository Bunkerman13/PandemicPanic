using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text timeText;
    int weekNum;
    int dayNum;
    int monthNum;
    int hourNum;
    bool incrementor;
    // Start is called before the first frame update
    void Start()
    {
        incrementor = true;
        timeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        hourNum = ((int)Time.time % 24) + 1;
        if(hourNum - 1 == 0 && incrementor)
        {
            dayNum++;
            incrementor = false;
            if (dayNum == 8)
            {
                weekNum++;
                dayNum = 0;
                if (weekNum == 5)
                {
                    monthNum++;
                    weekNum = 0;
                }
            }
        }
        
        if(hourNum - 2 == 0)
        {
            incrementor = true;
        }

        timeText.text = "Hour: " + hourNum + " - Day: " + dayNum + " - Week: " + weekNum + " - Month: " + monthNum;

        // debugging purposes
        if (Input.GetKey(KeyCode.W))
        {
            dayNum++;
            if (dayNum == 8)
            {
                weekNum++;
                dayNum = 0;
                if (weekNum == 5)
                {
                    monthNum++;
                    weekNum = 0;
                }
            }
        }
    }
}
