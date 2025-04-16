using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DailyTasksManager : MonoBehaviour
{
    [SerializeField] public List<DailyTaskData> _dailyTasks;

    [SerializeField] public List<int> _currentTasks;

    [SerializeField] public List<int> _doneTasks;
    [SerializeField] public List<int> _receivedTasks;
    [SerializeField] public TMP_Text _doneTaskText;

    private const string LastUpdateKey = "LastUpdateKey";
    private const int HoursToWait = 24;
    private const char Separator = ',';

    void Start()
    {
        DailyTaskEvents.OnTaskDone += DoTask; 
        

        _doneTaskText.text = (_doneTasks.Count - _receivedTasks.Count).ToString();
        CheckAndUpdateDailyTasks();
    }

    private void OnDestroy()
    {
        DailyTaskEvents.OnTaskDone -= DoTask;
    }
    public void CheckAndUpdateDailyTasks()
    {
        string lastUpdateString = PlayerPrefs.GetString(LastUpdateKey, "");

        if (string.IsNullOrEmpty(lastUpdateString))
        {
            UpdateDailyTasks();
           return;
        }
        else
        {
            _currentTasks = LoadIntList("CurrentTasks");
            CheckDoneTasks();
        }

        DateTime lastUpdate = DateTime.Parse(lastUpdateString);
        DateTime currentDateTime = DateTime.Now;

        if ((currentDateTime - lastUpdate).TotalHours >= 24)
        {
            UpdateDailyTasks();
            return;
        }
        else
        {
            _currentTasks = LoadIntList("CurrentTasks");
            CheckDoneTasks();
        }
    }
    private void UpdateDailyTasks()
    {
        _currentTasks = GenerateUniqueRandomNumbers(5, 0, 9);
        SaveIntList("CurrentTasks", _currentTasks);
        _doneTasks.Clear();
        _receivedTasks.Clear();
       
        CleanTaskSaves();
        PlayerPrefs.SetString(LastUpdateKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void CleanTaskSaves()
    {
        PlayerPrefs.DeleteKey(SaveDailyKey.Judge5Souls);
        PlayerPrefs.DeleteKey(SaveDailyKey.Judge10SoulsOneDay);
        PlayerPrefs.DeleteKey(SaveDailyKey.Send2SoulsHeavenlyRealm);
        PlayerPrefs.DeleteKey(SaveDailyKey.Judge10SoulsOneDay);
        PlayerPrefs.DeleteKey(SaveDailyKey.Reincarnate3Souls);
        PlayerPrefs.DeleteKey(SaveDailyKey.Condemn2SoulsUnderworld);
    }
    private List<int> GenerateUniqueRandomNumbers(int count, int min, int max)
    {
        if (count > (max - min + 1))
        {
            Debug.LogError("Неможливо згенерувати таку кількість унікальних чисел у вказаному діапазоні!");
            return null;
        }

        List<int> numbersList = new List<int>();
        System.Random random = new System.Random();

        while (numbersList.Count < count)
        {
            int randomNumber = random.Next(min, max + 1);

            // Перевіряємо, чи число вже міститься в списку
            if (!numbersList.Contains(randomNumber))
            {
                numbersList.Add(randomNumber);
            }
        }

        // Перетворюємо список у масив і повертаємо
        return numbersList;
    }

    public void CheckDoneTasks()
    {
        _doneTasks = LoadIntList("DoneTasks");
        _receivedTasks = LoadIntList("ReceivedTasks");
    }

    public void SaveIntList(string key, List<int> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("Список порожній, нічого зберігати!");
            return;
        }

        // Серіалізуємо список у рядок, використовуючи роздільник
        string serializedList = string.Join(Separator.ToString(), list);
        PlayerPrefs.SetString(key, serializedList);
        PlayerPrefs.Save();
        Debug.Log($"Список збережено під ключем '{key}': {serializedList}");
    }

    // Метод для завантаження List<int> з PlayerPrefs
    public  List<int> LoadIntList(string key)
    {
        
        if (CheckExistOfRow(key) != null)
        {
            List<int> list_1 = CheckExistOfRow(key);
            return list_1;
        }
        // Отримуємо збережений рядок і десеріалізуємо його у список
        string serializedList = PlayerPrefs.GetString(key);
        string[] stringArray = serializedList.Split(Separator);
        Something(key);
        List<int> list = new List<int>();
        foreach (string item in stringArray)
        {
            if (int.TryParse(item, out int number))
            {
                list.Add(number);
            }
        }

        Debug.Log($"Список завантажено з ключа '{key}': {string.Join(", ", list)}");
        return list;
    }

    private void Something(string key)
    {
        string serializedList = PlayerPrefs.GetString(key);
        string[] stringArray = serializedList.Split(Separator);
        List<int> list = new List<int>();
        foreach (string item in stringArray)
        {
            if (int.TryParse(item, out int number))
            {
                list.Add(number);
            }
        }
    }
    private List<int> CheckExistOfRow(string key)
    {
        // Перевіряємо, чи існує збережений рядок
        if (!PlayerPrefs.HasKey(key))
        {
            Debug.LogWarning($"Ключ '{key}' не знайдено!");
            return new List<int>();
        }
        return null;
    }
    public void DoTask(DailyTaskTypes dailyTaskTypes)
    {
        for(int i = 0; i < _currentTasks.Count; i++)
        {
            if (_dailyTasks[_currentTasks[i]].DailyTask == dailyTaskTypes)
            {
                int doneTask = _currentTasks[i];
                for(int j = 0; j < _doneTasks.Count;j++)
                {
                    if(doneTask == _doneTasks[j])
                    {
                        return;
                    }
                }

                _doneTasks.Add(_currentTasks[i]);
            }
        }
        SaveIntList("DoneTasks", _doneTasks);
        _doneTaskText.text = (_doneTasks.Count - _receivedTasks.Count).ToString();
    }
    public void GetPoints(int taskIndex)
    {
        for (int j = 0; j < _receivedTasks.Count; j++)
        {
            if (taskIndex == _receivedTasks[j])
            {
                return;
            }
        }
        _receivedTasks.Add(_currentTasks[taskIndex]);
        SaveIntList("ReceivedTasks", _receivedTasks);
        ResourceManager.Instance.UpdateScore(50);
        _doneTaskText.text = (_doneTasks.Count - _receivedTasks.Count).ToString();
    }
}