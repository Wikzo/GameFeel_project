using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    void OnGUI()
    {
        speed  = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), speed, 0.0F, 2F);
    }


    private float speed = 1;

    // Update is called once per frame
    void Update()
    {

        animator.speed = speed;

        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        if (vertical > 0)
        {
            animator.SetInteger("Direction", 0);
            
        }
        else if (vertical < 0)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (horizontal > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (horizontal < 0)
        {
            animator.SetInteger("Direction", 3);
        }
    }
}
