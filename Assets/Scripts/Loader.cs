using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class Loader 
{
    private static int currentLevel = 1;

    public enum Scene
    {
        main = 1,
        SecondaryScene = 2
    }

    public static void load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static void nextLevel()
    {
        currentLevel += 1;
        if(currentLevel > Enum.GetNames(typeof(Scene)).Length)
        {
            return;
        }
        load((Scene) currentLevel);
    }
}
