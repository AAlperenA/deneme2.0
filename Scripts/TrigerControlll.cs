using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerControlll : MonoBehaviour
{
    [SerializeField] GameObject player;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("tetiklendi");
        player.GetComponent<PlayerController>().onGround = true;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("tetiklenmeden çıktı");
        player.GetComponent<PlayerController>().onGround = false;

    }

    //private void Update()
    //{
    //    player.GetComponent<PlayerController>().onGround = false;


    //}

}
