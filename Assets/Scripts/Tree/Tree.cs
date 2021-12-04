using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] int health = 5;
    public GameObject WoodPlanks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health<=0)
        {
            Instantiate(WoodPlanks,transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    public void TakeDamage(int dmg)
    {
        health-=dmg;
    }
}
