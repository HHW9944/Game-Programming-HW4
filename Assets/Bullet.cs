using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Àû ÆÄ±«
            Destroy(gameObject); // ÃÑ¾Ë ÆÄ±«
            PlayerController.enemyDestroyedCount++;
            if (PlayerController.enemyDestroyedCount % 5 == 0) PlayerController.bullet02Count++;
            Debug.Log("Enemy Hunting Points : " + PlayerController.enemyDestroyedCount);
            Debug.Log("Bullet02 counts : " + PlayerController.bullet02Count);
        }
    }
}
