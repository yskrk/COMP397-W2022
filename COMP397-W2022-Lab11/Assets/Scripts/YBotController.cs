using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YBotController : MonoBehaviour
{
    [SerializeField] Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) 
        {
            animator.SetInteger("AnimState", 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            animator.SetInteger("AnimState", 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            animator.SetInteger("AnimState", 2);
        }
    }
}
