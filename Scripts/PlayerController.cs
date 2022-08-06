using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // üçlü zıplama

    private float mySpeedX;
    [SerializeField] float speed;
    [SerializeField] float JumpPower;
    private Rigidbody2D myBody;
    private Vector3 defaultLocalScale;
    public bool onGround;
    private bool canDoubleJump;
    [SerializeField] GameObject arrow;
    ////public bool canTribleJump;
    ////[SerializeField] GameObject arrow;
    [SerializeField] bool attacked;
    [SerializeField] float currentAttackTimer;
    [SerializeField] float defaultAttackTimer;
    private Animator myAnimator;
    [SerializeField] int arrowNumber;
    [SerializeField] Text arrowNumberText;
    [SerializeField] AudioClip dieMusic;
    [SerializeField] GameObject winPanel, losePanel;



    //private Vector2 screenBound;
    //private float objectWitdh;
    //private float objectHeight;   



    void Start()
    {
        attacked = false;
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();       // burayı yazma nedenimiz yazılımın get component'ta tekrara düşmesini engellemek adınailk frame'de çağırıyoz.
        defaultLocalScale = transform.localScale;
        arrowNumberText.text = arrowNumber.ToString();
        //canDoubleJump = true;



        //screenBound = Camera.main.ScreenToWorldPoint(new Vector3(-50f,-100f,Camera.main.transform.position.z));
        //objectWitdh = transform.GetComponent<SpriteRenderer>().bounds.size.x/2;
        //objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y/2;



    }
    //private void LateUpdate()
    //{
    //    Vector3 viewPos = transform.position;
    //    viewPos.x = Mathf.Clamp(viewPos.x, screenBound.x+objectWitdh, screenBound.x*-1-objectWitdh);
    //    viewPos.y = Mathf.Clamp(viewPos.y, screenBound.y+objectHeight, screenBound.y*-1-objectHeight);
    //    transform.position = viewPos;

    //}



    void Update()
    {

        //IsOnGround();
        // Debug.Log(Input.GetAxis("Horizontal"));     //string ifadeyi de doğru yazmak lazım
        mySpeedX = Input.GetAxis("Horizontal"); //-1 ile 1 arasında sağ ve sol ok tuşuna bağlı olarak değerler gelcek
        gameObject.GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(mySpeedX));
        //GetComponent<Rigidbody2D>().velocity = new Vector2(mySpeedX * speed, GetComponent<Rigidbody2D>().velocity.y);
        myBody.velocity = new Vector2(mySpeedX * speed, myBody.velocity.y);
        ////myAnimator.SetFloat("Speed", Mathf.Abs(mySpeedX));//skaler olarak almak lazımdı
        //                                                  Debug.Log("Frame Mantığı");



        #region karakterin sağ sola yönsel dönmesi
        if (mySpeedX > 0)
        {
            transform.localScale = new Vector3(defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        else if (mySpeedX < 0)
        {
            transform.localScale = new Vector3(-defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        #endregion



        #region playerın havada 2kere zıplaması
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("og"+onGround);
        //    if (onGround == true)
        //    {
        //        myBody.velocity = new Vector2(myBody.velocity.x, JumpPower);
        //        myAnimator.SetTrigger("Jump");
        //        canDoubleJump = true;


        //        if (canDoubleJump == true)
        //        {

        //            myBody.velocity = new Vector2(myBody.velocity.x, JumpPower);

        //            canDoubleJump = false;
        //            Debug.Log("dabılcamp"+canDoubleJump);
        //        }

        //    }

        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(onGround)
            {
                jump(true);
            }
            else if (!onGround && canDoubleJump)
            {
                jump(false);
            }

        }
        #endregion



        #region player'ın ok atmasının kontrolü

        if (Input.GetMouseButtonDown(0) && arrowNumber > 0)
        {
            if (attacked == false)
            {
                attacked = true;
                myAnimator.SetTrigger("Attack");
                Invoke("Fire", 0.25f);
                //Fire();
            }
        }
        #endregion
        if (attacked == true)
        {
            currentAttackTimer -= Time.deltaTime;
            //Debug.Log(currentAttackTimer);
        }
        else
        {
            currentAttackTimer = defaultAttackTimer;
        }
        if (currentAttackTimer <= 0)
        {
            attacked = false;
        }









    }







    void Fire()
    {
        GameObject okumuz = Instantiate(arrow, transform.position, Quaternion.identity);
        okumuz.transform.parent = GameObject.Find("Arrow").transform;

        if (transform.localScale.x > 0)
        {
            okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
        }
        else
        {
            Vector3 okumuzLokalScale = okumuz.transform.localScale;
            okumuz.transform.localScale = new Vector3(-okumuzLokalScale.x, okumuzLokalScale.y, okumuzLokalScale.z);

            okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);
        }
        arrowNumber--;
        arrowNumberText.text = arrowNumber.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            /*winPanel.active = true;
            Time.timeScale = 0;*/
            Destroy(collision.gameObject.gameObject);
            StartCoroutine(Wait(true));
        }
        else if (collision.gameObject.CompareTag("cover"))
        {
            Die();
        }


    }


    public void Die()
    {
        GameObject.Find("SoundController").GetComponent<AudioSource>().clip = null;
        GameObject.Find("SoundController").GetComponent<AudioSource>().PlayOneShot(dieMusic);
        myAnimator.SetFloat("Speed", 0);
        myAnimator.SetTrigger("Die");
        //myBody.constraints = RigidbodyConstraints2D.FreezePosition;
        myBody.constraints = RigidbodyConstraints2D.FreezeAll;
        enabled = false;
        losePanel.SetActive(true);
        Time.timeScale = 0;
        //StartCoroutine(Wait(false));
    }
    IEnumerator Wait(bool win)
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0;
        if (win == true)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
        // 2saniye ertelettirmeyi sağlatıyo acayüüp bişi
        //  winPanel.SetActive(true);


    }
    void jump(bool canjump)
    {

        myBody.velocity = new Vector2(myBody.velocity.x, JumpPower);
        canDoubleJump = canjump;
        myAnimator.SetTrigger("Jump");





    }







}

