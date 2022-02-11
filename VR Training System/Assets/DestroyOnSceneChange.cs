using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnSceneChange : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
