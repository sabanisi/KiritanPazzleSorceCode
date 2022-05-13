using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PentagonCreater : MonoBehaviour
{
    [SerializeField] private Pentagon PentagonPrefab;

    private List<Pentagon> Childrens = new List<Pentagon>();

    public void CreatePentagon(bool isSmall)
    {

        float randomX = Random.value*10-16;//-16から-6
        float randomY = Random.value * 6 + 5;//5から11
        if (isSmall)
        {
            randomX += 12;
            randomY += 4;
        }

        float speedX = Random.value * 5+1f;
        float speedY = Random.value * 5 + 1.5f;
        Vector3 speed = new Vector3(speedX, -speedY, 0);
        if (isSmall)
        {
            speed *= 0.7f;
        }

        float scale = Random.value * 4 + 3;
        if (isSmall)
        {
            scale -= 2f;
        }

        float rotateSpeed = Random.value * 270 - 135f;

        Pentagon child=null;
        bool isCreated = false;
        foreach (Pentagon obj in Childrens)
        {
            if (!obj.gameObject.activeSelf)
            {
                child = obj;
                isCreated = true;
                child.gameObject.SetActive(true);
                break;
            }
        }
        if (!isCreated)
        {
            child = Instantiate(PentagonPrefab);
            Childrens.Add(child);
        }
        child.gameObject.transform.position = new Vector3(randomX, randomY, 0);
        child.SetSpeed(speed, rotateSpeed);
        child.transform.localScale = new Vector3(scale, scale, 1);
        child.SetIsMove(true);
        if (isSmall)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }
        else
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }

    public  void Cash()
    {
        foreach(var obj in Childrens)
        {
            obj.gameObject.SetActive(false);
        }
        Childrens.Clear();
    }

    public void SetPentagonMove(bool isMove)
    {
        foreach(var obj in Childrens)
        {
            obj.SetIsMove(isMove);
        }
    }
}

