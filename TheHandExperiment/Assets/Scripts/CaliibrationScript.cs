using UnityEngine;

public class CaliibrationScript : MonoBehaviour
{

    [SerializeField] private string LayerMaskStringName;

    [SerializeField] private GameObject LeftRaycastOrigin;
    [SerializeField] private GameObject LeftRaycastDirection;
    [SerializeField] private GameObject LeftRaycastDirectionUp;

    [SerializeField] private GameObject Character1;
    [SerializeField] private GameObject Character2;
    private bool performRaycast = false;

    private float InitalCharacterHeight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitalCharacterHeight = Character1.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (performRaycast)
        {
            float dist = Raycast();
            if (dist != 0f)
			{
                //performRaycast = false;
                Character1.transform.position += new Vector3(0, -dist, 0);
                Character2.transform.position += new Vector3(0, -dist, 0);
                CalibrationSingleton.Instance.HeightAjustment = Character1.transform.position.y - InitalCharacterHeight;
			}
        }
    }

    public float Raycast()
	{
        Vector3 leftO = LeftRaycastOrigin.transform.position;
        Vector3 leftD = LeftRaycastDirection.transform.position;
        Vector3 leftDup = LeftRaycastDirectionUp.transform.position;
        RaycastHit LeftRC;

        int mask = 1 << 6;

        if ((leftD - leftO).y >= 0) // if hand is facing upward
            return 0f;
		// int mask = LayerMask.GetMask(LayerMaskStringName);
        bool rc1 = Physics.Raycast(leftO, leftD - leftO, out LeftRC, 1000, mask);
        
        //if (LeftRC.collider)
        if (rc1)
        {
            if (LeftRC.collider.gameObject.tag == "BottomPlane")
            {
                rc1 = Physics.Raycast(leftO, leftDup - leftO, out LeftRC, 1000, mask);

                if (rc1)
                    return -LeftRC.distance;
                else
                    Debug.Log("No hit ");
			}
        }
        else
        {
            //Debug.Log("No collider Found :(");
            return 0f;
        }




        return LeftRC.distance;
    }

    public void StartCallibration()
	{
        performRaycast = true;
	}

    public void EndCallibration()
    {
        performRaycast = false;
    }
}
