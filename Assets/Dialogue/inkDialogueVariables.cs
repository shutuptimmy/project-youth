using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class inkDialogueVariables
{
    private Dictionary<string, Ink.Runtime.Object> variables;

    public inkDialogueVariables(Story story)
    {
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in story.variablesState)
        {
            Ink.Runtime.Object value = story.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global variables: " + name + " = " + value);
        }
    }

    public void syncVarsAndStartListening(Story story)
    {
        // it's important that syncVarsToStory is before assigning the listener;
        syncVarsToStory(story);
        story.variablesState.variableChangedEvent += updateVarsState;
    }

    public void stopListening(Story story)
    {

        story.variablesState.variableChangedEvent -= updateVarsState;
    }

    public void updateVarsState(string name, Ink.Runtime.Object value)
    {
        // only maintain variables that were initialized from the globals ink file
        if (!variables.ContainsKey(name))
        {
            return;
        }
        variables[name] = value;
        Debug.Log("Updated dialogue var: " + name + " = " + value);
    }

    public void syncVarsToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
