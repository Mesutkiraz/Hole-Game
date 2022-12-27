using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyCubes : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float value;
    void Start()
    {
        value = 100f/ GameObject.FindGameObjectsWithTag("Block").Length ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            other.gameObject.SetActive(false);
            slider.value += value;
        }
    }
}
