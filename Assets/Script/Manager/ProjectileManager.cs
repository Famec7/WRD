using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    protected override void Init()
    {
        _poolManager = GetComponent<PoolManager>();
    }

    private PoolManager _poolManager;

    public T CreateProjectile<T>(string projectileName = default, Vector2 position = default) where T : ProjectileBase
    {
        var projectile = projectileName == default ? _poolManager.GetFromPool<T>() : _poolManager.GetFromPool<T>(projectileName);

        projectile.transform.position = position;

        return projectile;
    }

    public void ReturnProjectileToPool<T>(T projectile) where T : ProjectileBase
    {
        _poolManager.ReturnToPool(projectile);
    }
}