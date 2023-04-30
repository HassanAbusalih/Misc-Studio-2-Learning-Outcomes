using System.IO;
using TMPro;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] string path = "/testSave";
    [SerializeField] KeyCode nextStateKey = KeyCode.Space;
    [SerializeField] TextMeshProUGUI stateText;
    [SerializeField] TextMeshProUGUI unsortedText;
    [SerializeField] TextMeshProUGUI ascendingText;
    [SerializeField] TextMeshProUGUI descendingText;
    [SerializeField] TextMeshProUGUI pressSpaceText;
    State currentState;
    Arrays arrays = new();

    void Start()
    {
        currentState = State.Load;
    }

    void Update()
    {
        if (Input.GetKeyDown(nextStateKey))
        {
            if (pressSpaceText.enabled)
            {
                pressSpaceText.enabled = false;
            }
            switch (currentState)
            {
                case State.Load:
                    stateText.text = "Loaded";
                    Load();
                    currentState = State.Generate;
                    break;
                case State.Generate:
                    stateText.text = "Generated New Array";
                    GenerateNumbers();
                    currentState = State.Sort;
                    break;
                case State.Sort:
                    stateText.text = "Sorted";
                    Sort();
                    currentState = State.Save;
                    break;
                case State.Save:
                    stateText.text = "Saved";
                    Save();
                    currentState = State.Load;
                    break;
            }
        }
    }

    void ShowArray(string label, int[] array, TextMeshProUGUI textMeshPro)
    {
        for (int i = 0; i < array.Length; i++)
        {
            label += array[i].ToString();
            if (i < array.Length - 1)
            {
                label += ", ";
            }
        }
        textMeshPro.text = label;
    }

    void Sort()
    {
        arrays.descending = SortQueue.SortDescending(arrays.unsorted);
        arrays.ascending = SortQueue.SortAscending(arrays.unsorted);
        UpdateUI();
    }


    void UpdateUI()
    {
        ShowArray("Unsorted: ", arrays.unsorted, unsortedText);
        ShowArray("Ascending: ", arrays.ascending, ascendingText);
        ShowArray("Descending: ", arrays.descending, descendingText);
    }

    void GenerateNumbers()
    {
        arrays.unsorted = arrays.descending = arrays.ascending = new int[Random.Range(5, 10)];
        for (int i = 0; i < arrays.unsorted.Length; i++)
        {
            arrays.unsorted[i] = Random.Range(-500, 500);
        }
        ShowArray("Unsorted: ", arrays.unsorted, unsortedText);
        descendingText.text = ascendingText.text = string.Empty;
    }
    void Load()
    {
        if(File.Exists(Application.persistentDataPath + path))
        {
            string loadedData = File.ReadAllText(Application.persistentDataPath + path);
            JsonUtility.FromJsonOverwrite(loadedData, arrays);
            UpdateUI();
        }
        else
        {
            stateText.text = "No Data to Load";
        }
    }

    void Save()
    {
        string savedData = JsonUtility.ToJson(arrays, true);
        File.WriteAllText(Application.persistentDataPath + path, savedData);
    }
}

[System.Serializable]
public class Arrays
{
    public int[] descending;
    public int[] ascending;
    public int[] unsorted;
}

public enum State
{
    Load,
    Generate,
    Sort,
    Save
}
