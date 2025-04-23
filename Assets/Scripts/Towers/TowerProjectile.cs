using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    private GameObject target;
    private float damage;
    private float speed = 4;


    public void SetTarget(GameObject newTarget, float attackDamage)
    {
        target = newTarget;
        damage = attackDamage;
    }

    private void Update()
    {
        if (target != null && target.activeSelf == true)
        {
            Vector3 dir = target.transform.position - transform.position;
            float distThisFrame = speed * Time.deltaTime;
            transform.Translate(dir.normalized * distThisFrame, Space.World);

            if (dir.magnitude <= distThisFrame)
            {
                DamageTarget();
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void DamageTarget()
    {
        if (target.tag == "Player")
        {
            HeroStats stats = target.GetComponent<HeroStats>();
            stats.DamageHero(target.gameObject, damage);
            return;
        }

        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.DealDamage(target, damage);
        }
    }
}
