using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void One()
    {
        SceneManager.LoadScene(1);
    }

    public void Five()
    {
        SceneManager.LoadScene(2);
    }

    public void Ten()
    {
        SceneManager.LoadScene(3);
    }

    public void Twenty()
    {
        SceneManager.LoadScene(4);
    }

    public void Fifty()
    {
        SceneManager.LoadScene(5);
    }

    public void Hundred()
    {
        SceneManager.LoadScene(6);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }



}
