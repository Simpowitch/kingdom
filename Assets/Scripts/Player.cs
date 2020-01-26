using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Gender { Male, Female, Other}
public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;
    private void Awake()
    {
        if (Player.instance == null)
        {
            Player.instance = this;
        }
        else
        {
            Debug.LogWarning("another instance of " + this + " was tried to be created, but is now destroyed from gameobject" + this.gameObject.name);
            Destroy(this);
        }
    }
    #endregion

    [SerializeField] Gender gender = Gender.Other;
    string[][] names = new string[3][]
    {
        new string[] {"My king.", "Your highness.", "Your excellency."},
        new string[] {"My queen.", "Your highness.", "Your excellency."},
        new string[] {"My ruler.", "Your highness.", "Your excellency."}
    };

    public string GetName()
    {
        return names[(int)gender][Random.Range(0, names[(int)gender].Length)];
    }


    int gold;
    int happiness;
    int population;


    [SerializeField] int startGold = 100;
    [SerializeField] int startHappiness = 100;
    [SerializeField] int startPopulation = 100;


    [SerializeField] TextMeshProUGUI goldText = null;
    [SerializeField] TextMeshProUGUI happinessText = null;
    [SerializeField] TextMeshProUGUI populationText = null;

    private void Start()
    {
        gold = startGold;
        happiness = startHappiness;
        population = startPopulation;

        goldText.text = gold.ToString();
        happinessText.text = happiness.ToString();
        populationText.text = population.ToString();
    }

    public void ChangeGold(int change)
    {
        gold += change;
        goldText.text = gold.ToString();
    }

    public int CheckGold()
    {
        return gold;
    }
}
