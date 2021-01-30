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

    private List<GameObject> sectionsInstances;

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
        sectionsInstances = new List<GameObject>();
        for (int i = 0; i < totalSections; i++)
            sectionsInstances.Add(Instantiate(sectionsPrefabs[i % sectionsPrefabs.Length], transform.position + sectionsHeight * i * Vector3.up, Quaternion.identity, this.transform));
    }

    public void OnPlayerChangeSection(float height)
    {
        Debug.Log("change_section");
        int index = Mathf.RoundToInt(height / sectionsHeight);
        int distance = index - currentHeightIndex;
        if (distance > 0)
        {
            GameObject s = sectionsInstances[0];
            s.transform.position = sectionsInstances[totalSections - 1].transform.position + sectionsHeight * Vector3.up;
            sectionsInstances.RemoveAt(0);
            sectionsInstances.Add(s);
            currentHeightIndex++;
        }

        else if (distance < -1)
        {
            GameObject s = sectionsInstances[totalSections - 1];
            s.transform.position = sectionsInstances[0].transform.position - sectionsHeight * Vector3.up;
            sectionsInstances.RemoveAt(totalSections - 1);
            sectionsInstances.Insert(0, s);
            currentHeightIndex--;
        }

    }
}
