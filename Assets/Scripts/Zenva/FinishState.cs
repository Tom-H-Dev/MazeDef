using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishState : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out FpsMovement l_player))
        {
            SceneManager.LoadScene("End Screen");
        }
    }
}
