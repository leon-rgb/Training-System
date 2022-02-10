using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textTest : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<TextMeshProUGUI>())
        {
            TextMeshProUGUI textField = GetComponent<TextMeshProUGUI>();
            textField.text = "Depth of the wound was over 9000!!!";
            
        }
        else
        {
            TextMeshPro textField = GetComponent<TextMeshPro>();
            textField.text = "Depth of the wound was over 9000!!!";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
