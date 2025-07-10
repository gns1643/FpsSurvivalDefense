using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName; //동물의 이름
    [SerializeField] private int hp; //동물의 체력

    [SerializeField] private float walkSpeed; //동물의 걷는 속도

    private Vector3 direction;

    //상태 변수
    private bool isWalking; //걷고 있는가?
    private bool isAction; //행동 중인가?

    [SerializeField] private float walkTime; //걷는 시간
    [SerializeField] private float waitTime; //대기 시간
    private float currentTime;

    //필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;
    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        ElapseTime();
        Move();
        Rotation();
    }

    void Move()
    {
        if (isWalking)
        {
            rigid.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
        }
    }
    void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                Reset1();
            }
        }
    }
    private void Reset1()
    {
        isWalking = false; isAction = true;
        anim.SetBool("Walking", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }
    void RandomAction()
    {
        isAction = true;
        int _random = Random.Range(0, 4); //대기, 먹기, 찾기, 걷기

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
            
    }
    void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    void Eat()
    {
        currentTime = waitTime;
        Debug.Log("먹기");
        anim.SetTrigger("Eat");
    }
    void Peek()
    {
        currentTime = waitTime;
        Debug.Log("찾기");
        anim.SetTrigger("Peek");
    }
    void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        Debug.Log("걷기");
        anim.SetBool("Walking", isWalking);
    }
}
