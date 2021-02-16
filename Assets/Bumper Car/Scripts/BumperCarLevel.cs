using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
public class BumperCarLevel : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    private static BumperCarLevel _instance;
    public Transform finalCameraTarget;
    public Transform mainCameraCinemachine;
    public float cameraChangeSpeed;
    public Transform lookCamera;
    public List<GameObject> aICarsList = new List<GameObject>();
    public int aIListCountControl=0;
    public float aIStabilitySecond = 3f;
    public Vector3 aINotStabilityVector;
    public float aIRandomTargetSecond;
    public GameObject tryButton, nextButton;
    public GameObject confettiEffect;
    public PlayMode playMode = PlayMode.NotActive;
    public enum PlayMode
    {
        NotActive,
        Playing,
        Fail,
        Win,
    }
    public static BumperCarLevel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<BumperCarLevel>();
            }
            return _instance;
        }
    }
    // TODO: Win
    public void WinCondition()
    {
        nextButton.SetActive(true);
        confettiEffect.SetActive(true);
    } 
    // TODO: Fail
    public void FailCondition()
    {
        tryButton.SetActive(true);
        cinemachineVirtualCamera.gameObject.SetActive(false);
        Run loop = Run.EachFrame(() =>
        {
            try
            {
                Camera.main.transform.position = Vector3.Lerp(mainCameraCinemachine.position, Camera.main.transform.position+new Vector3(0,20f,0f), cameraChangeSpeed * Time.deltaTime);
                Camera.main.transform.LookAt(lookCamera);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        });
        Run.After(2f, loop.Abort);
    }
    // TODO: Level Manager
    public void nextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        if (level==2)
        {//level sayısı dışarından alınıp kontrol sağlanıcak.
            level=level - 3;
        }
        level++;
        SceneManager.LoadScene(level);
    }
    public void tryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
