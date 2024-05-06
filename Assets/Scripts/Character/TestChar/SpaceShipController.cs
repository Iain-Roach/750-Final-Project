using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceShipController : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask PlayerLayer;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject buildShot;
    PlayerControls controls;
    Rigidbody _rb;

    [SerializeField]
    GameObject gun;
    Vector2 lastGunAngle;


    [SerializeField]
    GameObject debugMenu;

    public Vector3 MoveInput
    {
        get { return controls.SpaceShip.Move.ReadValue<Vector2>(); }
    }
    // public Vector3 moveInput => controls.SpaceShip.Move.ReadValue<Vector2>();

    public Vector2 RotateInput
    {
        get { return controls.SpaceShip.Rotate.ReadValue<Vector2>(); }
    }
    public bool GetShoot
    {
        get { return controls.SpaceShip.Shoot.triggered; }
    }

    public bool GetBuild
    {
        get { return controls.SpaceShip.Build.triggered; }
    }

    void Awake()
    {
        controls = new PlayerControls();

        //controls.SpaceShip.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //controls.SpaceShip.Move.canceled += ctx => move = Vector2.zero;
        controls.SpaceShip.Shoot.performed += ctx => Shoot();
        controls.SpaceShip.Build.performed += ctx => Build();

        gun = gameObject.transform.GetChild(0).gameObject;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = controls.SpaceShip.Move.ReadValue<Vector2>();
        if(moveInput != Vector2.zero)
        {
            Vector2 m = (new Vector2(moveInput.x, moveInput.y) * Time.deltaTime);
            //transform.Translate(m, Space.World);
            _rb.AddForce(m);
            
        }

        Vector2 rotateInput = controls.SpaceShip.Rotate.ReadValue<Vector2>();
        if (lastGunAngle != Vector2.zero && rotateInput == Vector2.zero)
        {
            rotateInput = lastGunAngle;
        }
        lastGunAngle = rotateInput;

        float orbitRadius = 1.0f;
        
            Vector2 direction = new Vector2(rotateInput.x, rotateInput.y);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float radians = angle * Mathf.Deg2Rad;

            Vector3 orbitPosition = new Vector3(Mathf.Cos(radians) * orbitRadius, Mathf.Sin(radians) * orbitRadius, 0);

            gun.transform.localPosition = orbitPosition;

            gun.transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
        //gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationSpeed * Time.deltaTime);

        //RaycastHit hit;
        //bool sphereHit = Physics.SphereCast(gun.transform.position, 0.1f, gun.transform.localPosition.normalized, out hit, 1f, ~PlayerLayer);

        //if(sphereHit)
        //{
        //    Debug.Log(hit.transform.gameObject);
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (debugMenu.activeSelf)
            {
                //debugMenu.GetComponent<UIManager>().UnPauseGame();
            }
            else
            {
                //debugMenu.GetComponent<UIManager>().PauseGame();
            }
            debugMenu.SetActive(!debugMenu.activeSelf);

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(gun.transform.position, gun.transform.localPosition.normalized * 1);
    }

    void Shoot()
    {
        Debug.Log("Destroy Block");
        GameObject go = Instantiate(bullet, gun.transform.position, gun.transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(gun.transform.localPosition.normalized * (75));

    }

    void Build()
    {
        Debug.Log("Build Block");
        GameObject go = Instantiate(buildShot, gun.transform.position, gun.transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(gun.transform.localPosition.normalized * (75));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "QuadTree")
        {
            Debug.Log("Ran into quadtree");
        }
    }

    private void OnEnable()
    {
        controls.SpaceShip.Enable();
    }

    private void OnDisable()
    {
        controls.SpaceShip.Disable();
    }
}
