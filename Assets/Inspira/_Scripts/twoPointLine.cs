using UnityEngine;
[ExecuteInEditMode]
public class twoPointLine : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private LineRenderer line;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        line.positionCount = 2;
        line.SetPosition(0,pointA.position);
        line.SetPosition(1,pointB.position);
        
    }
}
