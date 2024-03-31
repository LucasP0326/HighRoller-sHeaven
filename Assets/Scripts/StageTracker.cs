using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTracker : MonoBehaviour
{
    public ProgressData progressData; //Reference to Progress Data
    public int stage = 0;
    public GameObject[] stages;

    // Start is called before the first frame update
    void Start()
    {
        stage = ProgressData.opponentNumber;
        for (int i = 0; i < stages.Length; i++)
        {
            if(i == stage)
                {
                    stages[i].SetActive(true);
                }
            if(i != stage)
                {
                    stages[i].SetActive(false);
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
