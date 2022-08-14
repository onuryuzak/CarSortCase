using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void OnLevelSuccessDelegate();
    public static event OnLevelSuccessDelegate LevelSuccess;

    public delegate void OnLevelFailedDelegate();
    public static event OnLevelFailedDelegate LevelFailed;

    public delegate void OnLevelLoadedDelegate();
    public static event OnLevelLoadedDelegate LevelLoaded;

    public static void OnLevelSuccess()
    {
        LevelSuccess?.Invoke();
    }
    public static void OnLevelFailed()
    {
        LevelFailed?.Invoke();
    }
    public static void OnLevelLoaded()
    {
        LevelLoaded?.Invoke();
    }
}
