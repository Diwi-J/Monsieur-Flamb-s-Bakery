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

    private void Start()
    {
        UpdateChecklistText();
    }

    public void MarkIngredientAsAdded(string ingredientName)
    {
        foreach (var task in tasks)
        {
            if (!task.completed && task.requiredIngredientName == ingredientName)
            {
                task.completed = true;
                if (task.checkmark != null)
                    task.checkmark.SetActive(true);

                UpdateChecklistText();
                return;
            }
        }
    }

    private void UpdateChecklistText()
    {
        string output = "";
        foreach (var task in tasks)
        {
            output += (task.completed ? "[X] " : "[ ] ") + task.description + "\n";
        }

        if (textDisplay != null)
            textDisplay.text = output;
    }
}
