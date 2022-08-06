using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
    [SerializeField] Text scoreValueText;
    
    private void Update()
    {
        transform.Rotate(new Vector3(0f, 1.85f, 0f));
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            /* int scoreValue = int.Parse(scoreValueText.text);
             scoreValue += 50;
             scoreValueText.text = scoreValue.ToString();*/
            //ScoreManager.instance.addPoint();
            GameObject.Find("LevelManager").GetComponent<LevelManager>().AddScore(50); ;

            Destroy(gameObject);
        }


    }
}
//using System.Collections;
 //using System.Collections.Generic;
 //using UnityEngine;
 //using UnityEngine.UI;

//public class ScoreManager : MonoBehaviour
//{

//    private Text scoreValueText;
//    public int scoreValue;
//    int score = 0;


//    public static ScoreManager instance;
//    public Text highScoreText;
//    int highscore = 0;


//    private void Awake()
//    {
//        instance = this;


//    }

//    void Start()
//    {
//        highscore = PlayerPrefs.GetInt("highscore",0);
//        highScoreText.text = "HIGHSCORE" + highscore.ToString();
//    }

//    public void addPoint(int score)
//    {
//        scoreValue = int.Parse(scoreValueText.text);
//        scoreValue += score;
//        scoreValueText.text = scoreValue.ToString();
//        if(highscore<score)
//        PlayerPrefs.SetInt("highscore",score);
//    }




//}