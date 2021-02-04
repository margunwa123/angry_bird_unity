using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBird : Bird
{
    public SpriteRenderer spriteRenderer;
    public Sprite explosionSprite;

    [SerializeField]
    private int explosionMultiplier = 1;
    
    private bool hasExploded = false;

    private const float explosionForce = 100f;

    public void Explode(Collision2D other)
    {
        // can only explode once
        if (hasExploded) return;

        var tag = other.gameObject.tag;
        if(tag == "Enemy" || tag == "Obstacle")
        {
            float upwardsModifier = 0;

            var explosionDir = RigidBody.position - (Vector2)transform.position;
            var explosionDistance = explosionDir.magnitude;

            // Normalize without computing magnitude again
            if (upwardsModifier == 0)
                explosionDir /= explosionDistance;
            else
            {
                // From Rigidbody.AddExplosionForce doc:
                // If you pass a non-zero value for the upwardsModifier parameter, the direction
                // will be modified by subtracting that value from the Y component of the centre point.
                explosionDir.y += upwardsModifier;
                explosionDir.Normalize();
            }
            hasExploded = true;
            StartCoroutine(ExplosionEffect());
            other.rigidbody.AddForce(Mathf.Lerp(0, explosionForce * explosionMultiplier, (1 - explosionDistance)) * explosionDir, ForceMode2D.Force);
        }
    }

    public IEnumerator ExplosionEffect()
    {

        Sprite normalSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = explosionSprite;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = normalSprite;
    }

    public override void Start()
    {
        base.Start();
        // explode on collision
        this.OnBirdCollision += Explode;
    }
}
