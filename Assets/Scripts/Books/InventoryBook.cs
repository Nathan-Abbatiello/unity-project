using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.EndlessBook;

public class InventoryBook : MonoBehaviour
{
    public EndlessBook book;

    // Start is called before the first frame update
    void Start()
    {
        book.SetState(EndlessBook.StateEnum.OpenMiddle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
