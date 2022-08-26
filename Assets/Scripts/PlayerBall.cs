using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    public float jumpPower = 10;
    public float jumpBlockPower = 100;
    public int itemCount;
    public GameManagerLogic manager;
    bool isJump;
    Rigidbody rigid;
    AudioSource audio;

    void Awake()
    {
        isJump = false;
        manager.GetItem(itemCount);
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Jump") && !isJump)
        {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);            
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(isJump == true){
            h = h/2;
            v = v/2;
        }
        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
        else if(collision.gameObject.tag == "JumpBlock")
        {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpBlockPower, 0), ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Item")
        {
            itemCount += 1;
            audio.Play();
            other.gameObject.SetActive(false);
            manager.GetItem(itemCount);
        }
        else if(other.tag == "Finish")
        {
            if(itemCount == manager.totalItemCount)
            {
                //claer
                manager.stage += 1;
                SceneManager.LoadScene("Example1_" + (manager.stage));
            }
            else
            {
                //Restart
                SceneManager.LoadScene("Example1_"+ (manager.stage));
            }
        }
    }
}
