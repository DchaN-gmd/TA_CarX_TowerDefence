using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class Tower : SerializedMonoBehaviour
{
    [Header("Parameters")]
    [SerializeField, Min(0)] private float _shootInterval = 0.5f;
    [SerializeField, Min(0)] private float _range = 4f;

    [Header("Pool")]
    [SerializeField] private ProjectilePool _pool;

    private SphereCollider _sphereCollider;
    private bool _canShoot = true;
    private Coroutine _coroutine;
    
    protected List<Enemy> Enemies = new();

    protected virtual void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = _range;
    }

    protected virtual void Update()
    {
        if (IsShoot())
            Shoot();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            Enemies.Add(enemy);

            enemy.Dieing += OnEnemyDieing;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            Enemies.Remove(enemy);

            enemy.Dieing -= OnEnemyDieing;
        }
    }

    public Enemy GetCurrentEnemy()
    {
        if (Enemies != null && Enemies.Count > 0)
            return Enemies[0];

        return null;
    }

    protected bool IsShoot()
    {
        if (!_canShoot) return false;
        if (Enemies == null || Enemies.Count <= 0) return false;
        if (Vector3.Distance(transform.position, Enemies[0].transform.position) > _range) return false;
        return true;
    }

    protected void Shoot()
    {
        var newProjectile = CreateProjectile();
        if (newProjectile == null) throw new NullReferenceException("Projectile is null");

        ProcessShoot(newProjectile);

        _canShoot = false;

        if(_coroutine != null) return;
        _coroutine = StartCoroutine(Reloading());
    }

    protected virtual void ProcessShoot(Projectile newProjectile)
    {
        SubscribeToProjectile(newProjectile);

        newProjectile.SetTarget(GetCurrentEnemy());
    }

    private void SubscribeToProjectile(Projectile newProjectile)
    {
        newProjectile.Attacked += ReturnProjectile;
        newProjectile.LostTarget += ReturnProjectile;
        newProjectile.Release += ReturnProjectile;
    }

    private void ReturnProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectile.transform.position = transform.position;
        _pool.ReturnObjectToPool(projectile);

        projectile.Attacked -= ReturnProjectile;
        projectile.LostTarget -= ReturnProjectile;
        projectile.Release -= ReturnProjectile;
    }

    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(_shootInterval);
        _canShoot = true;
        _coroutine = null;
    }

    private Projectile CreateProjectile() => _pool.GetObject();

    private void OnEnemyDieing(Enemy enemy)
    {
        enemy.Dieing -= OnEnemyDieing;
        Enemies.Remove(enemy);
    }
}