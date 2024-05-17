using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public void ButtonPressed()
    {
        if (GameManager.instance.GetFlippedCard() == null && !GameManager.instance.GetShowing())
        {
            GameManager.instance.SetShowing(true);
            GameManager.instance.ShowAgainAll();
        }
    }
}
