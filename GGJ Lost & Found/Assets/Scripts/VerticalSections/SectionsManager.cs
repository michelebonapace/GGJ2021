using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionsManager : MonoBehaviour
{
    public static SectionsManager Instance;
    private Transform player;
    public int instancesPerSection = 2;
    public float sectionsHeight = 5;
    public GameObject[] sectionsPrefabs;
    public GameObject[] normalLuggage;
    public GameObject explosiveLuggage;
    public GameObject bouncingLuggage;
    public GameObject target;

    private List<Section> sectionsInstances;

    private int totalSections;
    private int currentHeightIndex;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        totalSections = instancesPerSection * sectionsPrefabs.Length;
        currentHeightIndex = totalSections / 2;
        sectionsInstances = new List<Section>();
        for (int i = 0; i < totalSections; i++)
            sectionsInstances.Add(Instantiate(sectionsPrefabs[i % sectionsPrefabs.Length], transform.position + sectionsHeight * i * Vector3.up, Quaternion.identity, this.transform).GetComponent<Section>());
        for (int i = 0; i < totalSections; i++)
            sectionsInstances[i].ResetDoors();
        GenerateLuggages();

    }

    private void GenerateLuggages()
    {
        for (int i = 0; i < totalSections; i++)
        {
            Transform[] ts = sectionsInstances[i].GetComponentsInChildren<Transform>();
            foreach (Transform t in ts)
            {
                if (t.name.Equals("Conveyor"))
                {
                    if (UnityEngine.Random.Range(0, 4) <= 1)
                    {
                        int rand = UnityEngine.Random.Range(0, 99);
                        if (rand < 70)
                        {
                            int r = UnityEngine.Random.Range(0, normalLuggage.Length - 1);
                            Instantiate(normalLuggage[r], t.transform.position + Vector3.up * 2, normalLuggage[r].transform.rotation);
                        }
                        else if (rand < 80)
                        {
                            Instantiate(explosiveLuggage, t.transform.position + Vector3.up * 2, explosiveLuggage.transform.rotation);
                        }
                        else if (rand < 90)
                        {
                            Instantiate(explosiveLuggage, t.transform.position + Vector3.up * 2, explosiveLuggage.transform.rotation);
                        }
                        else
                        {
                            Instantiate(target, t.transform.position + Vector3.up * 2, target.transform.rotation);
                        }
                    }
                }
            }
        }
    }

    public void OnPlayerChangeSection(float height)
    {
        Debug.Log("change_section");
        int index = Mathf.RoundToInt(height / sectionsHeight);
        int distance = index - currentHeightIndex;
        if (distance > 0)
        {
            Section s = sectionsInstances[0];
            s.transform.position = sectionsInstances[totalSections - 1].transform.position + sectionsHeight * Vector3.up;
            sectionsInstances.RemoveAt(0);
            sectionsInstances.Add(s);
            currentHeightIndex++;
            sectionsInstances[0].ResetDoors();
            sectionsInstances[totalSections - 1].ResetDoors();
            sectionsInstances[totalSections - 2].ResetDoors();
        }

        else if (distance < -1)
        {
            Section s = sectionsInstances[totalSections - 1];
            s.transform.position = sectionsInstances[0].transform.position - sectionsHeight * Vector3.up;
            sectionsInstances.RemoveAt(totalSections - 1);
            sectionsInstances.Insert(0, s);
            currentHeightIndex--;
            sectionsInstances[0].ResetDoors();
            sectionsInstances[1].ResetDoors();
            sectionsInstances[totalSections - 1].ResetDoors();
        }

    }
}
