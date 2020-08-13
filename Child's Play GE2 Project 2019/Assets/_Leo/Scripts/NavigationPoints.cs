using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationPoints : MonoBehaviour
{
    public static Transform[] navPointsArray;

    private void Awake()
    {
        navPointsArray = new Transform[this.transform.childCount];
        for (int i = 0; i < navPointsArray.Length; i++)
        {
            navPointsArray[i] = this.transform.GetChild(i);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
