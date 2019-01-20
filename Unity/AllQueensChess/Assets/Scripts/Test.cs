using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Database.Open();
        for (ulong i = 0; i < 2000; i++)
        {
            Database.Add(i, 0);
        }
        Database.Close();
    }

    long LongRandom(long min, long max, System.Random rand)
    {
        long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
        result = (result << 32);
        result = result | (long)rand.Next((Int32)min, (Int32)max);
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
