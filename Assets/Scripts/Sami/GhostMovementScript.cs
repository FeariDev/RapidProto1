using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostMovementScript : MonoBehaviour
{
    enum PointfollowBehaviour
    {
        Cycle = 0,
        DoOnce = 1,
        RandomItem = 2,
        Random = 3,
    }

    [SerializeField] private PointfollowBehaviour m_PointfollowBehaviour;
    [SerializeField] private float randomizeRadius;
    [SerializeField] private float randomizeInterval;
    private float randomizertimer;
    [SerializeField] private Vector3 moveToPosition;
    [SerializeField] private float keepPositionDistance;
    private Vector3 curSpeed;
    [SerializeField] private bool disableYAxisMovement;
    [SerializeField] [Tooltip("")] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private List<string> destoryOnCollisionTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (acceleration > 0)
        {
            MoveObjectA();
        }
        else
        {
            MoveObjectS();
        }
        TimerHandler();
    }
    void TimerHandler()
    {
        if (m_PointfollowBehaviour == PointfollowBehaviour.Random & randomizeInterval > 0)
        {
            if (randomizertimer > randomizeInterval)
            {
                randomizertimer = 0;
                moveToPosition = transform.position + new Vector3(Random.Range(-randomizeRadius, randomizeRadius), Random.Range(-randomizeRadius, randomizeRadius), Random.Range(-randomizeRadius, randomizeRadius));
            }
            randomizertimer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (string i in destoryOnCollisionTag)
        {
            if (collision.collider.gameObject.tag == i)
            {
                Destroy(gameObject); break;
            }
        }
    }
    void MoveObjectA()
    {
        if (moveToPosition.x > transform.position.x & Vector3.Distance(gameObject.transform.position,moveToPosition) > keepPositionDistance)
        {
            curSpeed.x = MathLibary.Approach(curSpeed.x, maxSpeed, acceleration);
        }
        else if (moveToPosition.x < transform.position.x & Vector3.Distance(gameObject.transform.position, moveToPosition) > keepPositionDistance)
        {
            curSpeed.x = MathLibary.Approach(curSpeed.x, -maxSpeed, acceleration);
        }
        else
        {
            curSpeed.x = MathLibary.Approach(curSpeed.x,0, deceleration);
        }
        if (!disableYAxisMovement)
        {
            if (moveToPosition.y > transform.position.y & Vector3.Distance(gameObject.transform.position, moveToPosition) > keepPositionDistance)
            {
                curSpeed.y = MathLibary.Approach(curSpeed.y, maxSpeed, acceleration);
            }
            else if (moveToPosition.y < transform.position.y & Vector3.Distance(gameObject.transform.position, moveToPosition) > keepPositionDistance)
            {
                curSpeed.y = MathLibary.Approach(curSpeed.y, -maxSpeed, acceleration);
            }
            else
            {
                curSpeed.y = MathLibary.Approach(curSpeed.y, 0, deceleration);
            }
        }

        transform.position += curSpeed;
    }
    void MoveObjectS()
    {
        if (moveToPosition.x > transform.position.x & Vector3.Distance(gameObject.transform.position, moveToPosition) > keepPositionDistance)
        {
            curSpeed.x = maxSpeed;
        }
        else if (moveToPosition.x < transform.position.x & Vector3.Distance(gameObject.transform.position, moveToPosition) > keepPositionDistance)
        {
            curSpeed.x = -maxSpeed;
        }
        else
        {
            curSpeed.x = 0;
        }
        if (!disableYAxisMovement)
        {
            if (moveToPosition.y > transform.position.y & Vector3.Distance(gameObject.transform.position, moveToPosition) > keepPositionDistance)
            {
                curSpeed.y = maxSpeed;
            }
            else if (moveToPosition.y < transform.position.y & Vector3.Distance(gameObject.transform.position, moveToPosition) > keepPositionDistance)
            {
                curSpeed.y = -maxSpeed;
            }
            else
            {
                curSpeed.y = 0;
            }
        }

        transform.position += curSpeed;
    }
}
