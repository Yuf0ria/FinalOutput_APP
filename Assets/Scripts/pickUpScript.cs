using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class pickUpScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform holdPos;

    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 50f; //how far the player can pickup the object from
    public float gravity = 9.81f;

    //private float rotationSensitivity = 1f; how fast/slow the object is rotated in relation to mouse movement
    private GameObject heldObjPrefab; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object

    private int LayerNumber; //layer index

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    MouseLook mouseLookScript;
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
        //mouseLookScript = controller.GetComponent<MouseLook>();
    }
    void Update()
    {
        if (Input.GetButton("Interact")) //change E to whichever key you want to press to pick up
        {
            if (heldObjPrefab == null) //if currently not holding anything
            {
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        //pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (canDrop == true)
                {
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
        if (heldObjPrefab != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            //RotateObject();
            if (Input.GetButton("Throw") && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
            {
                StopClipping();
                ThrowObject();
            }

        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObjPrefab = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObjPrefab.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObjPrefab.GetComponent<Collider>(), controller.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObjPrefab.GetComponent<Collider>(), controller.GetComponent<Collider>(), false);
        heldObjPrefab.layer = 0; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObjPrefab.transform.parent = null; //unparent object
        heldObjPrefab = null; //undefine game object
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObjPrefab.transform.position = holdPos.transform.position;
    }
    /*void RotateObject()
    {
        if (Input.GetButton("Mouse ScrollWheel"))//Use the ScrollWheel to Rotate
        {
            canDrop = false; //make sure throwing can't occur during rotating

            //disable player being able to look around
            mouseLookScript.mouseSensitivity = 0f;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            heldObjPrefab.transform.Rotate(Vector3.down, XaxisRotation);
            heldObjPrefab.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            //re-enable player being able to look around
            mouseLookScript.mouseSensitivity = 100f;
            canDrop = true;
        }
    }*/
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObjPrefab.GetComponent<Collider>(), controller.GetComponent<Collider>(), false);
        heldObjPrefab.layer = 0;
        heldObjRb.isKinematic = false;
        heldObjPrefab.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce * gravity);
        heldObjPrefab = null;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObjPrefab.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObjPrefab.transform.position = transform.position + new Vector3(0f, -0.1f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
