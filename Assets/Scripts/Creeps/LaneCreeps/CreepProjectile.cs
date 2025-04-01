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
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void DamageTarget()
    {
        if (target.gameObject.tag != "CornerMarker")
        {
            Stats targetStats = target.GetComponent<Stats>();
            if (targetStats != null)
            {
                targetStats.TakeDamage(target, damage);
            }
        }
    }
}
