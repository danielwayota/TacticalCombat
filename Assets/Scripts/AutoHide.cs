using UnityEngine;

public class AutoHide : MonoBehaviour
{
    void Awake()
    {
        this.gameObject.SetActive(false);
    }
}