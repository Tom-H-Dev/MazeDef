using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MovingWalls : MonoBehaviour
{
    [Tooltip("The collider in the wall.")]
    private Collider _col;

    [Header("Timers & Animations")]
    [Tooltip("The time the doors are closed and after wich the doors open again.")]
    private float _closeTime = 10;

    [Tooltip("The time the doors are open.")]
    private float _waitTime = 5f;

    [Tooltip("The bool to check if there are still player in the collider.")]
    private bool _playerInArea;

    [Tooltip("The bool to check if the wall is a moving wall.")]
    public bool _isMovingWall;

    [Tooltip("The percetage change for a wall to be a moving wall.")]
    private float chanceToBeMovingWall = 0.2f;

    [Tooltip("The animator of the walls.")]
    [SerializeField] private Animator _animator;


    private void Start()
    {
        _col = GetComponent<Collider>();

        //Random change to be moving wall
        float rand = Random.Range(0.0f, 1.0f);
        if (rand <= chanceToBeMovingWall)
        {
            _isMovingWall = true;
            StartCoroutine(MoveWall());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_playerInArea && other.gameObject.TryGetComponent(out FpsMovement l_player))
        {
            _playerInArea = true;
            //Do Not close the wall
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (_playerInArea && other.gameObject.TryGetComponent(out FpsMovement l_player))
        {
            _playerInArea = false;
            //Can close wall again
        }
    }
    private IEnumerator MoveWall()
    {
        //Random change to be moving wall
        float rand = Random.Range(0.0f, 1.0f);
        if (rand <= chanceToBeMovingWall)
        {
            _isMovingWall = true;
            yield break;
        }
        //If the player is not the collider of the wall
        if (!_playerInArea)
        {
            _animator.SetTrigger("OpenWall");
            _col.isTrigger = true;
            yield return new WaitForSeconds(_waitTime);
            if (!_playerInArea)
            {
                _animator.SetTrigger("CloseWall");
                _col.isTrigger = false;
                yield return new WaitForSeconds(_closeTime);
            }
        }
        //Resets the routine
        StartCoroutine(MoveWall());
    }
}
