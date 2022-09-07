using System.Collections.Generic;

using UnityEngine;

public class DynamicItemUIList : MonoBehaviour
{
    protected RectTransform itemsHolder;
    protected List<GameObject> items;
    protected int visibleItemCount;

    public void ConfigureAndHide()
    {
        this.itemsHolder = this.GetComponent<RectTransform>();
        this.items = new List<GameObject>();

        int childCount = this.itemsHolder.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform btn = this.itemsHolder.GetChild(i);
            this.items.Add(btn.gameObject);
        }

        this.HideAll();
    }

    public T GetNextItemAndActivate<T>()
    {
        this.visibleItemCount++;
        this.ResizeContainer();

        foreach (var item in this.items)
        {
            if (item.activeSelf == false)
            {
                item.SetActive(true);
                return item.GetComponent<T>();
            }
        }

        GameObject sample = this.items[0];
        GameObject clonedItem = Instantiate(sample);

        this.items.Add(clonedItem);
        clonedItem.transform.SetParent(this.itemsHolder);
        clonedItem.SetActive(true);

        clonedItem.transform.localScale = Vector3.one;

        return clonedItem.GetComponent<T>();
    }

    protected void ResizeContainer()
    {
        GameObject sample = this.items[0];
        RectTransform rect = sample.GetComponent<RectTransform>();

        this.itemsHolder.sizeDelta = new Vector2(
            this.itemsHolder.sizeDelta.x,
            this.visibleItemCount * rect.sizeDelta.y
        );
    }

    public void HideAll()
    {
        this.visibleItemCount = 0;

        foreach (var item in this.items)
        {
            item.SetActive(false);
        }
    }
}