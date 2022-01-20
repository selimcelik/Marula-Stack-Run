using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ManagerActorFramework;
using Cinemachine;

public class PlayerMovement : Actor<LevelManager>
{
    public static PlayerMovement instance;

    public bool levelEnd = false;
    public bool startLevel = false;
    public bool levelFailed = false;

    public bool MoveByTouch;
    private Vector3 _mouseStartPos, PlayerStartPos;
    [Range(0f, 100f)] public float maxAcceleration;
    private Vector3 move, direction;
    public Transform target; // the player will look at this object, which is in front of it

    public float speed;

    Animator animator;

    [SerializeField]
    private GameObject Skates;

    public int skateCount = 0;
    public int gainedScore = 0;

    public bool powerUpBool = false;

    private bool canCollect = false;

    [SerializeField]
    GameObject collectParticle;
    [SerializeField]
    GameObject dropParticle;

    protected override void MB_Awake()
    {
        instance = this;
        startLevel = false;
    }

    protected override void MB_Start()
    {
        Debug.Log(GameManager.Instance.gameData.Score);
        animator = GetComponent<Animator>();
    }

    protected override void MB_Update()
    {
        if (transform.position.y < 0)
        {
            transform.localPosition = new Vector3(transform.position.x,0,transform.position.z);
        }
        if (!levelEnd && startLevel && !levelFailed)
        {
            if (!Skates.activeSelf)
            {
                Skates.SetActive(true);
                for (int i = 0; i < Skates.transform.childCount; i++)
                {
                    Skates.transform.GetChild(i).gameObject.SetActive(false);
                }
                skateCount = 0;
            }
            if (powerUpBool)
            {
                StartCoroutine(powerUp());
                powerUpBool = false;
            }
            transform.position += new Vector3(0, 0f, 1f) * Time.deltaTime * speed;

            if (Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, 0f);

                float Distance;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out Distance))
                {
                    _mouseStartPos = ray.GetPoint(Distance);
                    PlayerStartPos = transform.position;
                }

                MoveByTouch = true;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                MoveByTouch = false;
            }

            if (MoveByTouch)
            {
                Plane plane = new Plane(Vector3.up, 0f);
                float Distance;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                if (plane.Raycast(ray, out Distance))
                {
                    var newray = ray.GetPoint(Distance);
                    move = newray - _mouseStartPos;
                    var controller = PlayerStartPos + move;

                    controller.x = Mathf.Clamp(controller.x, -4.3f, 4.14f);

                    var TargetNewPos = target.position;

                    TargetNewPos.x = Mathf.MoveTowards(TargetNewPos.x, controller.x, 80f * Time.deltaTime);
                    TargetNewPos.z = Mathf.MoveTowards(TargetNewPos.z, 1000f, 10f * Time.deltaTime);

                    target.position = TargetNewPos;

                    var PlayerNewPos = transform.position;

                    PlayerNewPos.x = Mathf.MoveTowards(PlayerNewPos.x, controller.x, 10 * Time.deltaTime);
                    //PlayerNewPos.z = Mathf.MoveTowards(PlayerNewPos.z, 1000f, 10f * Time.deltaTime);
                    transform.position = PlayerNewPos;
                }
            }
        }

        if (skateCount <= 0)
        {
            canCollect = false;
            skateCount = 0;
        }

        animator.SetBool("canRun", startLevel);
        animator.SetBool("canSkate", canCollect);
        animator.SetBool("canDance", levelEnd);
        animator.SetBool("canDie", levelFailed);

    }

    void PlayerPosUp()
    {
        canCollect = true;
        Vector3 playerpos = transform.localPosition;
        playerpos.y += 0.25f;
        transform.localPosition = playerpos;
        SkateEnable();
        ActiveBoxCollider();
    }
    void PlayerPosDown()
    {
        Vector3 playerpos = transform.localPosition;
        playerpos.y -= 0.5f;
        transform.localPosition = playerpos;
        SkateDisable();
        ActiveBoxCollider();
        SkateDisable();
        ActiveBoxCollider();
    }
    void PlayerPosDown2()
    {
        Vector3 playerpos = transform.localPosition;
        playerpos.y -= 0.25f;
        transform.localPosition = playerpos;
        SkateDisable();
        ActiveBoxCollider();
    }
    void SkateEnable()
    {
        Skates.transform.GetChild(skateCount).gameObject.SetActive(true);
        skateCount++;
    }
    void SkateDisable()
    {
        if (skateCount > 0)
        {
            Skates.transform.GetChild(skateCount - 1).gameObject.SetActive(false);
            skateCount--;
        }

    }
    void ActiveBoxCollider()
    {
        if (skateCount > 0)
        {
            if (Skates.transform.childCount > 0)
            {
                for (int i = 0; i < Skates.transform.childCount; i++)
                {
                    if (Skates.transform.GetChild(i).gameObject.activeSelf)
                    {
                        Skates.transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
                    }
                    else
                    {
                        Skates.transform.GetChild(i - 1).GetComponent<BoxCollider>().enabled = true;
                        break;
                    }
                }
            }

        }
        else
        {
            levelFailed = true;
            startLevel = false;
            Push(ManagerEvents.FinishLevel, false);
        }


    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Collectable")
        {
            GameObject particleGO = Instantiate(collectParticle, new Vector3(transform.position.x,transform.position.y+1,transform.position.z), Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(particleGO, 0.5f);
            PlayerPosUp();
        }

        if(other.gameObject.tag == "Obstacle0.25")
        {
            GameObject particleGO = Instantiate(dropParticle, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(particleGO, 0.5f);
            PlayerPosDown2();
        }

        if (other.gameObject.tag == "Obstacle0.5")
        {
            GameObject particleGO = Instantiate(dropParticle, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(particleGO, 0.5f);
            PlayerPosDown();
        }

        if(other.gameObject.tag == "LevelEnd")
        {
            Skates.SetActive(false);
            transform.localPosition = new Vector3(0, 0, transform.position.z);
            other.gameObject.SetActive(false);
            gainedScore = skateCount * 10;
            GameManager.Instance.gameData.AddScore(gainedScore);
            Push(ManagerEvents.UpdateScore, GameManager.Instance.gameData.Score);
            levelEnd = true;
            Push(ManagerEvents.FinishLevel, true);

        }
    }

    IEnumerator powerUp()
    {
        speed += 5;
        yield return new WaitForSeconds(5);
        speed -= 5;
    }
}