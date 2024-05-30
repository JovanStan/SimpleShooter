using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    public float maxHealth = 500;
    public float currentHealth = 300;
    public Image healthUI;
   
   
 //   public EnemyController theEC;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();


   
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void DamageEnemy(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateUI();
      
     /*   if (theEC != null)
        {
            theEC.GetShot();
        } */

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }

   
    }


    private void UpdateUI()
    {
        healthUI.rectTransform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
    }
 
}
