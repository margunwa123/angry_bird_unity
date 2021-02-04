using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    public GameObject Parent;
    public Rigidbody2D RigidBody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };
    public UnityAction<Collision2D> OnBirdCollision = delegate { };

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    public BirdState state { get { return _state;  } }

    // Start is called before the first frame update
    public virtual void Start()
    {
        // pas awal awal, ga terpengaruh gaya gravitasi biar diem di tempat
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        _state = BirdState.Idle;
        // ga collide selama posisi idle
        Collider.enabled = false;
    }

    // fixed update run once, zero, or several times per frame, \
    // depending on how many physics frames per second are set in 
    // the time sttings and how fast/slow the framerate is
    void FixedUpdate()
    {
        // kalo dia lagi diem aja dan kecepatannya melewati kecepatan minimum
        if (_state == BirdState.Idle &&
              RigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if ((_state == BirdState.Thrown || _state == BirdState.HitSomething) &&
            RigidBody.velocity.sqrMagnitude < _minVelocity &&
            !_flagDestroy)
        {
            //Hancurkan gameobject setelah 2 detik
            //jika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            Destroy(gameObject, 2);
        }
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        // saat ditembakkan, jadi terpengaruh sama gaya fisika
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = velocity * speed * distance;
        OnBirdShot(this);
    }

    private void OnDestroy()
    {
        if(_state == BirdState.Thrown || _state == BirdState.HitSomething)
        {
           OnBirdDestroyed();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _state = BirdState.HitSomething;
        OnBirdCollision(collision);
    }

    public virtual void OnTap()
    {
        //powerup method
    }
}
