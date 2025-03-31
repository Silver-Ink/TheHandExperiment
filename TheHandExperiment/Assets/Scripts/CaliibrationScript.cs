using UnityEngine;

public class CaliibrationScript : MonoBehaviour
{

    [SerializeField] private string LayerMaskStringName;

    [SerializeField] private GameObject LeftRaycastOrigin;
    [SerializeField] private GameObject LeftRaycastDirection;
    [SerializeField] private GameObject LeftRaycastDirectionUp;
    [SerializeField] private GameObject RightRaycastOrigin;
    [SerializeField] private GameObject RightRaycastDirection;
    [SerializeField] private GameObject RightRaycastDirectionUp;

    [SerializeField] private GameObject Character1;
    [SerializeField] private GameObject Character2;
    [SerializeField] private float CountDown = 5;
    private bool performRaycast = false;
    private bool timeElapsed = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CountDown -= Time.deltaTime;
        if (CountDown <= 0 && !timeElapsed)
		{
            performRaycast = true;
            timeElapsed = true;
		}

        if (performRaycast)
        {
            float dist = Raycast();
            if (dist != 0f)
			{
                //performRaycast = false;
                Debug.Log("Distance " + dist.ToString());
                Character1.transform.position += new Vector3(0, -dist, 0);
                Character2.transform.position += new Vector3(0, -dist, 0);
			}
        }
    }

    public float Raycast()
	{
        Vector3 leftO = LeftRaycastOrigin.transform.position;
        Vector3 leftD = LeftRaycastDirection.transform.position;
        Vector3 leftDup = LeftRaycastDirectionUp.transform.position;
        RaycastHit LeftRC;

        Vector3 rightO = LeftRaycastOrigin.transform.position;
        Vector3 rightD = LeftRaycastDirection.transform.position;
        Vector3 rightDup = LeftRaycastDirectionUp.transform.position;
        RaycastHit RightRC;

        int mask = 1 << 6;
		// int mask = LayerMask.GetMask(LayerMaskStringName);
        bool rc1 = Physics.Raycast(leftO, leftD - leftO, out LeftRC, 1000, mask);
        bool rc2;
        
        //if (LeftRC.collider)
        if (rc1)
        {
            if (LeftRC.collider.gameObject.tag == "BottomPlane")
            {
                rc1 = Physics.Raycast(leftO, leftDup - leftO, out LeftRC, 1000, mask);

                if (rc1)
                {

                    rc2 = Physics.Raycast(rightO, rightDup - rightO, out RightRC, 1000, mask);

					if (rc1 && rc2)
					{
						return -(RightRC.distance + LeftRC.distance) / 2f;
					}
				}
                else
                    Debug.Log("No hit ");
			}
        }
        else
        {
            //Debug.Log("No collider Found :(");
            return 0f;
        }
        



        rc2 = Physics.Raycast(rightO, rightD - rightO, out RightRC, 1000, mask);
        if (rc1 && rc2)
		{
            return (RightRC.distance + LeftRC.distance) / 2f; 
		}

        return 0f;
    }
}
