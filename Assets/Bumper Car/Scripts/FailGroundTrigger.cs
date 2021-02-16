using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshCollider))]
public class FailGroundTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // TODO: Düşen Nesnelerin Tespiti
        if (other.gameObject.CompareTag("AICars"))
        {
            other.gameObject.SetActive(false);

            BumperCarLevel.Instance.aICarsList[other.gameObject.GetComponent<AICars>().thisObjectIndex] = null;
            BumperCarLevel.Instance.aIListCountControl++;
            int x = 0;
            for (int i = 0; i < BumperCarLevel.Instance.aICarsList.Count; i++)
            {
                if (BumperCarLevel.Instance.aICarsList[i] == null)
                {
                    x++;
                    if (BumperCarLevel.Instance.aICarsList.Count - 1 == x)
                    {
                        BumperCarLevel.Instance.WinCondition();  
                    }
                }
            }         
        }
        if (other.gameObject.CompareTag("Player"))
        {
            BumperCarLevel.Instance.FailCondition();
        }
    }
}
