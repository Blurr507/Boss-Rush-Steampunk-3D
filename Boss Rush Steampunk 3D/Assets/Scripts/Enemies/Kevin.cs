using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kevin : Enemy
{
    public GameObject fileAttack;
    public GameObject penAttack;
    public int fileAttackDamage = 80;
    public GameObject fileBlock;
    public int fileBlockFail = 0;
    public int fileBlockTarget = -50;
    public int fileBlockCrit = -70;
    public int penAttackDamage = 150;
    public int dashDamage = 50;
    public AnimationCurve posCurve;
    public GameObject fileFolder;
    public List<Transform> spawnPoints = new List<Transform>();
    public List<FileFolder> files = new List<FileFolder>();
    public int fileSpawnWait = 1;
    public int fileSpawnCountdown = 1;
    private Animator anim;

    public override void StartOverride()
    {
        base.StartOverride();
        Random.InitState(10);
        anim = GetComponent<Animator>();
    }

    public override void DoTurn()
    {
        Debug.Log("Do turn");
        if (turns > 0)
        {
            turns--;
            if (turns <= 0)
            {
                BattleStateManager.me.IncrementCurrentEnemy();
            }
            if(fileSpawnCountdown <= 0 && files.Count == 0)
            {
                Debug.Log("Start spawn file coroutine");
                StartCoroutine(SpawnFiles());
            }
            else
            {
                Debug.Log("Start throw file coroutine");
                StartCoroutine(ThrowFile());
            }
        }
        else
        {
            //  If we're already out of turns, then skip this turn
            BattleStateManager.me.IncrementCurrentEnemy();
            BattleStateManager.me.IncrementState();
            BattleStateManager.me.IncrementState();
            BattleStateManager.me.IncrementState();
        }
    }

    private IEnumerator ThrowFile()
    {
        Debug.Log("Throw file");
        yield return new WaitForSeconds(0.5f);
        GameObject attack = Instantiate(fileAttack);
        DamageBubble bubble = FindObjectOfType<DamageBubble>();
        Debug.Log("spawned attack and found bubble");
        bubble.AddDamage(fileAttackDamage);
        Debug.Log($"Added {fileAttackDamage} damage");
        BattleStateManager.me.IncrementState();
        GameObject block = Instantiate(fileBlock);
        SteamGauge gauge = block.GetComponentInChildren<SteamGauge>();
        Debug.Log("Spawned block and found gauge");
        yield return new WaitForSeconds(0.5f);
        gauge.Spin();
        Debug.Log("Spun the gauge");
        while (true)
        {
            if (gauge.result != -1)
            {
                if (fileBlockFail != 0 || gauge.result > 0)
                {
                    SmallDamage smallDamage = gauge.GetComponent<CreateObjectInBounds>().CreateObject().GetComponent<SmallDamage>();
                    switch (gauge.result)
                    {
                        case 0:
                            smallDamage.damage = fileBlockFail;
                            break;
                        case 1:
                            smallDamage.damage = fileBlockTarget;
                            break;
                        case 2:
                            smallDamage.damage = fileBlockCrit;
                            break;
                    }
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(1f);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        bubble.MoveToPos(target.transform.position, 1, posCurve);
        yield return new WaitForSeconds(0.99f);
        HurtTarget(bubble.damage);
        yield return new WaitForSeconds(0.5f);
        Destroy(attack);
        Destroy(block);
        BattleStateManager.me.IncrementState();
    }

    private IEnumerator SpawnFiles()
    {
        Debug.Log("Spawn files");
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(1f);
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(fileFolder, spawnPoint);
        }
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
        fileSpawnCountdown = fileSpawnWait;
    }

    public override void ResetTurns()
    {
        base.ResetTurns();
        if(files.Count == 0 && fileSpawnCountdown > 0)
        {
            fileSpawnCountdown--;
        }
    }
}
