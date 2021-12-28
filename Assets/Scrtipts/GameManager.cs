using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    public GameObject masterSpawnPoint,
        clientSpawnPoint;
    public GameObject light;
    public GameObject shootBtnGO;
    public GameObject[] switcherBtns;

    GameObject player;
    Button shootBtn;

    void Start()
    {
        GameObject spawnPoint = PhotonNetwork.IsMasterClient ? masterSpawnPoint : clientSpawnPoint;

        player = PhotonNetwork.Instantiate(
            PlayerPrefab.name,
            spawnPoint.transform.position,
            Quaternion.identity
        );
        player.name = "MasterPlayer";
        if (!PhotonNetwork.IsMasterClient)
        {
            player.name = "ClientPlayer";
            Camera.main.transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, 180);
            light.transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, 180);
        }
        if (player.GetComponent<PhotonView>().IsMine)
        {
            player.GetComponent<PlayerControls>().GameManager = this;
            InitButton();
        }
    }

    void InitButton()
    {
        PlayerControls playerControls = player.GetComponent<PlayerControls>();
        shootBtnGO.GetComponent<Button>().onClick.AddListener(playerControls.Shoot);
        switcherBtns[0]
            .GetComponent<Button>()
            .onClick.AddListener(
                delegate
                {
                    playerControls.switchPlayer(0);
                }
            );
        switcherBtns[1]
            .GetComponent<Button>()
            .onClick.AddListener(
                delegate
                {
                    playerControls.switchPlayer(1);
                }
            );
        switcherBtns[2]
            .GetComponent<Button>()
            .onClick.AddListener(
                delegate
                {
                    playerControls.switchPlayer(2);
                }
            );
    }

    // Update is called once per frame
    void Update() { }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        // when current player leaving the room
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player {0} entered room", newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player {0} left room", otherPlayer.NickName);
    }
}
