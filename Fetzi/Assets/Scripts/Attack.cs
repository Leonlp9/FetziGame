using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform attackPos;
    public LayerMask enemies;
    public float attackRange;
    public int dammage;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !Application.isMobilePlatform)
        {
            AttackEnemy();
        }
    }

    public void AttackEnemy() {

        if (gameObject == null)
        {
            return;
        }

        anim.SetBool("isAttacking", true);
        FindObjectOfType<AudioManager>().Play("attack");
        if (attackPos == null)
        {
            Debug.LogWarning("Attack position is null!");
            return;
        }
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemies);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().health -= dammage;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
