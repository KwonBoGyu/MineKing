using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItemDrop : cProjectile
{
    private float timer;
    private bool dropped;
    public GameObject obj_bag;
    private Vector3 dir;
    private float dist;
    private float speed;
    public byte itemId;

    public override void Init()
    {
        base.Init();

        defaultPower = 10.0f;
        speed = 1000;
        dropped = true;
        originObj = this.gameObject;
        this.transform.localScale = new Vector3(1, 1, 1);
        SetDir(Vector3.up);
    }

    protected override void FixedUpdate()
    {
        if(timer < 1.0f)
            base.FixedUpdate();

        if(dropped.Equals(true))
        {
               timer += Time.deltaTime;

            if(timer >= 1.0f)
            {
                dir = new Vector3(obj_bag.transform.position.x - this.transform.position.x, obj_bag.transform.position.y - this.transform.position.y,
                    this.transform.position.z).normalized;
                dist = Vector2.Distance(obj_bag.transform.position, this.transform.position);
                this.transform.Translate(dir * speed * Time.deltaTime);
                this.transform.localScale *= 0.99f; 

                if (dist < 10.0f)
                {
                    cUtil._user._playerInfo.inventory.AddItem(itemId, 1);
                    dropped = false;
                    timer = 0;
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
