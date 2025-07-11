using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : WeakAnimal
{
    protected override void Reset1()
    {
        base.Reset1();
        RandomAction();
    }

    void RandomAction()
    {
        RandomSound();
        isAction = true;
        int _random = Random.Range(0, 4); //���, �Ա�, ã��, �ȱ�

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
        Debug.Log("���");
    }
    void Eat()
    {
        currentTime = waitTime;
        Debug.Log("�Ա�");
        anim.SetTrigger("Eat");
    }
    void Peek()
    {
        currentTime = waitTime;
        Debug.Log("ã��");
        anim.SetTrigger("Peek");
    }
}
