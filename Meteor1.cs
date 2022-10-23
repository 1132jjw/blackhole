using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor1 : MonoBehaviour
{
    private float MeteorSpeed = -400f;   //메테오속도
    private RectTransform playerPlace;
    public float inAuraSpeed1;

    private int meteorHp;
    private int weight;
    private Rigidbody2D MeteorRigidbody;    //메테오 리지드 바디불러오기
    private RectTransform rectTransform;    //메테오 방향
    private bool t;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform playerPlace = GameObject.Find("Player").GetComponent<RectTransform>();
        //target = FindObjectOfType<PlayerSc>().transform;
        rectTransform =GetComponent<RectTransform>();
        MeteorRigidbody = GetComponent<Rigidbody2D>();  //메테오 리지드 바디 불러오기
        MeteorRigidbody.velocity = rectTransform.right * MeteorSpeed; //메테오 메테오속도로 왼쪽으로 가기
        Stage stage = GameObject.Find("Stage").GetComponent<Stage>();
        weight = stage.stageValue;
        t = false;
        inAuraSpeed1 = 4;   //2초뒤에 메테오 부수기
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerSc pSc = GameObject.Find("PlayerMax").GetComponent<PlayerSc>();
        DataManager dM = GameObject.Find("MoneyManager").GetComponent<DataManager>();
        
        if (other.tag == "Aura")
        {
            t = true;
            MeteorRigidbody.velocity = new Vector2(0, 0);
            //MeteorRigidbody.velocity = playerPlace.transform.forward * MeteorSpeed;
            RectTransform playerPlace = GameObject.Find("Player").GetComponent<RectTransform>();
            Debug.Log(playerPlace.position);
            float degree = Mathf.Atan2(playerPlace.position.y - rectTransform.position.y, playerPlace.position.x - rectTransform.position.y)*180f / Mathf.PI;
            MeteorRigidbody.MoveRotation (degree);
            StartCoroutine(MeteorFight());
            //MeteorRigidbody.MovePosition (playerPlace.position);
            //rectTransform.position = Vector2.MoveTowards(rectTransform.position,playerPlace.position,50);


        }
        if (other.tag == "Player")
        {
            Debug.Log("플레이어");
            pSc.hp += 10 * weight;
            Destroy(gameObject, 2f);
        }
        if (other.tag == "MeteorDeadZone")
        {
            Debug.Log("왜안되");
            Destroy(gameObject, 0f);
        }
    }
    IEnumerator MeteorFight()
    {
        RectTransform playerPlace = GameObject.Find("Player").GetComponent<RectTransform>();
        while (t==true)
        {
            rectTransform.position = Vector2.MoveTowards(rectTransform.position, playerPlace.position, inAuraSpeed1);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
