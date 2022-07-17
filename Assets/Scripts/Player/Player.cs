using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BasePlayer
{
    [SerializeField] public Gauge hp;
    [SerializeField] private InputAction mouseClick;
    [SerializeField] private InputAction spacePressed;
    [SerializeField] private float mouseDragSpeed;
    [SerializeField] private DeckSlot deckSlot;
    [SerializeField] private UnitManager unitManager;

    public DiceController diceController;

    private GameManager mngr;

    private Vector3 velocity = Vector3.zero;

    private Camera mainCamera;

    private void OnEnable()
    {
        spacePressed.Enable();
        spacePressed.performed += EndTurnPress;
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        spacePressed.performed -= EndTurnPress;
        spacePressed.Disable();
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }

    private void EndTurnPress(InputAction.CallbackContext ctx)
    {
        EndTurn();
    }

    public void Awake()
    {
        mainCamera = Camera.main;
    }

    public override bool IsDead() => hp.Current <= 0;

    public override IEnumerator StartTurn(GameManager mngr)
    {
        this.mngr = mngr;

        var task = diceController.Throw();
        while (!task.IsCompleted) {
            yield return null;
        }

        var unit = deckSlot.GetSlotContent();
        if (unit != null)
        {
            deckSlot.DropOwnership(this);
            Destroy(unit.gameObject);
        }

        var newUnit = unitManager.GenerateUnit(false, this.deckSlot);
        deckSlot.GetOwnership(newUnit, this);
        newUnit.transform.position = deckSlot.transform.position + new Vector3(0, 1, 0);

        isPlayerTurn = true;
    }

    private void EndTurn()
    {
        isPlayerTurn = false;
        mngr.SendMessage("EndTurn");
    }

    public override void TakeDamage(float damage)
    {
        hp.Current -= damage;
    }


    private void SwitchSlot(BaseUnit target, BaseUnit other, ISlot otherSlot)
    {
        var targetCurrentSlot = target.owner;
        if (otherSlot.DropOwnership(this) == ISlot.SlotReqResponse.KO)
        {
            return;
        }

        if (targetCurrentSlot.DropOwnership(this) == ISlot.SlotReqResponse.KO)
        {
            otherSlot.GetOwnership(other, this);
        }

        otherSlot.GetOwnership(target, this);
        targetCurrentSlot.GetOwnership(other, this);
    }

    // I'm very sorry for your eyes whoever you are :)  <- fuck u alexandre
    private IEnumerator DragUpdate(GameObject target)
    {
        var initialDistance = Vector3.Distance(target.transform.position, mainCamera.transform.position);
        var unitTarget = target.gameObject.GetComponent<BaseUnit>();
        while (mouseClick.ReadValue<float>() != 0)
        {
            var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            target.transform.position = Vector3.SmoothDamp(target.transform.position, ray.GetPoint(initialDistance),ref velocity , mouseDragSpeed);
     
            yield return null;
        }
        unitTarget.OnDrop();

        RaycastHit hit;
        ISlot slot;
        Physics.Raycast(target.transform.position, transform.TransformDirection((target.transform.up * -1)), out hit, 50);
        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<ISlot>(out slot))
        {
            var unit = slot.GetSlotContent();

            if (unit != null && unit.owner != null && unitTarget.owner != null)
            {
                SwitchSlot(unitTarget, unit, slot);
            }else if (unit == null)
            {
                if (unitTarget.owner.DropOwnership(this) == ISlot.SlotReqResponse.OK) {
                    slot.GetOwnership(unitTarget, this);
                }
            }
        }
    }

    private void MousePressed(InputAction.CallbackContext ctx)
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null) return;
            var grabbable = hit.collider.gameObject.GetComponent<BaseUnit>();
            if (grabbable == null) return;


            grabbable.OnGrab();
            StartCoroutine(DragUpdate(hit.collider.gameObject));


        }
    }
}
