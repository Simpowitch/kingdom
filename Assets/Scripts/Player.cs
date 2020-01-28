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
        new string[] {"My king.", "Your highness.", "Your excellency.", "Sire.", "My lord."},
        new string[] {"My queen.", "Your highness.", "Your excellency.", "My lady."},
        new string[] {"My ruler.", "Your highness.", "Your excellency."}
    };

    public string GetName()
    {
        return names[(int)gender][Random.Range(0, names[(int)gender].Length)];
    }


    int gold;
    int population;
    int happiness;
    int armyPower;
    int navalPower;


    [SerializeField] int startGold = 100;
    [SerializeField] int startPopulation = 100;
    [SerializeField] int startHappiness = 100;
    [SerializeField] int startArmy = 100;
    [SerializeField] int startNaval = 100;



    [SerializeField] TextMeshProUGUI goldText = null;
    [SerializeField] TextMeshProUGUI populationText = null;
    [SerializeField] TextMeshProUGUI happinessText = null;
    [SerializeField] TextMeshProUGUI armyText = null;
    [SerializeField] TextMeshProUGUI navalText = null;


    private void Start()
    {
        gold = startGold;
        population = startPopulation;
        happiness = startHappiness;
        armyPower = startArmy;
        navalPower = startNaval;

        goldText.text = gold.ToString();
        populationText.text = population.ToString();
        happinessText.text = happiness.ToString();
        armyText.text = armyPower.ToString();
        navalText.text = navalPower.ToString();
    }


    public int CheckGold()
    {
        return gold;
    }

    public void ChangeStat(StatChange statChange)
    {
        switch (statChange.stat)
        {
            case Stat.Money:
                gold += statChange.change;
                goldText.text = gold.ToString();
                break;
            case Stat.Population:
                population += statChange.change;
                populationText.text = population.ToString();
                break;
            case Stat.Happiness:
                happiness += statChange.change;
                happinessText.text = happiness.ToString();
                break;
            case Stat.Army:
                armyPower += statChange.change;
                armyText.text = armyPower.ToString();
                break;
            case Stat.Naval:
                navalPower += statChange.change;
                navalText.text = navalPower.ToString();
                break;
        }
    }

    public void ChangeStat(StatChange[] statChange)
    {
        for (int i = 0; i < statChange.Length; i++)
        {
            ChangeStat(statChange[i]);
        }
    }
}
