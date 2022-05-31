using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;







public class CharacterMovement : MonoBehaviour
{
    private Inputs_handle _input;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private Camera Cam;
    public GameObject test;
    
    private int t = 100;
    public float gravity = -10f;
    Alive_entity Selected;
    bool follow;


    public Text Speciess;
    public Text Location;
    public Text Status;
    public Text Hunger;
    public Text Thirst;
    public Text Speed;
    public Text SleepNess;
    public Text LockedOn;
    bool checker;
  



    public static CharacterMovement Instance { get; private set; }

    public Coords lol2;
    public Coords lol3;
    private CharacterController Body;
    private void Awake()
    {
        _input = GetComponent<Inputs_handle>();
        Body = GetComponent<CharacterController>();
    }
    //Coords lol2;
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame  
    void FixedUpdate()
    {
        //CONVERT 2D VECTOR TO 3D ON TWO AXIS
        var targetVec = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        //MOVE DIRECTION AIMING


        var MovementVec = moveToTarget(targetVec);
        //rotate direction traveling


        if (Input.GetMouseButton(1))
            rotatetoMovementVector(MovementVec);



      
        Vector3 CamPos = Cam.transform.position;

        

        if (Input.GetKeyDown("p"))
        {
            follow = !follow;
            transform.position = Selected.transform.position;

        }
        if (follow == true && Selected != null)
        {
            CamPos.x = Selected.transform.position.x - t;
            CamPos.y = Selected.transform.position.y + 100;
            CamPos.z = Selected.transform.position.z - 80;


            Speciess.text = "Species: " + Selected.Specie;
            Location.text = "X.Y: " + Selected.x + "," + Selected.y;
            Status.text = "Status: " + Selected.CurrentAction;
            Hunger.text = "Hunger: " + Selected.GetComponent<Animal>().Hunger;
            Thirst.text = "Thirst: " + Selected.GetComponent<Animal>().Thirst;
            Speed.text = "Speed: " + Selected.GetComponent<Animal>().speed;
            SleepNess.text = "Tired: " + Selected.GetComponent<Animal>().tired;
            LockedOn.text = "True";
            /*
                        public Text Species;
                public Text Location;
                public Text Status;
                public Text Hunger;
                public Text Thirst;
                public Text Speed;
                public Text SleepNess;
                public Text LockedOn;
            */



        }
        else
        {
            follow = false;

            CamPos.x = transform.position.x - t;
            CamPos.y = transform.position.y + 100;
            CamPos.z = transform.position.z - 80;
        }
        LockedOn.text = "LockOn:" + follow;
        // lol2 = new Coords(1,2);
        // lol3 = new Coords(1, 3);
        if (Input.GetMouseButton(0) && follow == false)
        {

            Ray ray = Cam.ScreenPointToRay(_input.MousePos);
            if (Physics.Raycast(ray, out RaycastHit hitinfo, maxDistance: 750f))
            {
                var target = hitinfo.point;
                target.y = transform.position.y;
                GameObject Selector = Instantiate(test,target,Quaternion.identity);
                Selected = EntityTracker.Instance.SelectClosestEntity(Selector.transform.position);
                Destroy(Selector);
                if (Selected != null) 
                    follow = true;
            }       
        }











        Cam.transform.position = CamPos;
    }

    private void rotatetoMovementVector(Vector3 MovementVec)
    {
        //keep last rotation unless changed
        /*
        if(MovementVec.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(MovementVec);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
        */


        Ray ray = Cam.ScreenPointToRay(_input.MousePos);
        if (Physics.Raycast(ray, out RaycastHit hitinfo, maxDistance: 500f))
        {
            var target = hitinfo.point;
            target.y = transform.position.y;
            transform.position = target;
        }


    }

    private Vector3 moveToTarget(Vector3 targetVec)
    {
        var speed = moveSpeed * Time.deltaTime;

        //rotate based on camera
        targetVec = Quaternion.Euler(0, Cam.gameObject.transform.eulerAngles.y, 0) * targetVec;

        var targetPos = transform.position + targetVec * moveSpeed;
        // transform.position = targetPos;
        //Body.transform.position = targetPos;
        // Body.MovePosition(targetPos);
        //targetVec.y -= 5;
        Body.Move(targetVec);
        //Body.AddForce(targetPos);
        return targetVec;
    }
}
