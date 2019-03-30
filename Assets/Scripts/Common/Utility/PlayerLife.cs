using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLife : PanelRoot
{
    [SerializeField]
    private TextMeshProUGUI _lifeText;

    [SerializeField]
    private int _life = 5;

    public void AddLife()
    {
        _life++;
        _lifeText.text = _life.ToString();
    }
}
