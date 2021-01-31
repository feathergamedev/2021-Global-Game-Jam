using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{

    [TitleGroup("Basic")]
    [SerializeField] private float curHP;
    [SerializeField] private float maxHP;

    [TitleGroup("HP Bar")]
    [SerializeField] private UIHpBar hpBar;
    [SerializeField] private Transform hpBarPos;

    [TitleGroup("Components")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private BoxCollider2D m_collider;

    private void Start()
    {
        hpBar = UIManager.instance.RequestHpBar();
        hpBar.Init(hpBarPos);
    }

    public void TakeDamage(float damage)
    {
        curHP -= damage;

        hpBar.Set((float)curHP / (float)maxHP);

        if (curHP <= 0)
            Die();
        else
            m_animator.SetTrigger("isHit");

        CameraManager.instance.CameraShake();
    }

    private void Die()
    {
        m_animator.SetBool("isDead", true);
        m_collider.enabled = false;
    }

    //Animation Event
    private void DestroySelf()
    {
        hpBar.Destroy();
        Destroy(gameObject);
    }
}
