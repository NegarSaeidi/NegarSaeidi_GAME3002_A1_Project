using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public BallPhysics ballBehavior;
    //[SerializeField]
    //private AudioSource audioData;
    void Start()
    {
        ballBehavior = GetComponent<BallPhysics>();
       // audioData = GetComponent<AudioSource>();
       // audioData.Play(0);
    }
    
      
        public void OnButtonPress()
        {
            
        ballBehavior.m_bRestart = true;
        
        }
    public void OnShoot()
    {

        ballBehavior.m_bDebugKickBall = true;

    }
    public void LoadScene()
    {
        SceneManager.LoadScene("SceneTwo");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
