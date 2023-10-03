using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Track : MonoBehaviour
{
    public string trackName;

    [SerializeField] private ItemBox itemBoxPrefab;
    [SerializeField] private List<Transform> itemSpawns = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var itemSpawn in itemSpawns)
        {
            ItemBox box = Instantiate(itemBoxPrefab, itemSpawn);
            box.ItemBoxCollected += (sender, ea) => OnItemBoxCollected(itemSpawn, sender, ea);
        }
    }

    void OnItemBoxCollected(Transform spawnLocation, object sender, ItemBox.ItemBoxCollectedEventArgs ea)
    {
        IEnumerator RespawnTimer()
        {
            yield return new WaitForSeconds(10);
            var box = Instantiate(itemBoxPrefab, spawnLocation);
            box.ItemBoxCollected += (sender, ea) => OnItemBoxCollected(spawnLocation, sender, ea);
        }

        StartCoroutine(RespawnTimer());
    }


    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var spawn in itemSpawns)
        {
            Gizmos.DrawWireSphere(spawn.position, 0.5f);
        }
    }
}