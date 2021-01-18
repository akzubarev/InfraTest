﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private List<List<GameObject>> teams = new List<List<GameObject>>();

    //лист листов не отображается в эдиторе
    [SerializeField] private List<GameObject> team1 = new List<GameObject>();
    [SerializeField] private List<GameObject> team2 = new List<GameObject>();

    public GameObject gameEndWindow, settingsWindow;
    private float timeStart, timeEnd;

    private string[] fighterNames = new string[] {"Red Triangle","Red Circle", "Red Square",
        "Blue Triangle", "Blue Circle", "Blue Square" };

    private Dictionary<string, Fighter> fighters = new Dictionary<string, Fighter>();

    public Fighter GetFighter(string name)
    {
        return fighters[name];
    }

    // Start is called before the first frame update
    private void Awake()
    {
        teams.Add(team1);
        teams.Add(team2);

        for (int i = 0; i < 6; i++)
            fighters.Add(fighterNames[i], teams[i / 3][i % 3].GetComponent<Fighter>());

        Dropdown namesdd = settingsWindow.transform.Find("Name").Find("Dropdown").GetComponent<Dropdown>();
        List<Dropdown.OptionData> names = new List<Dropdown.OptionData>();
        foreach (string fighterName in fighterNames)
            names.Add(new Dropdown.OptionData(fighterName));

        namesdd.options = names;

        settingsWindow.SetActive(true);
    }

    public GameObject FindClosestEnemy(Transform transform, int teamNum)
    {
        GameObject closestenemy = null;

        int enemyTeamNum = (teamNum + 1) % 2;
        float mindistance = 10000;

        foreach (GameObject enemy in teams[enemyTeamNum])
        {
            if (enemy)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < mindistance)
                {
                    mindistance = distance;
                    closestenemy = enemy;
                }
            }
        }

        if (!closestenemy)
            CheckGameEnd();

        return closestenemy;
    }

    public bool CheckGameEnd()
    {
        bool[] teamsDead = new bool[] { false, false };
        int dead = 0;

        for (int i = 0; i < teams.Count; i++)

        {
            dead = 0;
            foreach (GameObject fighter in teams[i])
                if (!fighter)
                    dead++;


            if (dead == teams[i].Count)
                teamsDead[i] = true;

        }

        bool isGameEnd = false;
        foreach (bool item in teamsDead)
            isGameEnd = isGameEnd || item;

        if (isGameEnd)
        {
            EndGame(teamsDead);
            return true;
        }
        else return false;

    }

    public void StartGame()
    {
        settingsWindow.SetActive(false);
        timeStart = Time.time;
        foreach (Fighter fighter in fighters.Values)
            fighter.StartGame();

        StartCoroutine("CheckGameEndCoroutine");
    }

    private IEnumerator CheckGameEndCoroutine()
    {
        bool isGameEnd = false;
        while (!isGameEnd)
        {
            yield return new WaitForSeconds(1);
            if (CheckGameEnd())
                isGameEnd = true;
        }
    }

    private void EndGame(bool[] teamresults)
    {
        Time.timeScale = 0;
        timeEnd = Time.time;
        gameEndWindow.SetActive(true);

        string result = "";
        if (teamresults[0] && teamresults[1])
            result = "NOONE";
        else if (teamresults[0])
            result = "BLUE";
        else
            result = "RED";
        gameEndWindow.transform.Find("Winner").GetComponent<Text>().text = $"The winner is {result}";

        gameEndWindow.transform.Find("Time").GetComponent<Text>().text =
                   $"The fight took {System.Math.Round(timeEnd - timeStart, 3)} seconds";
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
