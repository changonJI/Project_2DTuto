using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public int health;

    public PlayerMove player;

    public GameObject[] Stages;

    //UI
    public Image[] UIHealth;
    public Text UIPoint;
    public Text UIStage;
    public Button UIRestart;

    private void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        Stages[stageIndex].SetActive(false);
        stageIndex++;

        //Change Stage
        if (stageIndex < Stages.Length)
        {
            Stages[stageIndex].SetActive(true);
            PlayerRepostion();

            UIStage.text = $"STAGE {stageIndex + 1}";
        }
        // GameClear
        else
        {
            //Player Control Lock
            Time.timeScale = 0;

            //Result UI
            Debug.Log("게임 클리어");

            // retry button ui
            ViewBnt();
            Text btnText = UIRestart.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
        }

        //Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIHealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            //All Health UI Off
            UIHealth[0].color = new Color(1, 0, 0, 0.4f);

            // player die effect
            player.OnDie();

            // retry button ui
            ViewBnt();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Player Reposition
            if (health > 1)
            {
                //NOTE : RigidBody를 바로 가져옴
                //collision.attachedRigidbody.velocity = Vector3.zero;
                PlayerRepostion();
            }

            // Health Down
            HealthDown();

        }
    }

    void PlayerRepostion()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    void ViewBnt()
    {
        UIRestart.gameObject.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
