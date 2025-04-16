using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DailyTasksPopup: BasePopup
{
    [SerializeField] private DailyTasksManager _dailyTasksManager;
    [SerializeField] private Button _getPointsButton;
    [SerializeField] private TMP_Text _taskName;
    [SerializeField] private TMP_Text _taskDescription;
    [SerializeField] private TMP_Text _taskNumber;

    [SerializeField] private Button _close;
    [SerializeField] private Button _nextTask;
    [SerializeField] private Button _prevTask;

    [SerializeField] private Image[] _taskPoints;
    [SerializeField] private Sprite _currentPoint;
    [SerializeField] private Sprite _defaultPoint;

    public Image _bg;
    public Sprite _bgSpriteDefault;
    public Sprite _bgSpriteDisable;

    private int _currentTask;

    private void Start()
    {
        _close.onClick.AddListener(Close);
        _nextTask.onClick.AddListener(NextTask);
        _prevTask.onClick.AddListener(PrevTask);
        _getPointsButton.onClick.AddListener(GetPoints);
    }
    private void OnDestroy()
    {
        _close.onClick.RemoveListener(Close);
        _nextTask.onClick.RemoveListener(NextTask);
        _prevTask.onClick.RemoveListener(PrevTask);
        _getPointsButton.onClick.RemoveListener(GetPoints);
    }

    public override void Show()
    {
        _dailyTasksManager.CheckDoneTasks();
        SetPopup();
        base.Show();
    }

    public void SetPopup()
    {
        ResetPopup();
        SetTask();

    }
    private void ResetPopup()
    {
        _currentTask = 0;
        _getPointsButton.gameObject.SetActive(true);
        _getPointsButton.interactable = false;
        _bg.sprite = _bgSpriteDisable;
        _taskNumber.text = "Daily Task 1";
        _prevTask.interactable=false;
        _nextTask.interactable = true;
    }

    private void  SetTask()
    {
        _taskNumber.text = "Daily Task " + (_currentTask + 1);
        DailyTaskData dailyTask = _dailyTasksManager._dailyTasks[_dailyTasksManager._currentTasks[_currentTask]];

        _taskName.text = dailyTask.TaskName; ;
        _taskDescription.text = dailyTask.Description;

        SetButton();

    }
    private void SetButton()
    {
        List<int> doneTasks = _dailyTasksManager._doneTasks;
        List<int> receivedTasks = _dailyTasksManager._receivedTasks;
        _getPointsButton.interactable = false;
        _bg.sprite = _bgSpriteDisable;
        _getPointsButton.gameObject.SetActive(true);

        if (doneTasks.Count != 0)
        {
            for (int i = 0; i < doneTasks.Count; i++)
            {
                if (_dailyTasksManager._currentTasks[_currentTask] == doneTasks[i])
                {
                    _getPointsButton.interactable = true;
                    _bg.sprite = _bgSpriteDefault;
                }
            }
        }
        if (receivedTasks.Count != 0)
        {
            for (int i = 0; i < receivedTasks.Count; i++)
            {
                if (_dailyTasksManager._currentTasks[_currentTask] == receivedTasks[i])
                {
                    _getPointsButton.gameObject.SetActive(false);
                }
            }
        }
    }

    private void Close()
    {
        Hide();
    }
    private void NextTask()
    {
        _currentTask++;
        SetTask();
        if (_currentTask == 4)
        {
            _nextTask.interactable = false;
        }
        _prevTask.interactable = true;

        SetPoint();
    }

    private void PrevTask()
    {
        _currentTask--;
        SetTask();
        if (_currentTask == 0)
        {
            _prevTask.interactable = false;
        }
        _nextTask.interactable = true;

        SetPoint();
    }

    private void GetPoints()
    {
        _dailyTasksManager.GetPoints(_currentTask);
        _getPointsButton.gameObject.SetActive(false );
    }

    private void SetPoint()
    {
        foreach (var point in _taskPoints)
        {
            point.sprite = _defaultPoint;
        }
        _taskPoints[_currentTask].sprite = _currentPoint;
    }
}
