using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_vTargetPos;
    [SerializeField]
    private Vector3 m_vInitialVel;
    [SerializeField]
    public bool m_bDebugKickBall = false;
    [SerializeField]
    public bool m_bRestart = false;
    [SerializeField]
    public int goalCount = 0;
    private Rigidbody m_rb = null;
    private GameObject m_TargetDisplay = null;
    [SerializeField]
    public GameObject leftPost;
    private bool m_bIsGrounded = true;
    [SerializeField]
    private bool addScore = false;
    private float m_fDistanceToTarget = 0f;
    private Vector3 initialPos = new Vector3(0.0f, 0.0f, -20.0f);
  
    private Vector3 vDebugHeading;
    private bool goal = false;
  


    //UI
    [SerializeField]
    public Text txt;
    //[SerializeField]
    //private AudioSource audioData;
    [SerializeField]
    public InputField Xpos, Ypos, Zpos;
    // Start is called before the first frame update
    /*
     * 
     * In this function I set the 3 input fields that we have to zero
     */

    void Start()
    {
       // audioData = GetComponent<AudioSource>();
       // audioData.Play(0);
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Houston, we've got a problem here! No Rigidbody attached");
        Xpos.GetComponent<UnityEngine.UI.InputField>().text = "0";
        Ypos.GetComponent<UnityEngine.UI.InputField>().text = "0";
        Zpos.GetComponent<UnityEngine.UI.InputField>().text = "0";
        CreateTargetDisplay();
 
      
    }

    // Update is called once per frame
    /*
     * By uncommenting the movePlayer function, you can move the player but since it is penalty kick game,
     * I thought it is better to comment this.
     * 
     * next i Check the target position, if it is equal to (0,0,0), it means that the player has not set the target
     * or the user has restarted the game. So I set the velocity of ball to zero cause I don't want it to move.
     * */
    void Update()
    {
       // movePlayer();
        updateTargetPos();

            if (m_TargetDisplay != null && m_bIsGrounded)
            {
                if (m_vTargetPos.y == 0 && m_vTargetPos.z == 0 && m_vTargetPos.x == 0)
                {

                    m_TargetDisplay.transform.position = Vector3.zero;
                    m_vInitialVel = Vector3.zero;
                    m_rb.velocity = m_vInitialVel;
                    vDebugHeading = Vector3.zero;
                }
                //If the ball is not in the goal position,  I still calculate the heading and target display position
                else
                {
                    if (!goal)
                    {
                        m_TargetDisplay.transform.position = m_vTargetPos;

                        vDebugHeading = m_vTargetPos - transform.position;
                    }
                }
            }
            //If the user presses shoot, he can kick the ball
            if (m_bDebugKickBall && m_bIsGrounded)
            {
                m_bDebugKickBall = false;
            addScore = false;
            OnKickBall();
            }
            //If the user restart the game, every variable restarts as well except the score
            if (m_bRestart)
            {
                m_vTargetPos = Vector3.zero;
                m_TargetDisplay.transform.position = m_vTargetPos;
                transform.position = initialPos;
                goal = false;
            addScore = false;
                m_rb.position = initialPos;
            Xpos.GetComponent<UnityEngine.UI.InputField>().text = "0";
            Ypos.GetComponent<UnityEngine.UI.InputField>().text = "0";
            Zpos.GetComponent<UnityEngine.UI.InputField>().text = "0";
            m_bRestart = false;

            }
            //Check to see if the ball is whithin the goal post
            checkBound();
        //If the ball is in the goal, I multiply the velocity in the y axis casue I want the ball to fall there.
        //I also add to the score
        if (goal)
        {
            if(!addScore)
            goalCount++;
            addScore = true;
            m_vInitialVel.z = 0.0f;
            if (m_vInitialVel.y > 0.0f)
                m_vInitialVel.y = -1 * m_vInitialVel.y;
            m_rb.velocity = m_vInitialVel;
            goal = false;
        }
    }
    //In this function, I update the target position by the inputs I received from the player
  private void updateTargetPos()
    {
        m_vTargetPos.x = float.Parse(Xpos.GetComponent<UnityEngine.UI.InputField>().text);
        m_vTargetPos.y = float.Parse(Ypos.GetComponent<UnityEngine.UI.InputField>().text);
        m_vTargetPos.z = float.Parse(Zpos.GetComponent<UnityEngine.UI.InputField>().text);
   
    }
    //In this function, I create the red target sign
    private void CreateTargetDisplay()
    {
        m_TargetDisplay = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        m_TargetDisplay.transform.position = Vector3.zero;
        m_TargetDisplay.transform.localScale = new Vector3(2.0f, 0.1f, 2.0f);
        m_TargetDisplay.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        m_TargetDisplay.GetComponent<Renderer>().material.color = Color.red;
        m_TargetDisplay.GetComponent<Collider>().enabled = false;
    }
    //Checkbound function checks to see if the ball is in the goal
    private void checkBound()
    {
        if((Mathf.Abs(leftPost.transform.position.x-transform.position.x)<19)&&
            (Mathf.Abs(leftPost.transform.position.y - transform.position.y) < 8)&&
           (((transform.position.z) > 0.03f) && ((transform.position.z) < 9.74f)))
        {
            
                
            if (Mathf.Abs(transform.position.z - 9.0f)<2.0f)
            {


                txt.GetComponent<UnityEngine.UI.Text>().text = goalCount.ToString();


                
               goal = true;
               
            }
        }
    }
    //To move the player with WASD
    private void movePlayer()
    {
        if (Input.GetKey(KeyCode.A))
            transform.position = new Vector3(transform.position.x - 0.05f, transform.position.y, transform.position.z);
          
        if (Input.GetKey(KeyCode.D))
            transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y, transform.position.z);
        if (Input.GetKey(KeyCode.W))
            transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z+0.05f);
        if (Input.GetKey(KeyCode.S))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.05f);
    }
    //To kick the ball
    public void OnKickBall()
    {
        // H = Vi^2 * sin^2(theta) / 2g
        // R = 2Vi^2 * cos(theta) * sin(theta) / g

        // Vi = sqrt(2gh) / sin(tan^-1(4h/r))
        // theta = tan^-1(4h/r)

        // Vy = V * sin(theta)
        // Vz = V * cos(theta)

        float fMaxHeight = m_TargetDisplay.transform.position.y;
        m_fDistanceToTarget = (m_TargetDisplay.transform.position - transform.position).magnitude;
        float fRange = (m_fDistanceToTarget * 2);
        float fTheta = Mathf.Atan((4 * fMaxHeight) / (fRange));

          float fInitVelMag = Mathf.Sqrt((2 * Mathf.Abs(Physics.gravity.y) * fMaxHeight)) / Mathf.Sin(fTheta);
       //Then I calculate the direction in which the ball should move
     
        Vector3 direction = Vector3.Normalize(m_TargetDisplay.transform.position - transform.position);
        m_vInitialVel.x = direction.x * fInitVelMag;
        m_vInitialVel.y = direction.y*4*fInitVelMag * Mathf.Sin(fTheta);
        m_vInitialVel.z = direction.z*fInitVelMag * Mathf.Cos(fTheta);
      
        m_rb.velocity = m_vInitialVel;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + vDebugHeading, transform.position);
    }
   
}
