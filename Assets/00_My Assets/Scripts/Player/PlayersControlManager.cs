using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayersControlManager : MonoBehaviour
{
    public static PlayersControlManager instance;

    public Vector2 virtualAxis;
    public Vector3 dirMovement, dirRotation;

    public PlayerControllerData[] playerControllers;
    public PlayerControllerData currentPlayer;

    [Range (0f, 5f)]
    public float movSpeed;

    public float rotSpeed;

    private void Awake()
    {
        instance = this;
        currentPlayer = playerControllers[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        movSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        virtualAxis.x = Input.GetAxisRaw("Horizontal");
        virtualAxis.y = Input.GetAxisRaw("Vertical");

        dirMovement = new Vector3(virtualAxis.x, 0f, virtualAxis.y);

        if (virtualAxis.magnitude != 0f)
        {
            currentPlayer.rb.transform.rotation = GetSmoothRot();
            currentPlayer.anim.SetInteger("movement", 1);
        }
        else currentPlayer.anim.SetInteger("movement", 0);

        if (Input.GetKeyDown(KeyCode.Q)) ChangePlayer();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        FrontalRaycast();
    }

    void MovePlayer()
    {
        currentPlayer.rb.velocity = dirMovement.normalized * movSpeed + Vector3.up * currentPlayer.rb.velocity.y;
    }

    Quaternion GetSmoothRot()
    {
        Quaternion currentRot = currentPlayer.rb.transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(dirMovement);

        return Quaternion.RotateTowards(currentRot, targetRot, rotSpeed * Time.deltaTime);
    }

    void ChangePlayer()
    {
        currentPlayer.anim.SetInteger("movement", 0);
        currentPlayer.rb.velocity = Vector3.zero;
        currentPlayer.rb.isKinematic = true;

        if (currentPlayer.player == PlayerType.Player_1) currentPlayer = playerControllers[1];
        else if (currentPlayer.player == PlayerType.Player_2) currentPlayer = playerControllers[0];

        currentPlayer.rb.isKinematic = false;
    }

    public InteractiveObjectFx currentInteractiveObject;

    void FrontalRaycast()
    {
        Ray ray = new Ray(currentPlayer.tr.position + Vector3.up, currentPlayer.tr.forward * 2f);
        bool result = Physics.Raycast(ray, out RaycastHit hit, 1.5f);

        if (result)
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            Debug.Log("Player toca: " + hit.collider.name);

            if (hit.collider.gameObject.layer == 6)
            {
                if (currentInteractiveObject == null)
                {
                    InteractiveObjectFx _interactiveScript = hit.collider.GetComponent<InteractiveObjectFx>();
                    currentInteractiveObject = _interactiveScript;
                    currentInteractiveObject.VisibilityFx(true);

                }
                else
                {
                    InteractiveObjectFx _interactiveScript = hit.collider.GetComponent<InteractiveObjectFx>();

                    if (currentInteractiveObject != _interactiveScript)
                    {

                        currentInteractiveObject.VisibilityFx(false);
                        currentInteractiveObject = _interactiveScript;
                    }
                }
            }
        }
        else
        {
            if (currentInteractiveObject != null)
            {
                currentInteractiveObject.VisibilityFx(false);
                currentInteractiveObject = null;
            }

            Debug.DrawRay(ray.origin, ray.direction * 1.5f, Color.blue);
            Debug.Log("Player no toca nada");
        }
    }
}

[Serializable]
public class PlayerControllerData
{
    public PlayerType player;

    public  Transform tr;

    public  Rigidbody rb;
    public  Animator anim;

    public PlayerControllerData()
    {
        tr = null;
        rb = null;
        anim = null;
    }

    public PlayerControllerData (PlayerType _player, Transform _tr, Rigidbody _rb, Animator _anim){

        player = _player;
        tr = _tr;
        rb = _rb;
        anim = _anim;
    }
}

public enum PlayerType
{
    Player_1,
    Player_2
}
