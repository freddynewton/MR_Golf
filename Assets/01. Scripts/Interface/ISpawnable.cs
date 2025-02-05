using UnityEngine;

public interface ISpawnable
{
    Renderer Renderer { get; }
    Material OriginalMaterial { get; }

    void PrepareForSpawn(Material spawnMaterial);
    void Spawn();
    void Despawn();
}
