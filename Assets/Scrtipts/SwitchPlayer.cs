using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPlayer : MonoBehaviour
{
    public GameObject[] players;
    public GameObject currentPlayer;

    public GameObject ShootBtn;
    Shooting shooting;
    void Start()
    {
        shooting = ShootBtn.GetComponent<Shooting>();
        switchTo(0);
    }

    // Update is called once per frame
    void Update() { }

    public void switchTo(int id)
    {
        foreach (GameObject p in players)
        {
            switcher(p, true);
        }
        switcher(players[id], false);
    }

    void switcher(GameObject player, bool clean)
    {
        MoveByTouch mover = player.GetComponent<MoveByTouch>();
        mover.currentPlayer = !clean;
        var tempColor = mover.button.GetComponent<Image>().color;
        tempColor.a = clean ? .4f : 1f;
        mover.button.GetComponent<Image>().color = tempColor;
        player.transform.GetChild(0).gameObject.SetActive(!clean);
        shooting.firePoint = player.transform.GetChild(1).gameObject.transform;
    }
}
