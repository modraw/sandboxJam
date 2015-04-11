﻿using UnityEngine;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
     public Sprite _wideLaser;
    public Sprite _aim;
    public Transform Bomb;
    public Transform Planet;
    public LayerMask Mask;
   

    public void Shoot()
    {
        Planet = GameObject.FindGameObjectWithTag("Planet").transform;
        var spriteRenderer =(SpriteRenderer) gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = _wideLaser;

        var bomb =(Transform) Instantiate(Bomb, new Vector3(gameObject.GetComponent<Transform>().localPosition.x,0.5f,0f), Quaternion.identity);
        bomb.GetComponent<Bomb>().Init();
        Invoke("Back",1f); 
        
    }

    public void ShootRound()
    {
        Planet = GameObject.FindGameObjectWithTag("Planet").transform;
        var spriteRenderer = (SpriteRenderer)gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = _wideLaser;


        var raycastHit2D = Physics2D.Raycast(transform.position, Planet.transform.position - transform.position, Mask);

        var bomb = (Transform)Instantiate(Bomb, new Vector3(raycastHit2D.point.x,raycastHit2D.point.y), Quaternion.identity);
        bomb.GetComponent<Bomb>().Init();
        Invoke("Back", 1f);

    }

    public void Update()
    {
       
    }

    public void Back()
    {
        var spriteRenderer = (SpriteRenderer)gameObject.GetComponent<SpriteRenderer>();
       spriteRenderer.sprite = _aim;

    }

    
}