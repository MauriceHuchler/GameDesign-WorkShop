using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public enum JointParameter : byte
{
    SPRING = 0,
    DAMPER,
    MINDISTANCE,
    MAXDISTANCE,
    TOLERANCE,
    BODY
}




public class SpringJointUI : MonoBehaviourPun
{
    private JointHandler jointHandler;
    public GameObject player;
    public Slider springSlider;
    public Slider damperSlider;
    public Slider minDisSlider;
    public Slider maxDisSlider;
    public Slider tolerance;

    void Start()
    {
        jointHandler = player.GetComponent<JointHandler>();
    }

    public void UpdateData(int _key)
    {
        switch (_key)
        {
            case (int)JointParameter.SPRING:
                jointHandler.setValues((byte)JointParameter.SPRING, springSlider.value, player.GetPhotonView().ViewID);
                break;

            case (int)JointParameter.DAMPER:
                jointHandler.setValues((byte)JointParameter.DAMPER, damperSlider.value, player.GetPhotonView().ViewID);
                break;

            case (int)JointParameter.MINDISTANCE:
                jointHandler.setValues((byte)JointParameter.MINDISTANCE, minDisSlider.value, player.GetPhotonView().ViewID);
                break;

            case (int)JointParameter.MAXDISTANCE:
                jointHandler.setValues((byte)JointParameter.MAXDISTANCE, maxDisSlider.value, player.GetPhotonView().ViewID);
                break;

            case (int)JointParameter.TOLERANCE:
                jointHandler.setValues((byte)JointParameter.TOLERANCE, tolerance.value, player.GetPhotonView().ViewID);
                break;

            default:
                break;
        }
    }
}
