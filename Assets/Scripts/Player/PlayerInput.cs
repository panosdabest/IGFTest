using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    //references
    CharacterController characterController;
    private Inventory myInventory;

    //Player Vars
    [SerializeField]
    float playerSpeed=5f;
    [SerializeField]
    float DistanceToClick=3;

    //Layer for ray to work on.
    [SerializeField]
    LayerMask hoverableLayer;

    //images
    public Image WoodPlank;

    //Cursors
    public Texture2D ChopCursor;
    public Texture2D DefaultCursor;
    public Texture2D StorageCursor;
    public Texture2D PickUpCursor;

    //Texts
    public GameObject WarningText;
    public GameObject WarningStorage;
    public TextMeshProUGUI CoinCount;
    
    //RaycastHit info
    private RaycastHit hitHover;

    // Start is called before the first frame update
    void Start()
    {
        myInventory =GetComponent<Inventory>();
        characterController =GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
        Move();
    }

    public void Move()
    {
        //creating a vector passing him with our horizontal/vertical input  on x,z and then feed him in characterController 
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move * Time.deltaTime * playerSpeed);
    }
    public void Ray()
    {
        Ray rayHover = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayHover, out hitHover, Mathf.Infinity, hoverableLayer))
        {
            if (Vector3.Distance(transform.position, hitHover.point) <= DistanceToClick)
            {
                if (LayerCheck(hitHover, "Destroyable"))
                {
                    Cursor.SetCursor(ChopCursor, Vector3.zero, CursorMode.Auto);
                    Tree tree = hitHover.transform.gameObject.GetComponent<Tree>();
                    if (tree != null)
                        if (Input.GetMouseButtonDown(0))
                        {
                            tree.TakeDamage(1);
                        }
                }
                else if (LayerCheck(hitHover, "Pickable"))
                {
                    Cursor.SetCursor(PickUpCursor, Vector3.zero, CursorMode.Auto);
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (myInventory.WoodPlanks < 3)
                        {
                            Destroy(hitHover.transform.gameObject);
                            myInventory.WoodPlanks = 3;
                            WoodPlank.enabled = true;
                        }
                        else
                        {
                            WarningText.SetActive(true);
                        }
                        Debug.Log(myInventory.WoodPlanks);
                    }
                }

                else if (LayerCheck(hitHover, "Storage"))
                {
                    Cursor.SetCursor(StorageCursor, Vector3.zero, CursorMode.Auto);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Storage storage = hitHover.transform.gameObject.GetComponent<Storage>();
                        if (storage != null)
                        {
                            if (myInventory.WoodPlanks > 0)
                            {
                                storage.WoodPlankQuantity += myInventory.WoodPlanks;
                                myInventory.WoodPlanks -= myInventory.WoodPlanks;
                                WoodPlank.enabled = false;
                                myInventory.Coins += 9;
                                CoinCount.text = "Coins: " + myInventory.Coins;
                            }
                            else
                            {
                                WarningStorage.SetActive(true);
                            }
                        }
                    }
                }
            }
            else
            {
                //If player is outside of maximum distance to interact disable cursor and possible texts 
                Cursor.SetCursor(DefaultCursor, Vector3.zero, CursorMode.Auto);
                WarningText.SetActive(false);
                WarningStorage.SetActive(false);
            }

        }
        else
        {
            //if Mouse is not over a hoverable object and If player is outside of maximum distance to interact
            //set default cursor and disable possible  texts
            Cursor.SetCursor(DefaultCursor, Vector3.zero, CursorMode.Auto);
            WarningText.SetActive(false);
            WarningStorage.SetActive(false);
        }
    }

    //Method that returns true if the given layername index is the layer that we hover
    public bool LayerCheck(RaycastHit hit, string layerName)
    {
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer(layerName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
