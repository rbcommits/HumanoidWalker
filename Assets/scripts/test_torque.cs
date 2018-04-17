using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_torque : MonoBehaviour {
    public List<Transform> limbs;
    public Toggle limb1;
    public Toggle limb2;
    public Toggle limb3;
    public Toggle limb4;

    public List<InputField> values;
    List<Vector3> forces;
    // Use this for initialization
    void Start () {
        forces = new List<Vector3>();
        foreach(InputField input in values)
        {
            input.text = "0";
        }
    }
	
	// Update is called once per frame
	void Update () {
        forces = new List<Vector3>();
        for (int id = 0; id < 12; id+=3)
        {
            forces.Add(new Vector3(float.Parse(values[id].text), float.Parse(values[id + 1].text), float.Parse(values[id + 2].text)));
        }
		if(limb1.isOn)
        {
            limbs[0].transform.GetComponent<Rigidbody>().AddForce(forces[1]);
            Debug.Log("Applied force to limb1: " + forces[0].ToString());
        }

        if (limb2.isOn)
        {
            limbs[1].transform.GetComponent<Rigidbody>().AddTorque(forces[1]);
            Debug.Log("Applied force to limb2: " + forces[1].ToString());
        }

        if (limb3.isOn)
        {
            limbs[2].transform.GetComponent<Rigidbody>().AddTorque(forces[2]);
            Debug.Log("Applied force to limb3: " + forces[2].ToString());
        }

        if (limb4.isOn)
        {
            limbs[3].transform.GetComponent<Rigidbody>().AddTorque(forces[3]);
            Debug.Log("Applied force to limb4: " + forces[3].ToString());
        }

    }
}
