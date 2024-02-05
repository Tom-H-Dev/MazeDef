using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishState : MonoBehaviour
{ 
    /// <summary>
    /// Loads the player to the 'End Screen' scene when the player reaches the end.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out FpsMovement l_player))
        {
            SceneManager.LoadScene("End Screen");
        }
    }
}
