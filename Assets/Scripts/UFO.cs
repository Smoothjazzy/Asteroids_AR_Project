using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class UFO : MonoBehaviour
{

    public enum UFOStates
    {
        Idle,
        Attacking
    }

    [SerializeField] private UFOStates _currentState;
    [SerializeField] private List<Vector3> _trajectoryVectors = new List<Vector3>();
    [SerializeField] private int _trajectoriesPerSpawn = 2;
    [SerializeField] private float _spawnDistanceFromPlayer = 20;
    [SerializeField] private float _xyOffset = 10;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private int _cooldownMinTime = 5;
    [SerializeField] private int _cooldownMaxTime = 15;
    [SerializeField] private GameState _gameState;

    private Transform _player;

    public UFOStates CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            if (_currentState == UFOStates.Attacking)
            {
                OnStartAttacking?.Invoke();
            }
            else
            {
                OnStopAttacking?.Invoke();
            }
        }
    }

    [SerializeField] private UnityEvent OnStartAttacking;
    [SerializeField] private UnityEvent OnStopAttacking;
    [SerializeField] private UnityEvent OnDie;

    [SerializeField] private AudioSfx _ufoOnScene;

    // Start is called before the first frame update
    void Start()
    {
        // find the player in the scene.
        GameObject playerObject = GameObject.Find("Player");

        // If the Player is found,then grab its transform object.
        if (playerObject)
        {
            _player = playerObject.transform;
        }

        CurrentState = UFOStates.Idle;
    }

    private IEnumerator IdleRoutine()
    {
        yield return null;
        transform.position = new Vector3(1000, 1000, 1000);
        _trajectoryVectors.Clear();
        yield return new WaitForSeconds(Random.Range(_cooldownMinTime, _cooldownMaxTime));
        CurrentState = UFOStates.Attacking;
    }

    public void StartCooldown()
    {
        StartCoroutine(IdleRoutine());
        _ufoOnScene.StopAudio();
    }

    private Vector3 GetNewPositionVector()
    {
        return new Vector3(Random.Range(-_xyOffset, _xyOffset), Random.Range(-_xyOffset, _xyOffset), _player.position.z + _spawnDistanceFromPlayer);
    }

    public void StartAttacking()
    {
        if (_player == null) return;
        Vector3 spawnPosition = GetNewPositionVector();
        transform.position = spawnPosition;

        for (int i = 0; i < _trajectoriesPerSpawn; i++)
        {
            _trajectoryVectors.Add(GetNewPositionVector());
        }

        _ufoOnScene.PlayAudio(gameObject);
        StartCoroutine(AttackMovement());
    }

    IEnumerator AttackMovement()
    {
        for (int i = 0; i < _trajectoryVectors.Count; i++) {
            float distance = Vector3.Distance(transform.position, _trajectoryVectors[i]);
            while (distance > 0.5f && !_gameState.GameOver) {
                yield return null;
                transform.position = Vector3.MoveTowards(transform.position, _trajectoryVectors[i], Time.deltaTime * _movementSpeed);
                distance = Vector3.Distance(transform.position, _trajectoryVectors[i]);
            }
        }
        CurrentState = UFOStates.Idle;
    }

    public void Die()
    {


        StopAllCoroutines();
        StartCooldown();
        _ufoOnScene.StopAudio();

        OnDie?.Invoke();
        OnStopAttacking?.Invoke();
    }


}
