using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnebyOne : MonoBehaviour
{
    public void ButtonPressed()
    {
        // if (!GameManager.instance.GetIsFlipping() && !GameManager.instance.GetIsMatching())
        if (GameManager.instance.GetFlippedCard() == null && !GameManager.instance.GetShowing())
        {
            GameManager.instance.SetShowing(true);
            GameManager.instance.ShowAgainOnebyOne();
        }
    }
}
