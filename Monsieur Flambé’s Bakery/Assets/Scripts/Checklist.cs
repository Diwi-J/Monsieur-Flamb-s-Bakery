using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checklist : MonoBehaviour
{
    [System.Serializable]
    public class Task
    {
        public string description;
        public bool completed;
        public string requiredIngredientName;
        public GameObject checkmark;
    }

    public List<Task> tasks = new List<Task>();
    public TextMeshPro textDisplay;

    void Start()
    {
        // Initialize the checklist text display
        UpdateChecklistText();
    }

    public void CompleteTask(int index)
    {
        // Check if the index is valid
        if (index >= 0 && index < tasks.Count)
        {
            tasks[index].completed = true;
            UpdateChecklistText();
        }
    }

    void UpdateChecklistText()
    {
        // Generate the checklist text output
        string output = "";
        for (int i = 0; i < tasks.Count; i++)
        {
            output += (tasks[i].completed ? "[X] " : "[ ] ") + tasks[i].description + "\n";
        }

        if (textDisplay != null)
        {
            textDisplay.text = output;
        }
    }

    public void MarkIngredientAsAdded(string ingredientName)
    {
        // Find the task that matches the ingredient name and mark it as completed
        for (int i = 0; i < tasks.Count; i++)
        {
            if (!tasks[i].completed && tasks[i].requiredIngredientName == ingredientName)
            {
                tasks[i].completed = true;

                if (tasks[i].checkmark != null)
                    tasks[i].checkmark.SetActive(true);

                UpdateChecklistText();

                return;
            }
        }
    }
}
