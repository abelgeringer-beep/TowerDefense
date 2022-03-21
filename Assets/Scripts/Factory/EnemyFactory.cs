using UnityEngine;

namespace Factory;

[CreateAssetMenu]
public class EnemyFactory: GameObjectFactory
{
    [SerializeField] private Enemy.Enemy prefab;

    public Enemy.Enemy get()
    {
        Enemy.Enemy instance = CreateGameObjectInstance(prefab);
        instance.
    }
}