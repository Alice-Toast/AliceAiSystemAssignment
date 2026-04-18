using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage;
    bool isAttacking;
    public GameObject attackIndicator;
    bool cooldown = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && !cooldown)
        {
            isAttacking = true;
            cooldown = true;
            StartCoroutine(attacking());
        }
        
    }

    IEnumerator attacking()
    {
        attackIndicator.SetActive(true);
        yield return new WaitForSeconds(1);
        attackIndicator.SetActive(false);
        isAttacking = false;
        yield return new WaitForSeconds(0.5f);
        cooldown = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "evil" && isAttacking)
        {
            collision.gameObject.GetComponent<GuardianStateMachine>().Hp -= damage;
        }
    }
}
