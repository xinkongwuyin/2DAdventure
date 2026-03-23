using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private PlayerInputController playerInput;
    private Animator animator;
    public Transform playerTrans;
    public GameObject signSprite;
    private IIteractable targert; // 当前可交互对象的引用
    public bool canPress;

    private void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
        animator = signSprite.GetComponent<Animator>();
        playerInput = new PlayerInputController();
        playerInput.Enable();

    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.GamePlay.CONFIRM.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
    }

    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress) 
        {
            //Debug.Log("Confirm Pressed");
            //在这里添加按下交互键时的逻辑，例如显示对话框等
            targert.triggerAction(); // 调用当前可交互对象的触发方法
            signSprite.GetComponent<AudioDefinition>()?.PlayAudioClip(); // Play the audio source attached to the sign when the confirm action is triggered
        }
    }

    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted) 
        {
            //Debug.Log(((InputAction)obj).activeControl.device);
            //这是固定的API表现当前激活的设备名字

            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    animator.Play("KeyBoard");
                    break;
            }

        }
    }

    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress; // Show the sign sprite when canPress is true+
        signSprite.transform.localScale = playerTrans.localScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = true;
            targert = collision.GetComponent<IIteractable>(); // 获取当前可交互对象的引用
        }
        else
        {
            canPress = false;
        }
    } 

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = false;
        }
    }
}
