using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopupButton : MonoBehaviour
{
    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
