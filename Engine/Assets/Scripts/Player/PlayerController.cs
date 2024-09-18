using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private KeyCode pauseKey;
    private float currentSpeed;
    Vector2 velocity;
    private Rigidbody rb;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
    }
    

    private void Update()
    {
        
        if (Input.GetKeyUp(pauseKey))
        {
            GameController.instance.IsPaused = !GameController.instance.IsPaused;
        }

        if (GameController.instance.IsPaused) { return; }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
        }
        if (rb!= null)
        {
            velocity.y = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
            velocity.x = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
            transform.Translate(velocity.x, 0, velocity.y);

            if (velocity.y > 0 || velocity.x > 0) AudioController.PlaySound(AudioKey.Walk);
        }
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            
            if (hit.collider.CompareTag("Door"))
            {
                GameController.instance.InfoText = true;
                if(GameController.instance.DoorOpen() && Input.GetKeyDown(KeyCode.E)) GameController.instance.EndGame(true);
            }
            else
            {
                GameController.instance.InfoText = false;


            }
        }
        else
        {
            GameController.instance.InfoText = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            AudioController.PlaySound(AudioKey.KeyUp);
            GameController.instance.CurrentKey = 1;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Spike")
        {
            GameController.instance.EndGame(false);
        }
    }

}
