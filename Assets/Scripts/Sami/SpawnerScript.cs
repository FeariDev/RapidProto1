using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Quaternion rotation;
    [SerializeField] [Tooltip("Asettaa Objektien Luomis Tahdin")] private float interval;
    [SerializeField] [Tooltip("Asettaa Onko Luoja P‰‰ll‰ Pelin Alkaessa")] private bool isEnabled = false;
    private float timer;
    private List<GameObject> spawnedObjects;
    [SerializeField] [Tooltip("Metodeja Joita Kutsutaan Kun Luoja Luo Objektit")] private UnityEvent onObjectsSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            SpawnHandler();
            timer += Time.deltaTime;
        }
    }
    void SpawnHandler()
    {
        if (timer > interval)
        {
            timer = 0;
            foreach (GameObject objects in spawnObjects)
            {
                spawnedObjects.Add(Instantiate(objects, gameObject.transform.position + offset, gameObject.transform.rotation * rotation));
            }
                onObjectsSpawn.Invoke(); //Aina Kun Objekti Luodaan
        }
    }
    public GameObject GetLastObject() //Tuo Viimeiseksi Luodun Objektin
    {
        return spawnedObjects[spawnedObjects.Count];
    }
    public List<GameObject> GetSpawnedObjects() //Tuo Listan Luoduista Objekteista
    {
        return spawnedObjects;
    }
    public void StartTimer() //K‰ynist‰‰ Ajastimen 0 Arvosta
    {
        timer = 0;
        isEnabled = true;
    }
    public void EnableTimer() //Jatkaa Laskemista 
    {
        isEnabled = true;
    }
    public void PauseTimer() //Pys‰ytt‰‰ Mutta Ei Nollaa
    {
        isEnabled = false;
    }
    public void ResetTimer() //Nollaa
    {
        timer = 0;
    }
    public void KillTimer() //Pys‰ytt‰‰ Ja nollaa
    {
        timer = 0;
        isEnabled = false;
    }
    public void AdjustTimer(float newInterval) //Asettaa Uuden Aikav‰lin
    {
        interval = newInterval;
    }
    public void ChangeSpawnOffset(Vector3 newOffset) //Asettaa Uuden 
    {
        offset = newOffset;
    }
}
