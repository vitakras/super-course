using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class HeartRate : MonoBehaviour {

	SerialPort sp = new SerialPort("/dev/tty.usbmodem1411", 115200);
	public int threshold = 120;
	public int targetBeat = 168;

	private int heartRate = 0;
	private int[] heartRateRecord;
	private bool isFirst;
	private int count;
	private int temp;
	private int average;

	// Use this for initialization
	void Start () {
		sp.Open ();
		sp.ReadTimeout = 1;

		heartRateRecord = new int[10];
		isFirst = true;
		count = 0;
		temp = 0;
		average = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (sp.IsOpen) 
		{
			try
			{
				heartRate = sp.ReadByte();
				if(isFirst)
				{
					if(heartRate <= threshold)
					{
						isFirst = false;
						heartRateRecord[count] = heartRate;
						count++;
					}
				}else{
					//read the previous stored heart rate
					temp = heartRateRecord[count - 1] - heartRate;

					//if the data is not stable, it won't be stored into the array
					if(temp<20 && count<10)
					{
						//add new heart rate data to the array
						heartRateRecord[count] = heartRate;
						if(count==9)	//check if the array is full, reset to 0
							count=0;
						else            //otherwise add the number
							count++;
					}

					//calculate the average heart rate
					for(int i = 0; i < count-1; i++)
					{
						average = average + heartRateRecord[i];
					}
					average = average/(count-1);

				}
				//MoveObject(sp.ReadByte());

				//check if the average heart rate is too high
				if(average > targetBeat)
				{
					print("too high");
				}
				//print(sp.ReadByte());

			}catch (System.Exception)
			{
				
			}
		}
	
	}


}
