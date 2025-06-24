using UnityEngine;

public class VineAnim : MonoBehaviour
{
    private Animator animator;
    private Collider2D col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Dissapear()
    {
        animator.SetTrigger("Disappear");
        col.enabled = false;
    }

    private void Appear()
    {
        animator.SetTrigger("Appear");
        col.enabled = true;
    }

    public void Toggle()
    {
        if(col.enabled == true)
        {
            Dissapear();
        }
        else
        {
            Appear();
        }
    }
}
