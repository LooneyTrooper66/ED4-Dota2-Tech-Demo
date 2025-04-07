using UnityEngine;

public class CreepProjectile : MonoBehaviour
{
    private GameObject target;
    private float damage;
    private float speed = 10;


    public void SetTarget(GameObject newTarget, float attackDamage)
    {
        target = newTarget;
        damage = attackDamage;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            float distThisFrame = speed * Time.deltaTime;
            transform.Translate(dir.normalized * distThisFrame, Space.World);

            if (dir.magnitude <= distThisFrame )
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
        if (target.gameObject.tag != "CornerMarker")
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(target, damage);
            }
        }
    }
}
