using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private readonly float Speed = 10f;
    Vector2 Movement;
    private Rigidbody2D PlayerRB;
    private BoxCollider2D Collider;
    public Animator Animator;


    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement.x = (Input.GetAxisRaw("Horizontal"));
        Movement.y = (Input.GetAxisRaw("Vertical"));

        Animator.SetFloat("Horizontal", Movement.x);
        Animator.SetFloat("Vertical", Movement.y);
        Animator.SetFloat("Speed", Movement.magnitude);
    }

    private void FixedUpdate()
    {
        PlayerRB.velocity = new Vector2 (Movement.x, Movement.y).normalized*Speed;
        
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.gameObject.CompareTag("Radaman"))
        {
            SceneManager.LoadScene("BattleScreen");
        }
        
    }  
}
