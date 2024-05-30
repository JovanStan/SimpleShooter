using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public static BulletController instance;

    public float moveSpeed, lifeTime;

    public Rigidbody theRB;

    public GameObject impactEffect;

    public int damage = 10;

    public bool damageEnemy, damagePlayer;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // kretanje metka sa promenom pravca 
        if(Input.GetMouseButton(1))
        {
            theRB.velocity = transform.forward * moveSpeed + Random.insideUnitSphere * 1;
        } else
        {
            theRB.velocity = transform.forward * moveSpeed + Random.insideUnitSphere * 4;
        }

        // da se metak unisti posle odredjenog vremena
        lifeTime -= Time.deltaTime;

        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    
  

    private void OnCollisionEnter(Collision collision)
    {
        // skidamo zivot neprijatelju
        if (collision.gameObject.tag == "Enemy" && damageEnemy)
        {
            collision.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }

        // skidamo mu duplo ako ga pogodimo u glavu
        if(collision.gameObject.tag == "Headshot" && damageEnemy)
        {
            collision.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage * 2);
        }

        // neprijatelj nam skida zivot
        if(collision.gameObject.tag == "Player" && damagePlayer)
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }

        Destroy(gameObject);
        // da se pojavi efekat unistavanja metka
        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
    }
}
