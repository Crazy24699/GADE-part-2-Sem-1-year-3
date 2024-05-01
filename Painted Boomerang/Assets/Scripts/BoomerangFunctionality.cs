using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class BoomerangFunctionality : MonoBehaviour
{

    private const int MaxBounces = 5;
    public int BouncesRemaining;
    public int Damage;

    public bool CanApplyDamage = true;
    public bool InstantBreak = false;   

    protected Vector2 LastVel;
    protected Vector2 Direction;

    protected float CurrentSpeed;
    protected Rigidbody2D RB2D;

    public WorldHandler.Teams ThisTeam;
    public PlayerFunctionality PlayerParent;

    public GameObject ParentEntity;
    public GameObject CollidedObject;

    [SerializeField]protected Collider2D BounceCollider;

    void Start()
    {
        BounceCollider = GetComponent<Collider2D>();

        if(Damage == 0)
        {
            Damage = 1;
        }
        BouncesRemaining = MaxBounces;
        RB2D = GetComponent<Rigidbody2D>();
        CanApplyDamage = true;
        if(InstantBreak && PlayerParent.InstantBreak)
        {
            Damage = 105;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BouncesRemaining <= 0)
        {
            BouncesRemaining = 0;
            BounceCollider.isTrigger = true;
            BounceCollider.excludeLayers = new LayerMask();
            Direction = (ParentEntity.transform.position- transform.position).normalized;
            ParentEntity.GetComponent<Collider2D>().excludeLayers = new LayerMask();
            RB2D.velocity = Direction * 35;
        }
        LastVel = RB2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.collider.tag.Contains("Wall"))
        {
            Debug.Log("Change");
            CurrentSpeed = LastVel.magnitude;
            Direction = Vector2.Reflect(LastVel.normalized, Collision.contacts[0].normal);
            RB2D.velocity = Direction * Mathf.Max(CurrentSpeed, 0);

            BouncesRemaining--;
            if ( Collision.collider.tag.Contains("Break") ) 
            {
                //StartCoroutine(DamageCooldown());
                CollidedObject = Collision.gameObject;
                if (PlayerParent.InstantBreak && InstantBreak)
                {
                    Damage = 105;
                    BouncesRemaining = 0;
                }
                if (!PlayerParent.InstantBreak && Damage > 3 && !InstantBreak) 
                {
                    PlayerParent.InstantBreak = false;
                    Damage = 3;
                }
            }

        }
        if (Collision.collider.CompareTag("Entity"))
        {
            if (Collision.collider.GetComponent<EntityBase>().AssignedTeam != ThisTeam)
            {
                Collision.collider.GetComponent<EntityBase>().HandleHealth(-1);
                BouncesRemaining = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Entity"))
        {
            EntityBase EntityRef = Collision.GetComponent<EntityBase>();
            if (EntityRef.AssignedTeam == ThisTeam)
            {
                ParentEntity.GetComponent<EntityBase>().PlayerScript.CanPerformAction = true;
                ParentEntity.GetComponent<EntityBase>().PlayerScript.MovesRemaining--;
                Destroy(this.gameObject);
            }
        }
    }

    public IEnumerator RemoveLayerExclusion()
    {
        yield return new WaitForSeconds(1);
        BounceCollider.excludeLayers = new LayerMask();
    }

    public IEnumerator DamageCooldown()
    {
        CanApplyDamage = false;
        yield return new WaitForSeconds(0.25f);
        CollidedObject = null;
        CanApplyDamage = true;
    }
}
