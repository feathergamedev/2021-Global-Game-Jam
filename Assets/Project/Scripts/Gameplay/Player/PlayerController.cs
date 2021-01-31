using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public EPlayerState PlayerState;

    [TitleGroup("Key Stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float attackCoolDown;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxHP;
    [SerializeField] private float gravityScale;
    private UIHpBar hpBar;
    [SerializeField] private float curHP;

    [TitleGroup("Basic")]
    [ShowInInspector] private Vector2 curVelocity;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private Transform hpBarTransform;
    [SerializeField] private GameObject dieParticle;
    [SerializeField] private bool isOnGround;
    [SerializeField] private GameObject effectParticle;

    [TitleGroup("Attack")]
    [SerializeField] private CircleCollider2D attackHitBox;

    [TitleGroup("Components")]
    [SerializeField] private Rigidbody2D m_rigid;
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private Animator m_animator;

    private bool isFreezed = true;

    private float baseMoveSpeed;
    private float baseJumpForce;
    private float baseDamage;
    private float baseAttackCooldown;
    private float baseMaxHP;

    private float attackTimer;

    private void Awake()
    {
        RecordBaseStats();

        void RecordBaseStats()
        {
            baseMoveSpeed = moveSpeed;
            baseJumpForce = jumpForce;
            baseDamage = damage;
            baseAttackCooldown = attackCoolDown;
            baseMaxHP = maxHP;
        }
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (PlayerState)
        {
            case EPlayerState.Walk:
                MoveRight();
                break;
            case EPlayerState.Attack:
                Attack();
                break;                
        }

        var anyFloor = (Physics2D.OverlapBox(groundCheckTransform.position, new Vector2(0.6f, 0.1f), 0, (int)EGameLayer.Platform));
        isOnGround = (anyFloor != null);
        /*
        HandleMoveInput();
        HandleJumpInput();
        HandleDashInput();
        HandleAttackInput();
        */


        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
        else
            attackTimer = 0f;
    }

    private void FixedUpdate()
    {
        GravityEffect();

        m_rigid.velocity = curVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var targetLayer = (EGameLayer)(1 << collision.gameObject.layer);

        switch (targetLayer)
        {
            case EGameLayer.Mechanic:
                if (collision.gameObject.tag == "Spike")
                    TakeDamage(1f);
                    hpBar.Set(curHP / maxHP);
                break;
            default:
                break;
        }

    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        var targetLayer = (EGameLayer)(1 << collision.gameObject.layer);

        switch (targetLayer)
        {
            case EGameLayer.Enemy:
                PlayerState = EPlayerState.Attack;
                break;
            default:
                PlayerState = EPlayerState.Walk;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var targetLayer = (EGameLayer)(1 << collision.gameObject.layer);

        switch (targetLayer)
        {
            case EGameLayer.Enemy:
                PlayerState = EPlayerState.Walk;
                break;
        }
    }

    public void Setup(Vector3 initPos, Potion potion = null)
    {      
        if (potion != null)
        {
            switch (potion.Type)
            {
                case EEffectType.MoveSpeed:
                    moveSpeed = baseMoveSpeed * (1 + potion.Rate.Value());
                    break;
                case EEffectType.Damage:
                    damage = baseDamage * (1 + potion.Rate.Value());
                    break;
                case EEffectType.AttackFrequency:
                    attackCoolDown = baseAttackCooldown * (1 - potion.Rate.Value());
                    break;
                case EEffectType.MaxHP:
                    maxHP = baseMaxHP * (1 + potion.Rate.Value());
                    break;
            }

            effectParticle.SetActive(true);
        }
        else
        {
            ResumeBaseStats();
            effectParticle.SetActive(false);
        }

        curHP = maxHP;

        hpBar = UIManager.instance.RequestHpBar();
        hpBar.Init(hpBarTransform);
        hpBar.Set(curHP / maxHP);

        m_rigid.velocity = Vector2.zero;
        transform.position = initPos;
        PlayerState = EPlayerState.Walk;
        SetPlayerDisplay(true);

        void ResumeBaseStats()
        {
            moveSpeed = baseMoveSpeed;
            damage = baseDamage;
            jumpForce = baseJumpForce;
            attackCoolDown = baseAttackCooldown;
            maxHP = baseMaxHP;
        }
    }

    public void SetPlayerDisplay(bool isEnable)
    {
        transform.localScale = isEnable ? Vector3.one : Vector3.zero;
    }

    public void TakeDamage(float damage)
    {
        CameraManager.instance.CameraShake();

        curHP -= damage;

        if (curHP <= 0)
            Die();
    }

    public void Die()
    {
        StartCoroutine(DieSequence());
        PlayerState = EPlayerState.None;
        SetPlayerDisplay(false);
    }

    private IEnumerator DieSequence()
    {
        var particle = Instantiate(dieParticle, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1.0f);

        Destroy(particle.gameObject);

        StageManager.instance.DemoRoundFinished();
    }

    private void GravityEffect()
    {
        if (!isOnGround)
            curVelocity.y -= 9.8f * gravityScale;
        else if (curVelocity.y <= 0f)
            curVelocity.y = 0f;
    }

    private void MoveRight()
    {
        if (isFreezed)
            return;

        transform.position += new Vector3(moveSpeed * Time.deltaTime, 0);

        curVelocity.x = moveSpeed;

        transform.rotation = Quaternion.Euler(Vector3.zero);
        m_animator.SetBool("isWalking", true);

    }

    private void MoveLeft()
    {
        transform.position += new Vector3(-1 * moveSpeed * Time.deltaTime, 0);

        curVelocity.x = moveSpeed;

        transform.rotation = Quaternion.Euler(0, 180f, 0);
        m_animator.SetBool("isWalking", true);
    }

    private void HandleJumpInput()
    {
        if (isOnGround)
        {
            isOnGround = false;
            curVelocity.y = jumpForce;
        }
    }

    private void HandleDashInput()
    {
        m_animator.SetTrigger("Dash");

        //Animation Event
        void DashDone()
        {
            if (transform.rotation.y == 0f)
                transform.position += new Vector3(1.915f, 0, 0);
            else
                transform.position += new Vector3(-1.915f, 0, 0);

            UnlockInput();
        }
    }

    private void Attack()
    {
        if (attackTimer > 0f)
            return;

        m_animator.SetTrigger("Attack");
        attackTimer = attackCoolDown;
        LockInput();
    }

    //Animation Event
    private void ActivateAttackHitBox()
    {
        var results = Physics2D.OverlapCircleAll(attackHitBox.transform.position, attackHitBox.radius, (int)EGameLayer.Enemy);
        foreach(var e in results)
        {
            e.GetComponent<EnemyBase>().TakeDamage(damage);
        }
    }

    //Animation Event
    private void LockInput()
    {
        curVelocity = Vector2.zero;
        isFreezed = true;
    }

    //Animation Event
    private void UnlockInput()
    {
        isFreezed = false;
    }
}