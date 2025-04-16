using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DailyTaskEvents 
{
    public static event Action<DailyTaskTypes> OnTaskDone;

    public static void DoneTask(DailyTaskTypes dailyTaskTypes)
    {
        OnTaskDone?.Invoke(dailyTaskTypes);
    }
}
