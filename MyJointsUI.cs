using UnityEngine;
using UnityEngine.UI;

public class MyJointsUI : MonoBehaviour
{

    public SpringJoint mySpringJoint;
    public Text spring;
    public Text damper;
    public Text minDistance;
    public Text maxDistance;


    public void updateUIElementsSpring()
    {
        if (GetComponent<SpringJoint>() != null)
        {
            spring.text = GetComponent<SpringJoint>().spring.ToString();
            damper.text = GetComponent<SpringJoint>().damper.ToString();
            minDistance.text = GetComponent<SpringJoint>().minDistance.ToString();
            maxDistance.text = GetComponent<SpringJoint>().maxDistance.ToString();
        }
    }

}