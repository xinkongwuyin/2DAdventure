using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PlayStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;
    public float healthDelySpeed;

    private void Update()
    {
        if(healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime * healthDelySpeed;
        }
    }

        ///<summary>
        ///接受Health的变更百分比
        ///</summary>
        ///<param name="percentage">百分比:Current/Max</param>
        public void OnHealthChange(float percentage)
        {
            healthImage.fillAmount = percentage;
        }



}
