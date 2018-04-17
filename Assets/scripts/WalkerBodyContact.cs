using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerBodyContact : MonoBehaviour {

    public WalkerAgentConfigurable agent;
    
    void Start(){
        //agent = gameObject.transform.parent.gameObject.GetComponent<WalkerAgentConfigurable>();
    }

    void OnTriggerEnter(Collider other){
        
        
        if (other.gameObject.name == "Ground")
        {
            agent.fell = true; 
        }
    }
}
