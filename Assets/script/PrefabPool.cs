using System.Collections.Generic;
using UnityEngine;

public class PrefabPool : MonoBehaviour
{
    public GameObject goPrefab;
    public int poolMaxSize;
    List<GameObject> elementPoolList;

    //Creamos un método estático para que se pueda llamar desde donde se desee
    void Start()
    {
        elementPoolList = new List<GameObject>();
        for (int i = 0; i < poolMaxSize; ++i)
        {
            GameObject obj = Instantiate(goPrefab);
            obj.SetActive(false);
            elementPoolList.Add(obj);
        }
    }

    public GameObject GetPoolObject()
    {
        for (int i = 0; i < elementPoolList.Count; ++i)
        {
            if (!elementPoolList[i].activeInHierarchy)
            {
                return elementPoolList[i];
            }
        }
        return null;
    }

    public void ReturnPoolObject(GameObject obj)
    {
        obj.SetActive(false);
        elementPoolList.Add(obj);
    }
}
