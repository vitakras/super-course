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

	public int target_heartR;

	// Use this for initialization
	void Start () {
		cachedY = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {

		currentHealth = HRManager.AverageHeartRate;
		HandleHealth ();
	
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
}
