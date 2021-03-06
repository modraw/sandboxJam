﻿using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {

    bool isFertil;
    public bool IsFertil
    {
        get { return isFertil; }

        set
        {
            isFertil = value;
        }
    }
    public GameObject PlantedFlower;

    public SpriteRenderer BackSprite;
    public SpriteRenderer FrontSprite;

    public SpriteBackFront[] Sprites;

    public Transform Center;

    void Start ()
    {
        int index = Random.Range(0, Sprites.Length);

        BackSprite.sprite = Sprites[index].BackSprite;
        FrontSprite.sprite = Sprites[index].FrontSprite;

        ShowSprites(false);
    }

    public Voices[] possibleVoices; 

    void OnTriggerEnter2D(Collider2D other)
    {
        Seed seed = other.GetComponent<Seed>();
        Spore spore = other.GetComponent<Spore>();

        if (null != seed && seed.IsCaptured)
            return;

        if ((IsFertil || GameManager.Instance.IsNewFlowerReplaceOldOne) && null != seed)
        {
            seed.IsCaptured = true;

            int previousLevel = 0;

            if(GameManager.Instance.IsNewFlowerReplaceOldOne)
            {
                if(null != PlantedFlower)
                {
                    FlowerRoot flower = PlantedFlower.GetComponent<FlowerRoot>();
                    FlowerRoot seedFlower = seed.Flower.GetComponent<FlowerRoot>();

                    if (flower.Level ==seedFlower.Level)
                    {
                        previousLevel = flower.GrowthLevel;
                        PlantedFlower.GetComponent<FlowerRoot>().Kill();
                        CreateTheFlower(seed, previousLevel + 1);
                    }
                }
                else
                {
                    CreateTheFlower(seed, 1);
                }

                ScaleObject scale = seed.gameObject.AddComponent<ScaleObject>();
                scale.GetComponent<Rigidbody2D>().isKinematic = true;
                scale.Scale(0.2f, Vector3.zero, true, Center.position, AbsorbIsFinish); 
            }   
        }

        if(null != spore)
        {
            spore.Kill();
        }
    }

    void AbsorbIsFinish()
    {
        IsFertil = false;

        ShowSprites(false);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Seed seed = coll.gameObject.GetComponent<Seed>();
        if (null != seed)
        {
            coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up*2f);
            GameObject.Destroy(coll.gameObject, 0.4f);
        }
        Spore spore = coll.gameObject.GetComponent<Spore>();
        if(null != spore && spore.IsFlying)
        {
            spore.Kill();
        }
    }

    public void HitByLaser(Vector3 hitVector)
    {
        if(!IsFertil)
        {
            IsFertil = true;
            ShowSprites(true);
        }
        else if(IsFertil)
        {
            //
        }
    }

    void ShowSprites(bool show)
    {
        BackSprite.enabled = show;
        FrontSprite.enabled = show;
        GetComponent<BoxCollider2D>().isTrigger = show;
    }

    void CreateTheFlower (Seed seed , int growthlevel)
    {
        GameObject flower = GameObject.Instantiate(seed.Flower.gameObject);
        flower.transform.position = transform.position;
        flower.transform.rotation = transform.rotation;
        flower.transform.parent = Planet.Instance.transform;
        flower.GetComponent<FlowerRoot>().GrowthLevel = growthlevel;
        PlantedFlower = flower;

        var flowerObject = PlantedFlower.GetComponent<FlowerRoot>();

        flowerObject.Voice = Instantiate(possibleVoices[Random.Range(0, possibleVoices.Length)]);
        flowerObject.Voice.transform.SetParent(flowerObject.transform);
        if (flowerObject.Level > GameManager.Instance.currentMaxLevel)
        {
            GameManager.Instance.currentMaxLevel = flowerObject.Level;
            if (GameManager.Instance.currentMaxLevel == 1)
            {
                GameManager.Instance.SwitchSoundSource(2);
            }

            if (GameManager.Instance.currentMaxLevel == 2)
            {
                GameManager.Instance.SwitchSoundSource(3);
            }
        }

        IsFertil = false;
    }
}
