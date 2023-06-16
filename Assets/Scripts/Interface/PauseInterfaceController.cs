using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseInterfaceController : InterfaceBehavior
{
    public Text PauseText;
    public Button PlayAgainBtn;
    public Button BackToMenuBtn;
    public Image Background;

    void Start()
    {
        Pause(false);
    }

    public void Pause(bool pause)
    {
        GameStatus.Pause(pause);
        PauseText.gameObject.SetActive(pause);
        Background.gameObject.SetActive(pause);
        PlayAgainBtn.gameObject.SetActive(pause);
        BackToMenuBtn.gameObject.SetActive(pause);
    }
}
