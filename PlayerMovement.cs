using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sidewaysforce = 500f;
    public float jumpforceY = 200f;
    public float jumpforceZ = 200f;
    public bool checkgrounded = true;
    private float screenCenterX;
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;
    public float SWIPE_THRESHOLD = 10f;

    public void moveright(){
     rb.AddForce(sidewaysforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    }

    public void moveleft(){
     rb.AddForce(-sidewaysforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    }
    
       
    private void OnCollisionEnter(Collision Collision)
    {
      if(Collision.gameObject.name == "Ground"){
          checkgrounded = true;
      }
    }
    
    
   private void Start()
    {
        
        screenCenterX = Screen.width * 0.5f;
    }


    void checkSwipe()
    {
        // Check if Vertical swipe
       if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0 && checkgrounded)//up swipe
            {
                rb.AddForce(0, jumpforceY * Time.deltaTime, jumpforceZ * Time.deltaTime, ForceMode.Impulse);
                
                
                checkgrounded = false;
            }
            
        }
        
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

  
   void FixedUpdate()
    {


         foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }
        

        if (Input.touchCount > 0) {

         // get the first one
            Touch firstTouch = Input.GetTouch(0);
 
                if(firstTouch.position.x > screenCenterX)
                {
                  moveright();
                }
                else if(firstTouch.position.x < screenCenterX)
                {
                  moveleft();
                }

            //}

        }
    
    
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);

        if(Input.GetKey("d")) {

            moveright();

        }

        if(Input.GetKey("a")) {
            
            moveleft();

        }
        
        if(Input.GetKeyDown("space") && checkgrounded) {
         
           rb.AddForce(0, jumpforceY * Time.deltaTime, jumpforceZ * Time.deltaTime, ForceMode.Impulse);
           checkgrounded = false;

        }

       if (rb.position.y < -1.1f){
           
           
            FindObjectOfType<GameManager>().gameoverfelldown();
           

        }
        
    }
    
}
