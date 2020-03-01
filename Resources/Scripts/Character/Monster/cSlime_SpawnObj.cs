using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cSlime_SpawnObj : cProjectile
{
    private GameObject motherSlime;
    public GameObject slime;

    private bool isOnGround;

    public void Init(Vector3 pOriginPos, Vector3 pDir, GameObject pMother)
    {
        this.transform.position = pOriginPos;
        upBlockedContinue = false;
        flyingTime = 0;
        defaultPower = 8.0f;
        gravityAmount = 8f;
        changingPower = defaultPower;
        isReflectOn = true;
        isGravityOn = true;
        SetDir(pDir);
        motherSlime = pMother;
        isOnGround = false;
    }
    
    void Update()
    {
        if (dir.Equals(Vector3.zero))
            return;

        base.FixedUpdate();

        if (changingPower <= 0.5f && changingPower > 0)
        {
            isOnGround = true;
        }
        if(isOnGround)
        {
            changingPower = 0;
            slime.SetActive(true);
            slime.GetComponent<cMonster_stage1_slime>().Split();
            slime.GetComponent<cMonster_stage1_slime>().SetMotherObj(motherSlime);
            slime.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1000f);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1000f);
            this.GetComponent<BoxCollider2D>().enabled = false;
            isOnGround = false;
        }
    }
}
