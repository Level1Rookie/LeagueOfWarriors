using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Dota.Networking;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] Image playerIcon = null;
    [SerializeField] TextMeshProUGUI playerNameTextMesh = null;
    [SerializeField] TextMeshProUGUI teamTextMesh = null;
    [SerializeField] TextMeshProUGUI readyTextMesh = null;
    [SerializeField] ChampionIdMapping mapping = null;

    public void SetCard(string playerName, Team team, int championId, PlayerConnectionState connectionState)
    {
        playerIcon.sprite = mapping.GetIcon(championId);
        playerNameTextMesh.text = playerName;
        teamTextMesh.text = team.ToString();

        string showText = "not ready";
        if(connectionState == PlayerConnectionState.RoomNotReady)
        {
            showText = "not ready";
        }
        else if(connectionState == PlayerConnectionState.RoomReady)
        {
            showText = "ready";
        }
        readyTextMesh.text = showText;
    }
}