using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class AICars : MonoBehaviour
{
    public int thisObjectIndex;
    private int chooseIndexRand;
    private NavMeshAgent agent;
    private Vector3 impulseForce;
    private int randIndex;
    private bool waitMode = false;
    private void Start()
    {
        InvokeRepeating("RandomIndexDeterminant", 0, BumperCarLevel.Instance.aIRandomTargetSecond);
        GameObject aIList = GameObject.Find("AI");
        for (int i = 0; i < aIList.transform.childCount; i++)
        {
            if (aIList.transform.GetChild(i).GetChild(0).transform==this.gameObject.transform)
            {
                thisObjectIndex = i;
            }        
        }
        agent = GetComponent<NavMeshAgent>();
    }
    
    void RandomIndexDeterminant()
    { // TODO: AI Target Belirleyici
        randIndex = Random.Range(0, BumperCarLevel.Instance.aICarsList.Count);
        if (randIndex <= 0)
        {
            chooseIndexRand = BumperCarLevel.Instance.aICarsList.Count-1;
        }
        if (randIndex > 0)
        {
            chooseIndexRand = Random.Range(0, BumperCarLevel.Instance.aICarsList.Count);
        }
    }
    private void FixedUpdate()
    {
        
        if (waitMode==false)
        {
            if (BumperCarLevel.Instance.aICarsList[chooseIndexRand] ==null)
            {
                RandomIndexDeterminant();
            }
            else
            {
// unity agent kullandım.

                /*   Vector3 lTargetDir = bumperCarLevel.aICarsList[rand].transform.position - transform.position;
                   lTargetDir.y = 0.0f;
                   transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * 1);
                   if (transform.rotation == Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * 1))
                   {

                       Vector3 pos = Vector3.MoveTowards(transform.position, bumperCarLevel.aICarsList[rand].transform.position, speed * Time.fixedDeltaTime);
                       body.MovePosition(pos);
                   }*/
                agent.destination = BumperCarLevel.Instance.aICarsList[chooseIndexRand].transform.position;
            }
        }
        else
        {
            agent.destination = new Vector3(transform.position.x+BumperCarLevel.Instance.aINotStabilityVector.x,
                transform.position.y+ BumperCarLevel.Instance.aINotStabilityVector.y,
                transform.position.z+ BumperCarLevel.Instance.aINotStabilityVector.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AICars"))
        {
            waitMode = true;
            Run.After(BumperCarLevel.Instance.aIStabilitySecond, async () => 
            {
                waitMode = false;
            });
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            impulseForce = collision.impulse;
            collision.rigidbody.AddForce(impulseForce * collision.rigidbody.mass/10, ForceMode.Impulse);

            waitMode = true;
            Run.After(BumperCarLevel.Instance.aIStabilitySecond, async () =>
            {
                waitMode = false;
            });
        }
    }
}