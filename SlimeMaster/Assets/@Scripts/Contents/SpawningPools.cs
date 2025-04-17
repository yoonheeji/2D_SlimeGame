using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class SpawningPool : MonoBehaviour
{

    public int _maxMonsterCount = 1000;
    Coroutine _coUpdateSpawningPool;
    GameManager _game;

    public void StartSpawn()
    {
        _game = Managers.Game;
        if (_coUpdateSpawningPool == null)
            _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

    IEnumerator CoUpdateSpawningPool()
    {
        while (true)
        {
            //for (int i = 0; i < 20; i++)
            //{
            //    Vector2 spawnPos = Util.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);
            //    Managers.Object.Spawn<MonsterController>(spawnPos, 202001);
            //}
            //yield return new WaitForSeconds(1243511);
            if (_game.CurrentWaveData.MonsterId.Count == 1)
            {
                for (int i = 0; i < _game.CurrentWaveData.OnceSpawnCount; i++)
                {
                    Vector2 spawnPos = Util.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);
                    Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.MonsterId[0]);
                }
                yield return new WaitForSeconds(_game.CurrentWaveData.SpawnInterval);
            }
            else
            {
                for (int i = 0; i < _game.CurrentWaveData.OnceSpawnCount; i++)
                {
                    Vector2 spawnPos = Util.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);

                    if (Random.value <= Managers.Game.CurrentWaveData.FirstMonsterSpawnRate) // 90%�� Ȯ���� ù��° MonsterId ���
                    {
                        Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.MonsterId[0]);
                    }
                    else // 10%�� Ȯ���� �ٸ� MonsterId ���
                    {
                        int randomIndex = Random.Range(1, _game.CurrentWaveData.MonsterId.Count);
                        Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.MonsterId[randomIndex]);
                    }
                }
                yield return new WaitForSeconds(_game.CurrentWaveData.SpawnInterval);
            }

        }
    }


}