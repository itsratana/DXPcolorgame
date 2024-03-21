using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Start Position")]
    [SerializeField] private Quaternion _startRotation;
    [SerializeField] private Vector3 _startPosition;

    [Header("Ref Move")]
    //[SerializeField] private Move move;

    [Header("Respawn Boolean")]
    [SerializeField] private bool isRespawning = false;

    private void Start()
    {
        //move = GetComponent<Move>();
        _startRotation = transform.rotation;
        _startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            _startRotation = other.transform.rotation;
            _startPosition = other.transform.position;
        }

        if (other.gameObject.layer == 12 && !isRespawning)
        {
            /*dieCount++;
            Debug.Log(dieCount);
            if (dieCount >= maxDie)
            {
                Invoke(nameof(DelayGameOver), 0.4f);
            }*/
            isRespawning = true;
            CancelInvoke(nameof(Respawn));
            Invoke(nameof(Respawn), 0.7f);

        }
    }

    public void Respawn()
    {
        isRespawning = false;
        transform.position = _startPosition + new Vector3(0, 4f, 0);
        transform.rotation = _startRotation;
    }
}
