using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("AgentManager script");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)){
            spawnAgent();
            Debug.Log("New Agent");
        }
    }
    
    public void spawnAgent() {
        Instantiate(prefab, new Vector3(0, 10, 0), Quaternion.identity);
    }
}

