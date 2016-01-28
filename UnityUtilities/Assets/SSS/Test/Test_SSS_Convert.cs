using UnityEngine;
using UnityEngine.Assertions;
using SSS.Convert;

public class Test_SSS_Convert : MonoBehaviour
{

    void Start()
    {
        Assert.raiseExceptions = true;
        // Deg ~ Rad
        Debug.Log("***** Deg ~ Rad *****");
        Assert.IsTrue(Convert.DegToRad(1f).ToString("0.000000") == ((1f * Mathf.PI) / 180).ToString("0.000000"));
        Assert.IsTrue(Convert.DegToRad(5f).ToString("0.000000") == ((5f * Mathf.PI) / 180).ToString("0.000000"));
        Assert.IsTrue(Convert.DegToRad(42.42f).ToString("0.000000") == ((42.42f * Mathf.PI) / 180).ToString("0.000000"));
        Assert.IsTrue(Convert.DegToRad(180f).ToString("0.000000") == ((180f * Mathf.PI) / 180).ToString("0.000000"));
        Assert.IsTrue(Convert.DegToRad(360f).ToString("0.000000") == ((360f * Mathf.PI) / 180).ToString("0.000000"));

        Assert.IsTrue(Convert.RadToDeg(1f).ToString("0.000000") == ((1f * 180f) / Mathf.PI).ToString("0.000000"));
        Assert.IsTrue(Convert.RadToDeg(5f).ToString("0.000000") == ((5f * 180f) / Mathf.PI).ToString("0.000000"));
        Assert.IsTrue(Convert.RadToDeg(42.42f).ToString("0.000000") == ((42.42f * 180f) / Mathf.PI).ToString("0.000000"));
        Assert.IsTrue(Convert.RadToDeg(180f).ToString("0.000000") == ((180f * 180f) / Mathf.PI).ToString("0.000000"));
        Assert.IsTrue(Convert.RadToDeg(360f).ToString("0.000000") == ((360f * 180f) / Mathf.PI).ToString("0.000000"));

        // KM ~ Miles
        Debug.Log("***** KM ~ Miles *****");


        // M ~ Feet
        Debug.Log("***** M ~ Feet *****");


        // C ~ F
        Debug.Log("***** C ~ F *****");


        // KG ~ Pound
        Debug.Log("***** KG ~ Pound *****");


        // CamelCase to full string
        Debug.Log("***** camelCase -> camel Case");       

    }
}