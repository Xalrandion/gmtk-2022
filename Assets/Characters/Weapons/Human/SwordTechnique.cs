using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTechnique : MonoBehaviour
{
    public GameObject swordman;
    private Animator anim;
    private Transform position;
    private Vector3 targetPosition;
    private Vector3 startingPosition;
    private bool isCharging = false;
    public float travelTime;
    private float travelStart;

    void Start()
    {
        anim = swordman.GetComponent<Animator>();
        position = swordman.GetComponent<Transform>();
    }
    void Update()
    {
        if (isCharging == true) {
            anim.SetFloat("speed", 1f);
            position.position = Vector3.Lerp(startingPosition, targetPosition, (Time.time - travelStart) / travelTime);
            if (Time.time - travelStart > travelTime) {
                anim.SetFloat("speed", 0f);
                anim.Play("MeleeAttack");
                isCharging = false;
            }
        }
    }

    public void onAttackEnded() {
        position.position = startingPosition;
    }

    public void Charge(Vector3 targetPosition)
    {
        startingPosition = swordman.transform.position;
        this.targetPosition = targetPosition;
        travelStart = Time.time;
        isCharging = true;
    }
}
