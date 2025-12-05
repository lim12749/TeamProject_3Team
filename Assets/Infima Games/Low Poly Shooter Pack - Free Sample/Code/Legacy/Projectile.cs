using System;
using UnityEngine;
using System.Collections;
using InfimaGames.LowPolyShooterPack;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{

    [Range(5, 100)]
    [Tooltip("After how long time should the bullet prefab be destroyed?")]
    public float destroyAfter;

    [Tooltip("If enabled the bullet destroys on impact")]
    public bool destroyOnImpact = false;

    [Tooltip("Minimum time after impact that the bullet is destroyed")]
    public float minDestroyTime;

    [Tooltip("Maximum time after impact that the bullet is destroyed")]
    public float maxDestroyTime;

    [Header("Impact Effect Prefabs")]
    public Transform[] bloodImpactPrefabs;
    public Transform[] metalImpactPrefabs;
    public Transform[] dirtImpactPrefabs;
    public Transform[] concreteImpactPrefabs;

    [Header("Damage")]
    public int damageAmount = 10;        // <<=== 원하는 데미지 값

    private void Start()
    {
        var gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        Physics.IgnoreCollision(gameModeService.GetPlayerCharacter().GetComponent<Collider>(), GetComponent<Collider>());

        StartCoroutine(DestroyAfter());
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Ignore collisions with other projectiles.
        if (collision.gameObject.GetComponent<Projectile>() != null)
            return;

        // ===========================
        // 🔥 IDamageable 적용 부분
        // ===========================


        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);

            Destroy(gameObject);
            return;
        }
        // =============================
        // 🔥 IDamageable 적용 종료
        // =============================

        //If destroy on impact is false, start coroutine
        if (!destroyOnImpact)
            StartCoroutine(DestroyTimer());
        else
            Destroy(gameObject);

        //--- 기존 충돌 파티클 처리 ---
        if (collision.transform.tag == "Blood")
        {
            Instantiate(bloodImpactPrefabs[Random.Range(0, bloodImpactPrefabs.Length)],
                transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Metal")
        {
            Instantiate(metalImpactPrefabs[Random.Range(0, metalImpactPrefabs.Length)],
                transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Dirt")
        {
            Instantiate(dirtImpactPrefabs[Random.Range(0, dirtImpactPrefabs.Length)],
                transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Concrete")
        {
            Instantiate(concreteImpactPrefabs[Random.Range(0, concreteImpactPrefabs.Length)],
                transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Target")
        {
            collision.transform.gameObject.GetComponent<TargetScript>().isHit = true;
            Destroy(gameObject);
        }

        if (collision.transform.tag == "ExplosiveBarrel")
        {
            collision.transform.gameObject.GetComponent<ExplosiveBarrelScript>().explode = true;
            Destroy(gameObject);
        }

        if (collision.transform.tag == "GasTank")
        {
            collision.transform.gameObject.GetComponent<GasTankScript>().isHit = true;
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(Random.Range(minDestroyTime, maxDestroyTime));
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(destroyAfter);
        Destroy(gameObject);
    }
}
