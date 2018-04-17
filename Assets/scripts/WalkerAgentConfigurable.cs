using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAgentConfigurable : Agent
{

    public float strength;
    float x_position;
    [HideInInspector]
    public bool[] leg_touching;
    [HideInInspector]
    public bool fell;
    Vector3 past_velocity;
    public Transform head;
    Rigidbody bodyRB;
    public Transform[] limbs;
    Rigidbody[] limbRBs;
    Dictionary<GameObject, Vector3> transformsPosition;
    Dictionary<GameObject, Quaternion> transformsRotation;

    public Transform torso;

    public List<Vector3> torquesView;
    public override void InitializeAgent()
    {
        //body = waist.transform;
        //bodyRB = body.GetComponent<Rigidbody>();
        transformsPosition = new Dictionary<GameObject, Vector3>();
        transformsRotation = new Dictionary<GameObject, Quaternion>();
        Transform[] allChildren = GetComponentsInChildren<Transform>();



        foreach (Transform child in allChildren)
        {
            transformsPosition[child.gameObject] = child.position;
            transformsRotation[child.gameObject] = child.rotation;
        }
        leg_touching = new bool[2];
        limbRBs = new Rigidbody[limbs.Length];
        for (int i = 0; i < limbs.Length; i++)
        {
            limbRBs[i] = limbs[i].gameObject.GetComponent<Rigidbody>();
        }

        torquesView = new List<Vector3>(6);
        for (int i = 0; i < limbRBs.Length; i++)
        {
            torquesView.Add(new Vector3());
        }
    }

    public override void CollectObservations()
    {
        
        for (int i = 0; i < limbs.Length; i++)
        {
           // AddVectorObs(limbRBs[i].angularVelocity);
        }
        
        AddVectorObs(torso.rotation);
        //waist
        AddVectorObs(limbRBs[0].velocity.x); AddVectorObs(limbRBs[0].velocity.y); AddVectorObs(limbRBs[0].velocity.z);
        AddVectorObs(limbRBs[1].velocity.x); AddVectorObs(limbRBs[1].velocity.y); AddVectorObs(limbRBs[1].velocity.z);

        //forelegs movement in Y, Z plane (Blue and Green arrows)
        AddVectorObs(limbRBs[2].velocity.y); AddVectorObs(limbRBs[2].velocity.z);
        AddVectorObs(limbRBs[3].velocity.y); AddVectorObs(limbRBs[3].velocity.z);

        //feet movement in Z plane (Blue arrow)
        AddVectorObs(limbRBs[4].velocity.z);
        AddVectorObs(limbRBs[5].velocity.z);

        

        /*
        for (int i = 0; i <= 1; i++)
        {
            if(leg_touching[i])
            {
                AddVectorObs(1);
            }
            else
            {
                AddVectorObs(0);
            }

        }

        /*
        AddVectorObs(body.transform.rotation.eulerAngles);

        AddVectorObs(bodyRB.velocity);

        AddVectorObs((bodyRB.velocity - past_velocity) / Time.fixedDeltaTime);
        past_velocity = bodyRB.velocity;

        for (int i = 0; i < limbs.Length; i++)
        {
            AddVectorObs(limbs[i].localPosition);
            AddVectorObs(limbs[i].localRotation);
            AddVectorObs(limbRBs[i].velocity);
            AddVectorObs(limbRBs[i].angularVelocity);
        }

        for (int index = 0; index < 4; index++)
        {
            if (leg_touching[index])
            {
                AddVectorObs(1);
            }
            else
            {
                AddVectorObs(0);
            }
            leg_touching[index] = false;
        }
        //*/
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {


        torquesView[0] = new Vector3(vectorAction[0], vectorAction[1], vectorAction[2]);

        torquesView[1] = new Vector3(vectorAction[3], vectorAction[4], vectorAction[5]);

        torquesView[2] = new Vector3(0, vectorAction[6], vectorAction[7]);

        torquesView[3] = new Vector3(0, vectorAction[8], vectorAction[9]);

        torquesView[4] = new Vector3(0, 0, vectorAction[10]);

        torquesView[5] = new Vector3(0, 0, vectorAction[11]);

        //for (int k = 0; k < vectorAction.Length; k++)
        // {
        //    vectorAction[k] = Mathf.Clamp(vectorAction[k], -1f, 1f);

        // }


        //these movement's/torques are based off the unity system of co-ordinates where your z is typically considered a Y
        
        //waist or thighs movement in X, Y, Z plane (RGB arrows)
        limbRBs[0].AddTorque(new Vector3(vectorAction[0], vectorAction[1], vectorAction[2]) * strength, ForceMode.Force);
        limbRBs[1].AddTorque(new Vector3(vectorAction[3], vectorAction[4], vectorAction[5]) * strength, ForceMode.Force);

        //forelegs movement in Y, Z plane (Blue and Green arrows)
        limbRBs[2].AddTorque( new Vector3(0, vectorAction[6], vectorAction[7]) * strength, ForceMode.Force);
        limbRBs[3].AddTorque( new Vector3(0, vectorAction[8], vectorAction[9]) * strength, ForceMode.Force);

        //feet movement in Z plane (Blue arrow)
        limbRBs[4].AddTorque(new Vector3(0, 0, vectorAction[10]) * strength, ForceMode.Force);
        limbRBs[5].AddTorque(new Vector3(0, 0, vectorAction[11]) * strength, ForceMode.Force);
        




        /*
        limbRBs[0].AddTorque(-limbs[0].transform.right * strength * vectorAction[0]);
        limbRBs[1].AddTorque(-limbs[1].transform.right * strength * vectorAction[1]);
        limbRBs[2].AddTorque(-limbs[2].transform.right * strength * vectorAction[2]);
        limbRBs[3].AddTorque(-limbs[3].transform.right * strength * vectorAction[3]);
        limbRBs[0].AddTorque(-body.transform.up * strength * vectorAction[4]);
        limbRBs[1].AddTorque(-body.transform.up * strength * vectorAction[5]);
        limbRBs[2].AddTorque(-body.transform.up * strength * vectorAction[6]);
        limbRBs[3].AddTorque(-body.transform.up * strength * vectorAction[7]);
        limbRBs[4].AddTorque(-limbs[4].transform.right * strength * vectorAction[8]);
        limbRBs[5].AddTorque(-limbs[5].transform.right * strength * vectorAction[9]);
        limbRBs[6].AddTorque(-limbs[6].transform.right * strength * vectorAction[10]);
        limbRBs[7].AddTorque(-limbs[7].transform.right * strength * vectorAction[11]);
        */

        float torque_penalty = vectorAction[0] * vectorAction[0] + 
            vectorAction[1] * vectorAction[1] + 
            vectorAction[2] * vectorAction[2] + 
            vectorAction[3] * vectorAction[3] +
            vectorAction[4] * vectorAction[4] + 
            vectorAction[5] * vectorAction[5] + 
            vectorAction[6] * vectorAction[6] + 
            vectorAction[7] * vectorAction[7] + 
            vectorAction[8] * vectorAction[8] + 
            vectorAction[9] * vectorAction[9] + 
            vectorAction[10] * vectorAction[10] + 
            vectorAction[11] * vectorAction[11];
        //Debug.Log()
        if (!IsDone())
        {//- 0.01f * torque_penalty
            float reward = 0 + 0.1f * head.position.y - Mathf.Abs(0 - torso.rotation.x) - Mathf.Abs(0 - torso.rotation.y) - Mathf.Abs(0 - torso.rotation.z) - Mathf.Abs(1 - torso.rotation.w);
            //Debug.Log(reward);
            //for now focus on balancing. add velocity to reward once we want the agent to walk forward
            SetReward( reward
            // - 0.05f * Mathf.Abs(body.transform.position.z - body.transform.parent.transform.position.z)
            //           - 0.05f * Mathf.Abs(bodyRB.velocity.y)
            );
        }
        if (fell)
        {
            Done();
            AddReward(-1f);
        }
    }

    public override void AgentReset()
    {
        fell = false;
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if ((child.gameObject.name.Contains("Crawler"))
                || (child.gameObject.name.Contains("parent")))
            {
                continue;
            }
            child.position = transformsPosition[child.gameObject];
            child.rotation = transformsRotation[child.gameObject];
            Rigidbody child_rb = child.gameObject.GetComponent<Rigidbody>();
            if(child_rb != null)
            {
                child_rb.velocity = default(Vector3);
                child_rb.angularVelocity = default(Vector3);
            }
            
        }
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,00 , 0));//Random.value * 10 - 5
        limbs[0].transform.rotation = Quaternion.Euler(Random.Range(135, 200), -0.4279785f, Random.Range(5, 10));
        limbs[1].transform.rotation = Quaternion.Euler(Random.Range(135, 200), -0.4279785f, Random.Range(5, 10));
    }

    public override void AgentOnDone()
    {

    }
}
