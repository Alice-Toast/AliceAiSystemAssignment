using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardianStateMachine : MonoBehaviour
{
    public enum State {Idle, Guard, Defend, Heal}

    public State state;

    public float Hp = 10;

    public bool RootsFelt;

    public float RootDistance;

    public GameObject target;

    public float atkCooldown = 2f;
    float currentCooldown = 0f;

    private NavMeshAgent agent;

    private float healingCooldown;


    public MeshRenderer Indicator;

    public Material idle;
    public Material guard;
    public Material defend;
    public Material heal;
    private void Awake()
    {
        state = State.Idle;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Hp < 0)
        {
            Destroy(gameObject);
        }
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Guard:
                Guard();
                break;
            case State.Defend:
                Defend();
                break;
            case State.Heal:
                Heal();
                break;
        }
    }

    void Idle()
    {
        if (Hp < 20)
        {
            state = State.Heal;
            Indicator.material = heal;
        }
        if (RootDistance < 5)
        {
            state = State.Guard;
            RootsFelt = true;
            Indicator.material = guard;
        }
    }

    void Guard()
    {
        if (RootDistance > 6)
        {
            RootsFelt = false;
            state = State.Idle;
            Indicator.material = idle;
        }

        if (RootDistance < 2)
        {
            state = State.Defend;
            Indicator.material = defend;
        }
    }

    void Defend()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            currentCooldown = atkCooldown;
            if (RootDistance < 2)
            {
                target.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(2,2,2), transform.position);
            }
            
        }
        agent.destination = target.transform.position;

        if (RootDistance > 7)
        {
            state = State.Idle;
            RootsFelt = false;
            Indicator.material = idle;
        }

    }

    void Heal()
    {
        agent.destination = transform.position-target.transform.position;
        if (healingCooldown < 0)
        {
            Hp += 3;
            healingCooldown = 1;
        }
        if (Hp > 70)
        {
            state = State.Idle;
            Indicator.material = idle;
        }
        healingCooldown -= Time.deltaTime;
    }



    private void OnTriggerStay(Collider other)
    {
        
        if(other.tag == "Player")
        {
            RootDistance = Mathf.Abs(transform.position.x - other.transform.position.x) + Mathf.Abs(transform.position.z - other.transform.position.z);
            if (RootsFelt)
            {
                target = other.gameObject;
            }
        }
        
    }

}
