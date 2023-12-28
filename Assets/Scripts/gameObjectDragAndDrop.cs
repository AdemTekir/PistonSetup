using UnityEngine;

public class gameObjectDragAndDrop : MonoBehaviour
{
    public Transform targetOfParts;
    public GameObject makeChildrenForStopMove;
    public GameObject originalObj;
    public GameObject mainPistonParentObject;
    public GameObject mainCamera;
    public float speed;
    public string objectTag;
    public bool checkForStopMove;

    private GameObject originalObjCopy;
    private Material clonObjMaterial;
    private Vector3 screenSpace;
    private Vector3 offset;
    private bool checkForTransformPoisiton;
    private bool checkForTrigger;
    private bool checkForAnimationMove = false;
    private float rotateAnimationTime = 4;

    void Start()
    {
        clonObjMaterial = (Material)Resources.Load("ObjClonMaterial");
    }

    private void Update()
    {
        if (checkForAnimationMove)
        {
            PistonPartsSetup();
        }
    }

    private void OnMouseDown()
    {
        if (checkForStopMove)
        {
            mainCamera.gameObject.GetComponent<cameraRotateAround>().checkForRotateAround = false;

            screenSpace = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        }
    }

    private void OnMouseDrag()
    {
        if (checkForStopMove)
        {
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
            transform.position = currentPosition;
        }
    }

    private void OnMouseUp()
    {
        if (checkForStopMove)
        {
            if (!checkForTrigger)
            {
                if (checkForTransformPoisiton)
                {
                    PistonPartsSetup();

                    checkForTrigger = true;

                    Destroy(originalObjCopy);
                }
            }
            else
            {
                transform.parent = null;
            }
        }

        mainCamera.gameObject.GetComponent<cameraRotateAround>().checkForRotateAround = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(objectTag))
        {
            CreatePreviewCloneObj(1);

            checkForTransformPoisiton = true;
            checkForTrigger = false;

            if (!checkForTrigger)
            {
                mainPistonParentObject.gameObject.GetComponent<completeSetupPiston>().checkForCompleteSetup += 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(objectTag))
        {
            Destroy(originalObjCopy);

            rotateAnimationTime = 4;
            checkForAnimationMove = false;

            checkForTrigger = true;
            makeChildrenForStopMove.gameObject.GetComponent<gameObjectDragAndDrop>().checkForStopMove = true;

            if (checkForTrigger)
            {
                mainPistonParentObject.gameObject.GetComponent<completeSetupPiston>().checkForCompleteSetup -= 1;
            }

            mainPistonParentObject.gameObject.GetComponent<completeSetupPiston>().checkForCloseUI = true;
        }

    }

    void PistonPartsSetup()
    {
        checkForAnimationMove = true;

        if (objectTag == "rodCap")
        {
            Vector3 mainObjPosition = transform.position;
            Vector3 targetPosition = targetOfParts.transform.position;
            
            if (rotateAnimationTime > 0)
            {
                rotateAnimationTime -= Time.deltaTime;
                transform.Rotate(0, 0, 360 * 2 * Time.deltaTime);
            }
            else if (rotateAnimationTime < 0)
            {
                rotateAnimationTime = 0;
                transform.Rotate(0, 0, 0 * Time.deltaTime);
            }

            transform.position = Vector3.Lerp(mainObjPosition, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            Vector3 mainObjPosition = transform.position;
            Vector3 targetPosition = targetOfParts.transform.position;
            transform.position = Vector3.MoveTowards(mainObjPosition, targetPosition, speed * Time.deltaTime);
        }
        transform.parent = makeChildrenForStopMove.transform;

        makeChildrenForStopMove.gameObject.GetComponent<gameObjectDragAndDrop>().checkForStopMove = false;
    }

    void CreatePreviewCloneObj(int cloneNum)
    {
        for (int i = 0; i < cloneNum; i++)
        {
            Vector3 mainObjTargetPoisiton = targetOfParts.position;

            originalObjCopy = Instantiate(originalObj, new Vector3(mainObjTargetPoisiton.x, mainObjTargetPoisiton.y, mainObjTargetPoisiton.z), originalObj.transform.rotation);
            originalObjCopy.GetComponent<MeshRenderer>().material = clonObjMaterial;
            Destroy(originalObjCopy.gameObject.GetComponent<gameObjectDragAndDrop>());
        }
    }
}
