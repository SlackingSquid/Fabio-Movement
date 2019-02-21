using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountObjectsInList : MonoBehaviour
{

    public List<GameObject> objects;
    int NumOfObj = 0;
    int objectsCollected = 0;
    public Material glowyMat;
    public GameObject thingToEnable;

    public List<Renderer> indicators;
    // Start is called before the first frame update
    void Start()
    {
        NumOfObj = objects.Count;
        thingToEnable.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < objects.Count; i++)
        {
            if (objects[i] == null)
            {
                objects.RemoveAt(i);
                NumOfObj = objects.Count;
                indicators[objectsCollected].material = glowyMat;
                objectsCollected++;
                if(NumOfObj == 0)
                {
                    if(thingToEnable != null)
                    {
                        thingToEnable.SetActive(true);
                    }
                }
            }
        }
    }
}
