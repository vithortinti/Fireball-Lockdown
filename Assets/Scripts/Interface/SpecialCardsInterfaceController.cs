using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Special;

public class SpecialCardsInterfaceController : MonoBehaviour
{
    public Text Title;
    public Button BoomCard;
    public Button UnlimitedShotsCard;
    public Button FastShotsCard;
    public string[] TitleMessages;
    public GameObject BoomSpecial;
    public GameObject PlayerFireball;
    public Slider SliderTimer;
    public float MaxTimeToChoice;

    private float _timeCounter;
    private GameObject _cardController;
    private bool _isSpecialInterfaceEnabled = false;
    private GameInterfaceController _gameInterfaceController;
    private bool _sliderIsCompleted = false;
    private string _boomMessage = "BOOM";
    private string _unlimitedShotsMessage = "TIROS ILIMITADOS";
    private string _fastShotsMessage = "TIRO RÁPIDO";
    private string _randomMessage = "Escolha aleatória";

    // Start is called before the first frame update
    void Start()
    {
        _gameInterfaceController = GameObject.FindWithTag(Tags.Interface).GetComponent<GameInterfaceController>();
        _cardController = transform.GetChild(0).gameObject;
        SliderTimer.maxValue = MaxTimeToChoice;
        EnableInterface(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SliderTimer.value >= SliderTimer.maxValue)
        {
            _sliderIsCompleted = true;
        }

        if (SliderTimer.value < SliderTimer.maxValue)
        {
            LoadSlider();
        }
        else if (_sliderIsCompleted)
        {
            _gameInterfaceController.ShowTemporaryMessage($"ESPECIAL PERDIDO", 3);
            EnableInterface(false);
            _sliderIsCompleted = false;
        }
    }

    public void BoomButtonEvent()
    {
        int maxTime = 0;
        _gameInterfaceController.ShowTemporaryMessage($"{_boomMessage}", 3);
        Instantiate(BoomSpecial, Vector2.zero, Quaternion.identity);
        EnableInterface(false);
        DestroySpecialObject(maxTime);
        _gameInterfaceController.Odd = 1;
        DisableButtons(BoomCard)
            .EnableButtons(UnlimitedShotsCard, FastShotsCard);
    }

    public void FastShotEvent()
    {
        int maxTime = 10;
        _gameInterfaceController.ShowTemporaryMessage($"{_fastShotsMessage}", 3).SetTimeCounter(maxTime);
        PlayerFireball.GetComponent<Behavior>().Speed = 9;
        EnableInterface(false);
        DestroySpecialObject(maxTime);
        int odd = 3;
        _gameInterfaceController.Odd = 3;
        _gameInterfaceController.OddText.text = "X" + odd;
        DisableButtons(FastShotsCard)
            .EnableButtons(UnlimitedShotsCard, BoomCard);
    }

    public void UnlimitedShotsEvent()
    {
        int maxTime = 7;
        _gameInterfaceController.ShowTemporaryMessage($"{_unlimitedShotsMessage}", 3).SetTimeCounter(maxTime);
        PlayerFireballGenerator.TimeToGenerate = 0;
        EnableInterface(false);
        DestroySpecialObject(maxTime);
        int odd = 2;
        _gameInterfaceController.Odd = 2;
        _gameInterfaceController.OddText.text = "X" + odd;
        DisableButtons(UnlimitedShotsCard)
            .EnableButtons(FastShotsCard, BoomCard);
    }

    SpecialCardsInterfaceController EnableButtons(params Button[] buttons)
    {
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = new Color(1, 1, 1);
            button.enabled = true;
        }
        return this;
    }

    SpecialCardsInterfaceController DisableButtons(params Button[] buttons)
    {
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.5f);
            button.enabled = false;
        }
        return this;
    }

    string RandomTitle()
    {
        int index = Random.Range(0, TitleMessages.Length);
        return TitleMessages[index];
    }

    void LoadSlider()
    {
        if (_isSpecialInterfaceEnabled)
        {
            _timeCounter += Time.deltaTime;
            SliderTimer.value = _timeCounter;
        }
    }

    void EnableInterface(bool enable)
    {
        _cardController.SetActive(enable);
        _isSpecialInterfaceEnabled = enable;

        SliderTimer.value = 0;
        Title.text = RandomTitle();
        AudioController.AudioSource.volume = enable ? 0.5f : 1;
        Time.timeScale = enable ? 0.4f : 1;
    }

    private void DestroySpecialObject(float lifetime)
    {
        Destroy(LastSpecialObject, lifetime);
    }

    public void Call()
    {
        _timeCounter = 0;
        EnableInterface(true);
    }
}
