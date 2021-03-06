﻿using UnityEngine;
using System.Collections;

public class Spore : MonoBehaviour {

    public int Level;
    SpriteRenderer sprite;
    TrailRenderer trail;

	// Use this for initialization
	void Start ()
	{
	    HasCollided = false;
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        trail = GetComponent<TrailRenderer>();
        trail.sortingLayerName = "Flowers";
        trail.sortingOrder = 5;
        trail.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
       /* Destroy(gameObject);*/
    }

    private bool flying = false;

    public bool IsFlying
    {
        get { return flying; }
    }

    public FlowerRoot Flower { get; set; }
    public bool HasCollided { get; set; }

    void OnCollisionEnter2D(Collision2D col)
    {
       var spore = col.gameObject.GetComponent<Spore>();
       if (spore != null && spore.flying && this.flying)
        {
            
            GameManager.Instance.CreateSeed(this, spore);
        }

        var planet = col.gameObject.GetComponent<Planet>();
        if(null != planet && this.flying)
        {
            Kill();
        }
    }


    public void Explode()
    {
        flying = true;
        sprite.enabled = true;
        trail.enabled = true;
    }

    public void OnDestroy ()
    {
        if (null != Flower)
        {
            Flower.SporeDie();
        }
    }

    public void Kill ()
    {
        GameObject.Destroy(this.gameObject);
    }
}
