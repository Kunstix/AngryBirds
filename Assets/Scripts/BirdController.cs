using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Rigidbody2D hook;

    public GameObject nextBirdPrefab;
    public float releaseTime = 0.2F;

    private Rigidbody2D birdBody;
    private float maxDragDistance = 2F;
    private bool birdPressed;

    public AnimationClip angryBird;
    public AnimationClip dizzyBird;
    public AnimationClip explodeBird;

    private bool lastPlayer;
    private bool hasEntered;


    void Start()
    {
       birdBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (birdPressed)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(Vector3.Distance(mousePosition, hook.position) > maxDragDistance) {
                birdBody.position = hook.position + (mousePosition - hook.position).normalized * maxDragDistance;
            } else
            {
            birdBody.position = mousePosition;
            }
        }
    }

    private void OnMouseDown()
    {
        birdPressed = true;
        birdBody.isKinematic = true;
    }

    private void OnMouseUp()
    {
        birdPressed = false;
        birdBody.isKinematic = false;

        StartCoroutine(ReleaseBird());

   
        Debug.Log("Angry! " + angryBird.name);
        GetComponent<Animator>().Play(angryBird.name);

    }

    private IEnumerator ReleaseBird()
    {
        yield return new WaitForSeconds(releaseTime);

        GetComponent<SpringJoint2D>().enabled = false;
        enabled = false;

        yield return new WaitForSeconds(2.0F);

        if(nextBirdPrefab != null)
        {
            nextBirdPrefab.SetActive(true);
        } else
        {
            lastPlayer = true;
        }
        yield return new WaitForSeconds(2.0F);
        StartCoroutine(DestroyBird(2.0F));
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().Play(dizzyBird.name);       
        }
        if(gameObject.tag == "RedBird" && !hasEntered)
        {
           StartCoroutine(BirdExplode(collision));
           hasEntered = true;
        } else
        {
            StartCoroutine(DestroyBird(2.0F));
        }
    }

    private IEnumerator BirdExplode(Collision2D collision)
    {
        yield return new WaitForSeconds(0.5F);
        AddExplosionForce(collision.gameObject.GetComponent<Rigidbody2D>(), 4000.0F, gameObject.transform.position, 5.0F);
        Debug.Log("Explosion!");
        Debug.Log("Explosion Anim: " + explodeBird.name);
        GetComponent<Animator>().Play(explodeBird.name);
        DestroyBird(0.5F);
    }

    public void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Debug.Log("Force: " + dir.normalized * explosionForce * wearoff);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    IEnumerator DestroyBird(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        if (lastPlayer)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        GameMangagerController gm = GameMangagerController.GAME_MANAGER;
        if (gm != null)
        {
            if (gm.endScreen.activeSelf == true)
            {
                return;
            }
            else
            {
                gm.gameOver = true;
            }
        }
    }
}
