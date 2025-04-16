using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvicePopup : BasePopup
{
    [SerializeField] private AchievementsManager _achievementsManager;
    [SerializeField] private GodData[] _gods;
    [SerializeField] private Image _godImages;
    [SerializeField] private TMP_Text _godNames;

    [SerializeField] private GameObject _advice;
    [SerializeField] private TMP_Text _adviceText;
    [SerializeField] private GameObject _price;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _thanks;
    [SerializeField] private Button _getAdvice;
    [SerializeField] private Button _nextGodButton;
    [SerializeField] private Button _prevGodButton;
    [SerializeField] private Button _closeButton;

    [SerializeField] private Image[] _advicePoints;
    [SerializeField] private Sprite _currentPoint;
    [SerializeField] private Sprite _defaultPoint;

    private int _currentGod;

    private CharacterConfig _characterConfig;
    private List<GodData> _characterGods = new List<GodData>();
    private GodAdvice[] advice = new GodAdvice[4];
    private bool[] _boughtAdvices = new bool[4];
    private const char Separator = ',';
    private void Start()
    {
        _closeButton.onClick.AddListener(Close);
        _thanks.onClick.AddListener(Close);
        _getAdvice.onClick.AddListener(GetAdvice);
        _nextGodButton.onClick.AddListener(NextGod);
        _prevGodButton.onClick.AddListener(PrevGod);
    }
    private void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(Close);
        _thanks.onClick.RemoveListener(Close);
        _getAdvice.onClick.RemoveListener(GetAdvice);
        _nextGodButton.onClick.RemoveListener(NextGod);
        _prevGodButton.onClick.RemoveListener(PrevGod);
    }

    public void Show(CharacterConfig characterConfig)
    {
        if (_characterConfig == null || _characterConfig.CharacterName != characterConfig.CharacterName)
        {
            _characterConfig = characterConfig;
            ResetPopup();
        }
        _currentGod = 0;
        advice = _characterConfig.Advice;
        SetPopup();
        Show();
    }
    private void ResetPopup()
    {
        _boughtAdvices = new bool[4];
        _characterGods = new List<GodData>();
        _nextGodButton.interactable = true;
        _prevGodButton.interactable = false;
    }

    private void SetPopup()
    {
        _currentGod = 0;
        SetCharacterGods();
        SetCurrentGod();
    }
    private void SetCharacterGods()
    {
        for (int i = 0; i < _gods.Length; i++)
        {
            for (int j = 0; j < advice.Length; j++)
            {
                if (_gods[i].God == advice[j].God)
                {
                    Debug.Log(_gods[i].GodName);
                    _characterGods.Add(_gods[i]);
                }
            }
        }
    }

    private void SetCurrentGod()
    {
        _godImages.sprite = _characterGods[_currentGod].GodSprite;
        _godNames.text = _characterGods[_currentGod].GodName;
        SetGodAdvice();
    }
    private void SetGodAdvice()
    {
        if (_boughtAdvices[_currentGod])
        {
            _thanks.gameObject.SetActive(true);
            _getAdvice.gameObject.SetActive(false);

            _advice.SetActive(true);
            _price.SetActive(false);

            _adviceText.text = advice[_currentGod].GodAdviceText;
        }
        else
        {
            _thanks.gameObject.SetActive(false);
            _getAdvice.gameObject.SetActive(true);

            _advice.SetActive(false);
            _price.SetActive(true);

            _priceText.text = "50";
        }
    }

    private void NextGod()
    {
        _currentGod++;
        SetCurrentGod();
        Debug.Log(_currentGod);
        if(_currentGod == 3)
        {
            _nextGodButton.interactable = false;
        }
        _prevGodButton.interactable = true;
        SetPoint();
    }
    private void PrevGod()
    {
        _currentGod--;
        SetCurrentGod();
        if (_currentGod == 0 )
        {
            _prevGodButton.interactable = false;
        }
        _nextGodButton.interactable = true;
        SetPoint();

    }

    private void SetPoint()
    {
        foreach(var point in _advicePoints)
        {
            point.sprite =_defaultPoint;
        }
        _advicePoints[_currentGod].sprite = _currentPoint;
    }
    private void GetAdvice()
    {
        if (ResourceManager.Instance.IsEnought(50))
        {
            ResourceManager.Instance.UpdateScore(-50);
            _boughtAdvices[_currentGod] = true;
            SetGodAdvice();
            DailyTaskEvents.DoneTask(DailyTaskTypes.MakeDecisionWithZeusAdvice);

            CheckAchievements();

            PlayerPrefs.SetInt("GodAdvice", 1);

            int advice = PlayerPrefs.GetInt(SaveDailyKey.Consult3DifferentGods);
            advice++;
            if (advice == 3)
            {
                DailyTaskEvents.DoneTask(DailyTaskTypes.UseZeusAdviceForThreeCharacters);
            }
            if (advice == 5)
            {
                DailyTaskEvents.DoneTask(DailyTaskTypes.UseZeusAdviceFiveTimes);
            }
            PlayerPrefs.SetInt(SaveDailyKey.Consult3DifferentGods, advice);
        }
    }
    private void Close()
    {
        Hide();
    }

    private void CheckAchievements()
    {
        if (_characterGods[_currentGod].God == GodTypes.Zeus)
        {
            _achievementsManager.Achieve(TypesOfAchievements.FirstAdvice);
        }
        
        for (int i = 0;i < _gods.Length; i++)
        {
            if (_gods[i].God == _characterGods[_currentGod].God)
            {
                CheckSavedAskedGod(i);
            }
        }
        if (LoadIntList("AskedGods").Count == 3)
        {
            _achievementsManager.Achieve(TypesOfAchievements.FriendsOfGods);
        }
        if (LoadIntList("AskedGods").Count == _gods.Length)
        {
            _achievementsManager.Achieve(TypesOfAchievements.WisdomOfGods);
        }
        Debug.Log(LoadIntList("AskedGods").Count);
    }
    private void CheckSavedAskedGod(int i)
    {
        List<int> askedGods = LoadIntList("AskedGods");
        for (int j = 0; j < askedGods.Count; j++)
        {
            if (askedGods[j] == i)
            {
                return;
            }
        }
        askedGods.Add(i);
        SaveIntList("AskedGods", askedGods);
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
    public List<int> LoadIntList(string key)
    {
        // Перевіряємо, чи існує збережений рядок
        if (!PlayerPrefs.HasKey(key))
        {
            Debug.LogWarning($"Ключ '{key}' не знайдено!");
            return new List<int>();
        }

        // Отримуємо збережений рядок і десеріалізуємо його у список
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

        Debug.Log($"Список завантажено з ключа '{key}': {string.Join(", ", list)}");
        return list;
    }
}
