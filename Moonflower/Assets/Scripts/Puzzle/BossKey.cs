using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKey : MonoBehaviour
{
    public GameObject hitEffect;
    public int health = 3;

    private bool shattered;

    private readonly float hurtdelay = 0.2f;
    private float timeSinceLastHurt;
    BossDoor bossDoor;

    public delegate void AttackedUpdate(GameObject aggressor, bool forceAggression);
    public event AttackedUpdate OnHit;

    // Start is called before the first frame update
    void Start()
    {
        bossDoor = GameObject.Find("BossDoor").GetComponent<BossDoor>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastHurt += Time.deltaTime;

        if (!shattered && health <= 0)
        {
            shattered = true;
            Shatter();
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHurtbox")
        {
            if (timeSinceLastHurt >= hurtdelay)
            {


                timeSinceLastHurt = 0f;

                ObjectPoolController.current.CheckoutTemporary(hitEffect, transform, 1);

                IHurtboxController hurtboxController = other.gameObject.GetComponent<IHurtboxController>();
                GameObject source = hurtboxController.Source;
                health -= hurtboxController.Damage;

                Debug.Log(gameObject.name + " took <color=red>" + hurtboxController.Damage + "</color> damage");

                OnHit?.Invoke(source, false);
            }
        }
    }
    private void Shatter()
        {

            gameObject.SetActive(false);
            Destroy(gameObject, 1);
            bossDoor.KeyOpen();
        }
}
