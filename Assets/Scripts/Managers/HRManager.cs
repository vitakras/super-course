using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class HRManager : MonoBehaviour {
	//This script handles the communication between arduino board and unity
	//It reads HR from the board, calculate average HR from 10 reads and save it to a global variable called AverageHeartRate

	//global variables
	//public string port;
	public static int AverageHeartRate;
	public static int _heartRate;

	//initiate serial port communication
	SerialPort sp = new SerialPort("COM4", 9600);

	private int[] heartRateRecord;
	private bool isFirst;
	public int count;		//public to debug
	private int temp;
	private int sum;

	public int threshold = 105;

	//for debugging
	public int average;
	public int hr;
	// Use this for initialization
	void Start () {
		//open connection between unity and arduino
		sp.Open ();
		sp.ReadTimeout = 1;

		heartRateRecord = new int[10];
		isFirst = true;
		count = 0;
		temp = 0;
		AverageHeartRate = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (sp.IsOpen) 
		{
			try
			{
				//MoveObject(sp.ReadByte());
				_heartRate = sp.ReadByte();
				if(isFirst)
				{
					if(_heartRate <= threshold)
					{
						isFirst = false;
						heartRateRecord[count] = _heartRate;
						count++;
					}
				}else{
					temp = heartRateRecord[count - 1] - _heartRate;
					
					//if the data is not stable, it won't be stored into the array
					if(temp<20 && count <10)
					{
						//add new heart rate data to the array
						//heartRateRecord[count] = _heartRate;
						heartRateRecord.SetValue(_heartRate,count);
						print(heartRateRecord[count]);		//debug

						//calculate the average hr
						sum = 0;
						for(int i = 0; i < count; i++)
						{
							sum = sum + heartRateRecord[i];
						}
						AverageHeartRate = sum/count;

						if(count==9)	//check if the array is full, reset to 0
						{
							count=0;
							isFirst = true;

						}
						else            //otherwise add the number
						{
							count++;

						}
					}
					


				}
			}catch (System.Exception)
			{
				
			}
		}

		//debugging
		hr = _heartRate;
		average = AverageHeartRate;
	}
}
