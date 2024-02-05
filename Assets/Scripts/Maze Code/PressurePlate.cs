using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerIDs
{
    playerBlue = 1,
    playerRed = 2
    
}

public class PressurePlate : MonoBehaviour
{
    [Tooltip("To what player does this pressure plate belong.")]
    [SerializeField] PlayerIDs _whatPlayerPressurePlate;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInfo l_player))
        {
            if (l_player.id == ((int)_whatPlayerPressurePlate))
            {
                Debug.Log("Correct plaeyr on the pressure plate for " + _whatPlayerPressurePlate);
            }
        }
    }

    public void PressurePlateFunctionality(GameObject l_player)
    {
        PlayerInfo l_playerInfo = l_player.GetComponent<PlayerInfo>();
        if (l_playerInfo.id == ((int)_whatPlayerPressurePlate))
        {
            Debug.Log("Correct plaeyr on the pressure plate for " + _whatPlayerPressurePlate);
        }
    }
}
