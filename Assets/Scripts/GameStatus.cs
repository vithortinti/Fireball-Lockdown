using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatus
{
    public static bool IsPaused {  get; private set; }
    private static float _actualTimeScale = 1;

    public static void Pause(bool pause)
    {
        if (pause)
        {
            _actualTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioController.AudioSource.Pause();
            IsPaused = true;
            return;
        }
        AudioController.AudioSource.UnPause();
        Time.timeScale = _actualTimeScale;
        IsPaused = false;
    }
}
