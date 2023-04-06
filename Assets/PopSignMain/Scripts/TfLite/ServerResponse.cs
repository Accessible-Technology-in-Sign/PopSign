using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerResponse
{
    public float dad;
    public float elephant;
    public float red;
    public float where;
    public float yellow;

    public string FindMaxLabel()
    {
        float maxValue = 0f;
        string maxLable = "";

        if(dad > maxValue)
        {
            maxValue = dad;
            maxLable = "dad";
        }

        if (elephant > maxValue)
        {
            maxValue = elephant;
            maxLable = "elephant";
        }

        if (red > maxValue)
        {
            maxValue = red;
            maxLable = "red";
        }

        if (where > maxValue)
        {
            maxValue = where;
            maxLable = "where";
        }

        if (yellow > maxValue)
        {
            maxValue = yellow;
            maxLable = "yellow";
        }

        return maxLable;

    }

}
