using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    
    private bool chasing;
    public float distanceToChase = 10f, distanceToLose = 15f, DistanceToStop = 2f;

    private Vector3 targetPoint, startPoint;

    public NavMeshAgent agent;

    private float keepChasingTime = 5f;
    private float chaseCounter;

    public GameObject bullet;
    public Transform firePoint;

    public float fireRate, waitBetweenShots = 2f, timeToShot = 1f;
    private float fireCount, shotWaitCounter, shotTimeCounter;

    public Animator anim;

    private bool wasShot;

    


    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;

        shotTimeCounter = timeToShot;
        shotWaitCounter = waitBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        // da ne skace u vazduh za nama, i da nas ne prati kada se penjemo
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        
        if (!chasing)
        {
            // neprijatelj nas juri 
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;

                shotTimeCounter = timeToShot;
                shotWaitCounter = waitBetweenShots;
            }

            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;
                // ako je promenljiva 0 enemy se vraca na startnu poziciju
                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }

            // za aktiviranje animacije trcanja ako smo blizu njega
            if(agent.remainingDistance < .25f)
            {
                anim.SetBool("isMoving", false);
            } else
            {
                anim.SetBool("isMoving", true);
            }
        }
        else
        {
            // sluzi da se enemy ne zalepi za nas kada nas juri nego da bude udaljen onoliko koliko smo postavili(distanceToStop)
            if(Vector3.Distance(transform.position, targetPoint) > DistanceToStop)
            {
                agent.destination = targetPoint;
            } else
            {
                agent.destination = transform.position;
            }
          

            // da nas izgubi enemy kada se dovoljno udaljimo od njega
            if(Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                if (!wasShot)
                {
                    chasing = false;

                    chaseCounter = keepChasingTime;
                }
            } else
            {
                wasShot = false;
            }

            if (shotWaitCounter > 0)
            {
                shotWaitCounter -= Time.deltaTime;

                if(shotWaitCounter <= 0)
                {
                    shotTimeCounter = timeToShot;
                }

                anim.SetBool("isMoving", true);
            } else
            {
                if (PlayerController.instance.gameObject.activeInHierarchy)
                {
                    shotTimeCounter -= Time.deltaTime;

                    if (shotTimeCounter > 0)
                    {
                        // da nas neprijatelj puca 
                        fireCount -= Time.deltaTime;

                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;

                            firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f, 1.2f, 0f));

                            // proveri ugao Playera(igraca)
                            Vector3 targetDIr = PlayerController.instance.transform.position - transform.position;
                            float angle = Vector3.SignedAngle(targetDIr, transform.forward, Vector3.up);

                            if (Mathf.Abs(angle) < 30f)
                            {
                                Instantiate(bullet, firePoint.position, firePoint.rotation);

                                anim.SetTrigger("fireShot");
                            }
                            else
                            {
                                shotWaitCounter = waitBetweenShots;
                            }


                        }

                        agent.destination = transform.position;
                    }
                    else
                    {
                        shotWaitCounter = waitBetweenShots;
                    }
                }

                anim.SetBool("isMoving", false);
            }
        }
    }

    // enemy nas juri kada ga pogodimo sa vece daljine
    public void GetShot()
    {
        wasShot = true;

        chasing = true;

    }
}
