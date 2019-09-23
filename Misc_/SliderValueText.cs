using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SliderValueText : MonoBehaviour
{
    private Slider betSlider;
    private Text t;
    // Start is called before the first frame update
    void Start()
    {
        
        t = this.GetComponent<Text>();
        betSlider = GameObject.FindGameObjectWithTag("BetSlider").GetComponent<Slider>();

    }

   
    // Update is called once per frame
    public void TextUpdate()
    {
        t.text = "$" + betSlider.value.ToString();
    }
}
