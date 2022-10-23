using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMeteor : MonoBehaviour
{
    public float MeteorSpeed = -100f;   //메테오속도
    public Slider bossHpSlider;       //보스HP표시해준는 슬라이더
    public float bossInAuraSpeed;   //보스가 오오라에 끌려가는 스피드

    private Rigidbody2D bossRigidbody;    //메테오 리지드 바디불러오기
    private RectTransform rectTransform;    //메테오 방향
    private int bossHp;                   //보스피
    private bool t;                         //보스가 블랙홀에 닿았는지를 표시
    private Vector2 originPos;              //보스가 멈췄을 때의 위치를 표시
    private int bossDamage;                 //보스의 대미지
    

    // Start is called before the first frame update
    void Start()
    {

        Stage stage = GameObject.Find("Stage").GetComponent<Stage>();
        bossInAuraSpeed = 1;

        bossHp = 100*stage.stageValue;
        bossDamage = 1*stage.stageValue;
        t = false;
        rectTransform=GetComponent<RectTransform>();
        bossRigidbody = GetComponent<Rigidbody2D>();  //보스 리지드 바디 불러오기
        bossRigidbody.velocity = rectTransform.transform.right * MeteorSpeed; // 왼쪽으로 가기
        bossHpSlider.maxValue = bossHp;  //보스 슬라이더 최대치 설정
        bossHpSlider.value = bossHpSlider.maxValue;     //보스 슬라이더 현재값 설정
        gameObject.transform.Find("BossHpSlider").gameObject.SetActive(false);
        StartCoroutine(PlayerDieCheck());

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerSc pSc = GameObject.Find("PlayerMax").GetComponent<PlayerSc>();
        DataManager dM = GameObject.Find("MoneyManager").GetComponent<DataManager>();
        if (other.tag=="Aura")
        {
            bossRigidbody.velocity = new Vector2 (0,0); //보스 정지
            t = true;   //오오라 만났음으로 충돌은 true
            gameObject.transform.Find("BossHpSlider").gameObject.SetActive(true);
            StartCoroutine(BossMagnetic());
            StartCoroutine(BossFight());    //boss fight실행
        }
        if (other.tag=="Player")
        {
            t = false;
            StartCoroutine(PlayerEating());
        }
    }

    IEnumerator BossMagnetic()    //보스가 오오라 만났을때 빨려들어가는 함수
    {
        RectTransform playerPlace = GameObject.Find("Player").GetComponent<RectTransform>();
        while (t)
        {
            rectTransform.position = Vector2.MoveTowards(rectTransform.position, playerPlace.position, bossInAuraSpeed);   //보스 이동
            yield return null;
        }
    }

        IEnumerator BossFight()
    {
        Stage stage = GameObject.Find("Stage").GetComponent<Stage>();
        PlayerSc pSc = GameObject.Find("PlayerMax").GetComponent<PlayerSc>();
        DataManager dM = GameObject.Find("MoneyManager").GetComponent<DataManager>();

        while (true)
        {
            bossHpSlider.value -= pSc.damage;
            if(bossHpSlider.value==0)
            {
                dM.gold += bossHp * pSc.goldV;
                stage.stageText.text = "Stage: " + stage.stageValue;
                stage.stageText.fontSize = 15;
                GameObject.Find("Stage").GetComponent<Stage>().enabled = true;
                GameObject.Find("Stage").transform.Find("StageSlider").gameObject.SetActive(true);
                GameObject.Find("MeteorSpawner").GetComponent<MeteorSpawner>().enabled = true;
                GameObject.Find("MeteorSpawner1").GetComponent<MeteorSpawner>().enabled = true;
                GameObject.Find("MeteorSpawner2").GetComponent<MeteorSpawner>().enabled = true;
                GameObject.Find("MeteorSpawner3").GetComponent<MeteorSpawner>().enabled = true;
                stage.isBossSpawn = false;
                Destroy(gameObject, 0f);
            }
            yield return null;
        }
    }

    IEnumerator PlayerEating()  //플레이어 피 차게하는거  플레이어 스크립트의 피, 보스의 데미지 필요   보스가 플레이어에 닿으면
    {
        PlayerSc pSc = GameObject.Find("PlayerMax").GetComponent<PlayerSc>();
        while (true)
        {
            pSc.hp += bossDamage;
            yield return null;
        }
    }
    IEnumerator PlayerDieCheck()
    {
        Stage stage = GameObject.Find("Stage").GetComponent<Stage>();
        PlayerSc pSc = GameObject.Find("PlayerMax").GetComponent<PlayerSc>();
        while (true)
        {
            if (pSc.isPlayerDie == true)
            {
                stage.stageText.text = "Stage: " + stage.stageValue;
                stage.stageText.fontSize = 15;
                GameObject.Find("Stage").GetComponent<Stage>().enabled = true;
                GameObject.Find("Stage").transform.Find("StageSlider").gameObject.SetActive(true);
                GameObject.Find("MeteorSpawner").GetComponent<MeteorSpawner>().enabled = true;
                GameObject.Find("MeteorSpawner1").GetComponent<MeteorSpawner>().enabled = true;
                GameObject.Find("MeteorSpawner2").GetComponent<MeteorSpawner>().enabled = true;
                GameObject.Find("MeteorSpawner3").GetComponent<MeteorSpawner>().enabled = true;
                stage.isBossSpawn = false;
                Destroy(gameObject, 0f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}