using System;
using UnityEngine;

[Serializable]
public class DailyTaskData
{
    [SerializeField] private DailyTaskTypes _dailyTask;
    [SerializeField] private string _taskName;
    [SerializeField] private string _taskDescription;

    public DailyTaskTypes DailyTask => _dailyTask;
    public string TaskName => _taskName;    
    public string Description => _taskDescription;
}
