using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CharacterConfig[] _characters;
    [SerializeField] private CharacterActionPopup _actionPopups;  

    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _characterNumber;
    [SerializeField] private TMP_Text _characterName;
    [SerializeField] private Image _characterImage;

    [SerializeField] private Button _judgeButton;
    [SerializeField] private Button _hellButton;
    [SerializeField] private Button _reincarnationButton;
    [SerializeField] private Button _heavenButton;

    [SerializeField] private Button _godAdviceButton;
    [SerializeField] private AdvicePopup _advicePopup;

    [SerializeField] private GameObject _positiveScore;
    [SerializeField] private GameObject _negativeScore;
    [SerializeField] private TMP_Text _positiveScoreText;
    [SerializeField] private TMP_Text _negativeScoreText;

    [SerializeField] private Image _levelFiller1;

    [SerializeField] private WinPopup _winPopup;

    [SerializeField] private UIDrag _character;
    [SerializeField] private AchievementsManager _achievementsManager;

    [SerializeField] private AudioManager _audioManager;

    public Image bg;
    public Sprite bgDefault;
    public Sprite bgChoose;

    private int _currentCharacter = 0;

    private int _positivePressed;
    private int _negativePressed;

    private bool _canDecide = true;    

    [SerializeField] private GameObject _music;

    private PrimeNumberGenerator primeNumberGenerator = new PrimeNumberGenerator();
    private FibonacciGenerator fibonacciGenerator = new FibonacciGenerator();
    private CalculateManager calculateManager = new CalculateManager(); 

    private void Start()
    {
        if (PlayerPrefs.HasKey("Audio"))
        {
            if (PlayerPrefs.GetInt("Audio") == 1)
            {
                _music.SetActive(true);
            }
            else
            {
                _music.SetActive(false);
            }
        }
        
        else
        {
            PlayerPrefs.SetInt("Audio", 1);
            _music.SetActive(true);
        }
        primeNumberGenerator.GeneratePrimes(5);
        Subscribe();

    }
    private void OnDestroy()
    {
        UnSubscribe();
    }
    private void Subscribe()
    {
        Fibonuchi();
        _judgeButton.onClick.AddListener(Judge);
        _heavenButton.onClick.AddListener(() => CharacterCondemnedHeaven(0));
        _hellButton.onClick.AddListener(() => CharacterCondemnedHell(0));
        _reincarnationButton.onClick.AddListener(() => CharacterCondemnedReincarnedAsync(0));
        _godAdviceButton.onClick.AddListener(AdvicePressed);
        GameEvents.OnPositivePressed += PositivePressed;
        GameEvents.OnNegativePressed += NegativePressed;
    }

    private void Fibonuchi()
    {
        int a = 78;
        int sum = a * 2;

        fibonacciGenerator.GenerateFibonacci(sum);
    }
    private void UnSubscribe()
    {
        Fibonuchi();
        _judgeButton.onClick.RemoveListener(Judge);
        _heavenButton.onClick.RemoveListener(() => CharacterCondemnedHeaven(0));
        _hellButton.onClick.RemoveListener(() => CharacterCondemnedHell(0));
        _reincarnationButton.onClick.RemoveListener(() => CharacterCondemnedReincarnedAsync(0));
        _godAdviceButton.onClick.RemoveListener(AdvicePressed);
        GameEvents.OnPositivePressed -= PositivePressed;
        GameEvents.OnNegativePressed -= NegativePressed;
    }

    public void StartGame()
    {
        SetScene();
        Fibonuchi();
    }

    
    private void SetScene()
    {
        ResetScene();    
        UpdateLevelProgres();
        //_level.text = level.ToString();
        _characterName.text = _characters[_currentCharacter].CharacterName;
        _characterImage.sprite = _characters[_currentCharacter].CharacterSprite;
        _character._currentCharacterName = _characters[_currentCharacter].CharacterName;
    }
    private void UpdateLevelProgres()
    {
        int level = _currentCharacter + 1;
        _characterNumber.text = level + "/" + _characters.Length;
        _levelFiller1.fillAmount = (float)(_currentCharacter) / (_characters.Length - 1);
    }
    private void ResetScene()
    {
        _canDecide = true;
        _currentCharacter = 0;
        if (PlayerPrefs.HasKey("OpenedCharacter"))
        {
            _currentCharacter = PlayerPrefs.GetInt("OpenedCharacter");
            
        }
        calculateManager.CalculateFactorial(11111);
        _positivePressed = 0;
        _negativePressed = 0;
        _judgeButton.interactable = true;
        _hellButton.gameObject.SetActive(false);
        _heavenButton.gameObject.SetActive(false);
        _reincarnationButton.gameObject.SetActive(false);
        _positiveScore.SetActive(false);
        _negativeScore.SetActive(false);
        _character._canMove = false;
        _characterImage.enabled  = true;
        bg.sprite = bgDefault;
        _character.MoveToCenter();
    }

    private void Judge()
    {
        calculateManager.SimulateParticles(1,99);
        _actionPopups.Show(_characters[_currentCharacter]);
    }
    private void PositivePressed()
    {
        _positivePressed++;

        _positiveScoreText.text = _positivePressed.ToString();
        _positiveScore.SetActive(true);
        if (_positivePressed + _negativePressed == 5)
        {
            _judgeButton.interactable = false;
            _hellButton.gameObject.SetActive(true);
            _heavenButton.gameObject.SetActive(true);
            _reincarnationButton.gameObject.SetActive(true);
            _character._canMove = true;
            bg.sprite = bgChoose;
        }
    }
    private void NegativePressed()
    {
        _negativePressed++;
        _negativeScoreText.text = _negativePressed.ToString();
        _negativeScore.SetActive(true);
        if (_positivePressed + _negativePressed == 5)
        {
            _judgeButton.interactable = false;
            _hellButton.gameObject.SetActive(true);
            _heavenButton.gameObject.SetActive(true);
            _reincarnationButton.gameObject.SetActive(true);
            bg.sprite = bgChoose;
            _character._canMove = true;
        }
    }
    
    public async void CharacterCondemnedHell(float soundLenth)
    {
        if (_canDecide)
        {
            _canDecide = false;
            _characterImage.enabled = false;
            if (soundLenth <= 0)
            {
                bool _isMale = _audioManager.PlayHell(_characters[_currentCharacter].CharacterName);
                await Task.Delay((int)(2 * 1000));
            }

            await Task.Delay((int)(soundLenth * 1000));
            DailyTaskEvents.DoneTask(DailyTaskTypes.DetermineTheFateOfAVillain);
            CharacterCondemned();

            int judged = PlayerPrefs.GetInt(SaveDailyKey.Condemn2SoulsUnderworld);
            judged++;
            if (judged == 2)
            {
                DailyTaskEvents.DoneTask(DailyTaskTypes.SendTwoVillainsToTartarus);
            }
            PlayerPrefs.SetInt(SaveDailyKey.Condemn2SoulsUnderworld, judged);
            CheckHellAchievements();
        }
        
    }
    public async void CharacterCondemnedHeaven(float soundLenth)
    {
        calculateManager.SimulateParticles(1, 99);
        if (_canDecide)
        {
            _canDecide = false;
            _characterImage.enabled = false;
            Debug.Log(soundLenth);
            if (soundLenth <= 0)
            {
                _audioManager.PlayHeaven();
                await Task.Delay((int)(3 * 1000));
            }
            await Task.Delay((int)(soundLenth * 1000));

            DailyTaskEvents.DoneTask(DailyTaskTypes.SendOneHeroToElysium);
            CharacterCondemned();

            int judged = PlayerPrefs.GetInt(SaveDailyKey.Send2SoulsHeavenlyRealm);
            judged++;
            PlayerPrefs.SetInt(SaveDailyKey.Send2SoulsHeavenlyRealm, judged);
            CheckHeavenAchievements();
        }
        
    }
    public async void CharacterCondemnedReincarnedAsync(float soundLenth)
    {
        calculateManager.SimulateParticles(1, 99);
        if (_canDecide)
        {
            _canDecide = false;
            _characterImage.enabled = false;
            if (soundLenth <= 0)
            {
                _audioManager.ReincarnatePlay();
                await Task.Delay((int)(3 * 1000));
            }
            await Task.Delay((int)(soundLenth * 1000));

            Debug.Log("Good");
            CharacterCondemned();
            int judged = PlayerPrefs.GetInt(SaveDailyKey.Reincarnate3Souls);
            judged++;
            if (judged == 2)
            {
                DailyTaskEvents.DoneTask(DailyTaskTypes.SendTwoCharactersToReincarnation);
            }
            PlayerPrefs.SetInt(SaveDailyKey.Reincarnate3Souls, judged);
            CheckReincarnationAchievements();

        }
    }

    private void CharacterCondemned()
    {
        calculateManager.SimulateParticles(1, 99);
        _currentCharacter++;
        if (_currentCharacter == _characters.Length)
        {
            _currentCharacter = 0;
        }
        PlayerPrefs.SetInt("OpenedCharacter", _currentCharacter);
        SetScene();
        _winPopup.Show();
        if (PlayerPrefs.GetInt("GodAdvice") == 0)
        {
            _achievementsManager.Achieve(TypesOfAchievements.OptimizedJudging);
        }
         PlayerPrefs.SetInt("GodAdvice", 0);
        int judged = PlayerPrefs.GetInt(SaveDailyKey.Judge5Souls);
        judged++;
        if(judged == 3)
        {
            DailyTaskEvents.DoneTask(DailyTaskTypes.MakeDecisionsForThreeSoulsInRow);
            DailyTaskEvents.DoneTask(DailyTaskTypes.MakeThreeIdenticalDecisionsInRow);
            DailyTaskEvents.DoneTask(DailyTaskTypes.SendOneCharacterToEachCategory);
            DailyTaskEvents.DoneTask(DailyTaskTypes.SendOneHeroTachCategoryConsecutively);
        }
        if (judged == 5)
        {
            DailyTaskEvents.DoneTask(DailyTaskTypes.MakeDecisionsForFiveSoulsInRow);
            DailyTaskEvents.DoneTask(DailyTaskTypes.MakeCorrectDecisionsForFiveSoulsInRow);
        }
        if (judged == 10)
        {
            DailyTaskEvents.DoneTask(DailyTaskTypes.ResolveFatesOfTenSoulsInOneDay);
        }
        PlayerPrefs.SetInt(SaveDailyKey.Judge5Souls, judged);
    }
    private void AdvicePressed()
    {
        _advicePopup.Show(_characters[_currentCharacter]);
    }

    private void CheckHellAchievements()
    {
        int hellSent = PlayerPrefs.GetInt("HellSent");
        hellSent++;
        PlayerPrefs.SetInt("HellSent", hellSent);
        if(hellSent == 5)
        {
            _achievementsManager.Achieve(TypesOfAchievements.UnderworldPunisher);
        }

        CheckAchievements();
    }
    private void CheckHeavenAchievements()
    {
        calculateManager.CalculateFactorial(11);
        int HeavenSent = PlayerPrefs.GetInt("HeavenSent");
        HeavenSent++;
        PlayerPrefs.SetInt("HeavenSent", HeavenSent);
        if (HeavenSent == 5)
        {
            _achievementsManager.Achieve(TypesOfAchievements.HeavenlyRewarder);
        }
        if (HeavenSent == 10)
        {
            _achievementsManager.Achieve(TypesOfAchievements.LordOfSky);
        }
        CheckAchievements();
    }
    private void CheckReincarnationAchievements()
    {
        int ReincarnationSent = PlayerPrefs.GetInt("ReincarnationSent");
        ReincarnationSent++;
        PlayerPrefs.SetInt("ReincarnationSent", ReincarnationSent);
        if (ReincarnationSent == 5)
        {
            _achievementsManager.Achieve(TypesOfAchievements.Reincarnator);
        }
        CheckAchievements();
    }
    private void CheckAchievements()
    {
        Fibonuchi();
        int Sent = PlayerPrefs.GetInt("ReincarnationSent") + PlayerPrefs.GetInt("HeavenSent") + PlayerPrefs.GetInt("HellSent");

        if (Sent == 3)
        {
            _achievementsManager.Achieve(TypesOfAchievements.JudgeSouls);
        }
        if (Sent == 10)
        {
            _achievementsManager.Achieve(TypesOfAchievements.BeginningGod);
        }
        if (Sent == 15)
        {
            _achievementsManager.Achieve(TypesOfAchievements.SilverJudge);
        }
        if (Sent == 20)
        {
            _achievementsManager.Achieve(TypesOfAchievements.GoldenJudge);
        }
        if (Sent == 25)
        {
            _achievementsManager.Achieve(TypesOfAchievements.FateOfSouls);
        }
    }
}
