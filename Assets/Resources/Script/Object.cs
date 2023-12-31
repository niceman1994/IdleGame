using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour, IObject
{
    [SerializeField] protected float hp;
    [SerializeField] protected float atk;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected AudioSource deadSound;
    [SerializeField] protected Transform textPos;
    [SerializeField] protected ItemDrop itembox;
    [SerializeField] protected BoxCollider2D destroyOwnCollider;

    protected int atkLoop;
    protected int giveGold;
    protected float defaultHp;
    protected float defaultAtk;
    protected Animator objectAnimator;
    [SerializeField] protected DetectCollider targetCollider;

    void Start()
    {
        targetCollider = GetComponent<DetectCollider>();
        defaultHp = hp;
        defaultAtk = atk;
        atkLoop = 0;
    }

    void Update()
    {
        Death();
    }

    protected void ChangeDefault()
    {
        if (hp > defaultHp && hp < 100000)
            defaultHp = hp;

        if (atk > defaultAtk && atk < 10000)
            defaultAtk = atk;
    }

    public void Death()
    {
        hp = 0;
        atkLoop = 0;
        destroyOwnCollider.enabled = false;
        targetCollider = null;
        StartCoroutine(afterDeath());

        if (gameObject.CompareTag("Monster") || gameObject.CompareTag("Boss"))
        {
            objectAnimator.SetBool("attack", false);
            objectAnimator.SetBool("death", true);

            if (objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.0f)
            {
                GameManager.Instance.gameGold.curGold[0] += giveGold;
                deadSound.Play();

                if (gameObject.CompareTag("Monster"))
                    itembox.SpawnItem(transform.position);
            }
        }
    }

    public IEnumerator afterDeath()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);

        if (objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (objectAnimator.CompareTag("Monster") || objectAnimator.CompareTag("Boss"))
            {
                yield return waitForSeconds;
                hp = defaultHp;
                ObjectSpawn.Instance.PullObject(gameObject);
            }
            else if (objectAnimator.CompareTag("Player"))
            {
                yield return waitForSeconds;
                hp = defaultHp;
            }
        }
    }

    public abstract float CurrentHp();
    public abstract float CurrentHp(float _hp);
    public abstract float HpUp(float _hp);
    public abstract void Attack();
    public abstract float CurrentAtk();
    public abstract float CurrentAtk(float addAtk);
    public abstract void GetAttackDamage(float dmg);
}
