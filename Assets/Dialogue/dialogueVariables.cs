using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class dialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    public dialogueVariables(TextAsset loadGlobalsJSON)
    {
        // create the story
        Story globalVarStory = new Story(loadGlobalsJSON.text);

        // initialize the dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVarStory.variablesState)
        {
            Ink.Runtime.Object value = globalVarStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    public void startListening(Story story)
    {
        // its important that variablesToStory is before assigning to listener!
        variablesToStory(story);
        story.variablesState.variableChangedEvent += varChanged;
    }

    public void stopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= varChanged;
    }

    private void varChanged(string name, Ink.Runtime.Object value)
    {
        // only maintain variables that were initialized from the globals ink file
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    private void variablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
