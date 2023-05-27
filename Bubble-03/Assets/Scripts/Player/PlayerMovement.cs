using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public int jump;
    public Animator anim;
    public float speed = 3f;
    public float jumpForce = 400f;
    public float vertMov;
    public int left;
    public int right;
    public bool isDead;
    public playerCollision cs;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float elapsedTime;
    private float percentage;

    public static bool GameIsEnded = false;

    void Start()
     {
        jump = 0;
        left = 0;
        right = 1;
        isDead = false;
        elapsedTime = -1f;
    }
    void Update()
    {
        if (elapsedTime != -1f && !cs.correctPos)
        {
            cs.correctPos = false;
            elapsedTime += Time.deltaTime;
            percentage = elapsedTime / 0.6f;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentage);
        }
        if (elapsedTime != -1f && percentage >= 1) 
        {
            elapsedTime = -1f;
            cs.correctPos = true;
        }
        if (cs.tag == "Recta" && elapsedTime == -1f && !cs.correctPos)
        {
            cs.correctPos = false;
            elapsedTime = 0f;
            startPosition = rb.transform.position;
            endPosition = new Vector3(cs.centerX, rb.transform.position.y, cs.centerZ);
        }
        if (rb.transform.position.y < 1) isDead = true;
        if (!isDead){
            anim.SetFloat("movY", Time.deltaTime * speed);
            transform.Translate(0, 0, Time.deltaTime * speed);
            GameIsEnded = false;

        }
        else{
            StartCoroutine(WaitAfterDead());
        }
        
        if (Input.GetKeyDown("space") ) {
            if (jump < 2 && left <= 1 && right <= 1) {
                rb.AddForce(0, jumpForce, 0);
                ++jump;
                
            }
            else if(left == 2 && jump == 0){
                
                transform.Rotate(0, -90, 0);
                left = 0;
                right = 1;
                
            } else if (right == 2 && jump == 0)  {
                
                transform.Rotate(0, 90, 0);
                left = 1;
                right = 0;
                
            }

            
        }
    }

    private IEnumerator WaitAfterDead()
    {
        anim.SetBool("Death", true);
        yield return new WaitForSeconds(1);
        GameIsEnded = true;
    }

}
 