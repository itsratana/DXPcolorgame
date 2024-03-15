using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RefManager : MonoBehaviour
{
    public DataSaveManager dataSaveManager;
    public TextMeshProUGUI countdownTime;
    public CinemachineVirtualCamera virtualCamera;
    public Menu refMenu;

    public void AssignPlayertoMenu(PlayerII player)
    {
        refMenu.AssignPlayertoMenu(player);
    }
}
