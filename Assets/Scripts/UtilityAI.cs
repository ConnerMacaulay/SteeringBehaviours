using UnityEngine;
using System.Collections;

public class UtilityAI : MonoBehaviour {

    public float floatMeasure;
    public float sumMeasure;
    public float utilitySig;
    public float utilityExp;
    public float pMod;
    public float steep;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       
    }

   public void SigmoidCalculation()
    {
        sumMeasure = (floatMeasure-pMod)*steep;
        utilitySig = 10 / (1 + (Mathf.Exp(sumMeasure)));
    }

    public void ExponentialIncCalculation()
    {
        sumMeasure = (floatMeasure / 3.17f);
        utilityExp = Mathf.Pow(sumMeasure, pMod);
    }

    public void ExponentialDecCalculation()
    {

    }
}
