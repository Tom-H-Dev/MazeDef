using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MovingWalls : MonoBehaviour
{
    private Collider _col;

    public bool _canBeMovingWall = true;
    private float _closeTime = 10;
    private float _waitTime = 5f;
    private bool _playerInArea;
    public bool _isMovingWall;
    private float chanceToBeMovingWall = 0.2f;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _col = GetComponent<Collider>();

        float rand = Random.Range(0.0f, 1.0f);
        if (rand <= chanceToBeMovingWall)
        {
            _isMovingWall = true;
            StartCoroutine(MoveWall());
        }
        //Chance to be moing wall
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
        if (_canBeMovingWall)
        {
            float rand = Random.Range(0.0f, 1.0f);
            if (rand <= chanceToBeMovingWall)
            {
                _isMovingWall = true;
                yield break;
            }
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
            StartCoroutine(MoveWall());
        }
    }
}
