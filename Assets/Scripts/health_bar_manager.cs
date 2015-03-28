using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class health_bar_manager : MonoBehaviour {

	public RectTransform healthTransform;
	private float cachedY;
	private float minXValue;
	private float maxXValue;
	private int currentHealth;


	public int maxHealth;
	public Text healthText;
	public Image visualHR;

	public float target_heartR_min;
	public float target_heartR_max;
	public RectTransform targetZoneTransform;
	private float cachedYTZ;
	private float minXValueTZ;
	private float maxXValueTZ;
	private float currentTZMin;

	private float target_heartR_max_current = 100;
	private float target_heartR_min_current = 80;
	public float scaler;
	//private float temp_scaler;
	// Use this for initialization
	void Start () {
		cachedY = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		currentHealth = maxHealth;
		target_heartR_max_current = 100;
		target_heartR_min_current = 80;
		//temp_scaler = scaler;

		cachedYTZ = targetZoneTransform.position.y;
		maxXValueTZ = maxXValue + healthTransform.rect.width - targetZoneTransform.rect.width;
		minXValueTZ = maxXValue; 
		currentTZMin = target_heartR_min_current;
	}
	
	// Update is called once per frame
	void Update () {

		currentHealth = HRManager.AverageHeartRate;
		HandleHealth ();

		//change the size of target zone according to new min and max
		if (target_heartR_max!=0 && target_heartR_min!=0 &&(target_heartR_max - target_heartR_min) != (target_heartR_max_current - target_heartR_min_current)) 
		{
			scaler = (target_heartR_max - target_heartR_min)/(target_heartR_max_current - target_heartR_min_current);
			if(scaler>0)
			{
				HandleTargetZoneScale(scaler);
			}
//			target_heartR_max_current = target_heartR_max ;
//			target_heartR_min_current = target_heartR_min;
			maxXValueTZ = maxXValue + healthTransform.rect.width - targetZoneTransform.rect.width*2;
		}
		if (target_heartR_max != 0 && target_heartR_min != 0) 
		{
			float currentXValueTZ = MapValues(currentTZMin, 0, maxHealth, minXValueTZ, maxXValueTZ);
			targetZoneTransform.position = new Vector3(currentXValueTZ,cachedYTZ);
			print("tz position");
			print(targetZoneTransform.position.ToString());
		}
		target_heartR_max_current = target_heartR_max ;
		target_heartR_min_current = target_heartR_min;
	}

	private void HandleHealth()
	{
		healthText.text = currentHealth.ToString();

		float currentXValue = MapValues (currentHealth, 0, maxHealth, minXValue, maxXValue);

		healthTransform.position = new Vector3 (currentXValue, cachedY);

		if (currentHealth > maxHealth / 2) {
			//then I have more than 50%
			visualHR.color = new Color32 ((byte)MapValues (currentHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);

		} else //less than 50%
		{
			visualHR.color = new Color32(255, (byte)MapValues(currentHealth,0,maxHealth/2,0,255),0,255);
		}
	}

	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

	//change target zone size
	private void HandleTargetZoneScale(float scaler)
	{

		targetZoneTransform.sizeDelta = new Vector2 (targetZoneTransform.sizeDelta.x * scaler, targetZoneTransform.sizeDelta.y);
		//print ("scale function called");
	}
}
