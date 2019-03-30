using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _itemList = new List<Sprite>();

    public List<Sprite> Get()
    {
        return _itemList;
    }
}
