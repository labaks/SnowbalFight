using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPlayer : MonoBehaviour
{
    public GameObject player;
    // Shooting shooting;

    void Start()
    {
        // shooting = player.GetComponent<Shooting>();
        switchTo(0);
    }

    public void switchTo(int id)
    {
        foreach (Transform child in player.transform)
        {
            switcher(child, true, id);
        }
        switcher(player.transform.GetChild(id), false, id);
    }

    void switcher(Transform player, bool clean, int id)
    {
        MoveByTouch mover = player.GetComponent<MoveByTouch>();
        if (!clean)
        {
            mover.rb = mover.players[id].GetComponent<Rigidbody>();
        }
        var tempColor = gameObject.transform.GetChild(0).GetChild(id).GetComponent<Image>().color;
        tempColor.a = clean ? .4f : 1f;
        gameObject.transform.GetChild(0).GetChild(id).GetComponent<Image>().color = tempColor;
        player.transform.GetChild(0).gameObject.SetActive(!clean);
        // shooting.firePoint = player.transform.GetChild(1).gameObject.transform;
    }
}
