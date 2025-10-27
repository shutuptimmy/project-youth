using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class questStepState
{
    public string state;
    public string status;

    public questStepState(string state, string status)
    {
        this.state = state;
        this.status = status;
    }

    public questStepState()
    {
        this.state = "";
        this.status = "";
    }

}
