using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField]
    private List<Material> _itemList = new List<Material>();

    public List<Material> Get()
    {
        return _itemList;
    }
}
