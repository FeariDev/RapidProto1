using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Ghost : MonoBehaviour
{
    [SerializeField] private List<GameObject> interactableObjects;
    [SerializeField] private float spawnStartTime = 5;
    [SerializeField] private float spawnInterval = 10;
    [SerializeField] private float speed = 2;
    [SerializeField] private GameObject target;
    [SerializeField] private bool alive;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;
    [SerializeField] private SpriteRenderer sr;
    private bool direction; //left is false, right is true
    [SerializeField] private float timer;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnStartTime)
        {
            if (!alive)
            {
                alive = true;
                ChooseTarget();
            }
            Movement();
        }
    }

    private void Movement()
    {
        if (direction && transform.position.x <= 10)
        {
            transform.position = new Vector3(transform.position.x + speed / 40, transform.position.y, transform.position.z);
            sr.sprite = rightSprite;
            if (transform.position.x >= 10)
            {
                spawnStartTime = spawnInterval;
                timer = 0;
                alive = false;
            }
        }
        else if (!direction && transform.position.x >= -10)
        {
            transform.position = new Vector3(transform.position.x - speed / 40, transform.position.y, transform.position.z);
            sr.sprite = leftSprite;
            if (transform.position.x <= -10)
            {
                spawnStartTime = spawnInterval;
                timer = 0;
                alive = false;
            }
        }
    }

    private void ChooseTarget()
    {
        int targetNumber = Random.Range(0, interactableObjects.Count);
        target = interactableObjects[targetNumber];
        transform.position = new Vector3(transform.position.x, target.transform.position.y -1/100, transform.position.z);
        if (transform.position.x > target.transform.position.x)
        {
            direction = false;
        }
        else
        {
            direction = true;
        }
    }
}
