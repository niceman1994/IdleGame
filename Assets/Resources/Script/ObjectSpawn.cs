using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawn : Singleton<ObjectSpawn>
{
    [Header("STAGE")]
    public uint stageNum;
    public Text stage;
    [Space(15.0f)]
    public int monsterCount;
    [Header("Monster's name")]
    public List<GameObject> monsterName;
    [Header("MonsterList")]
    public List<GameObject> makeMonsters;

    void Start()
    {
        MakeMonster(7.0f);
    }

    void Update()
    {
        getStage();
    }

    public void MakeMonster(float _x)
    {
        for (int i = 0; i < monsterCount; ++i)
        {
            GameObject obj = Instantiate(monsterName[Random.Range(0, 4)],
                new Vector3(_x + (4.0f * i), -1.15f, 0.0f),
                Quaternion.identity);
            makeMonsters.Add(obj);

            int index = makeMonsters[i].name.IndexOf("(Clone)");
            if (index > 0)
                makeMonsters[i].name = makeMonsters[i].name.Substring(0, index) + (i + 1).ToString();
        }
    }

    public void PullObject(GameObject obj)
    {
        obj.GetComponent<BoxCollider2D>().enabled = true;
        Destroy(obj);
        obj = null;
    }

    public void DestroyMonster()
    {
        makeMonsters.Clear();
        for (int i = 0; i < makeMonsters.Count; ++i)
        {
            List<GameObject> obj = makeMonsters;
            Destroy(obj[i]);
            obj = null;
        }
    }

    void getStage()
    {
        stage.text = "STAGE " + stageNum.ToString();
    }

    public void StageUp()
    {
        DestroyMonster();
        MakeMonster(7.0f);
        stageNum += 1;
    }

    public void StageDown()
    {
        DestroyMonster();
        MakeMonster(7.0f);

        if (stageNum != 1)
            stageNum -= 1;
        else
            stageNum = 1;
    }
}
