using UnityEngine;

public class move : MonoBehaviour
{
	Vector3 startPosition = Vector3.zero;
	public float range = 6;
	public float speed = 1;
	public bool vertical = false;
	bool back = false;

	void Start()
	{
		startPosition = transform.position;
	}

	void FixedUpdate()
	{
		if(vertical){
			if(transform.position.y < startPosition.y + range && !back)
			{
				transform.position = Vector3.MoveTowards(transform.position,
										new Vector3(transform.position.x,
													transform.position.y + 1,
													transform.position.z),
										1* speed * Time.deltaTime);
			}
			else if (transform.position.y > startPosition.y + range || back)
			{
				back = true;
				transform.position = Vector3.MoveTowards(transform.position,
										new Vector3(startPosition.x,
													transform.position.y - 1,
													transform.position.z),
										1 * speed * Time.deltaTime);
			}
			if(transform.position.y < startPosition.y)
			{
				back = false;
			}
		}
		else{
			if(transform.position.x < startPosition.x + range && !back)
			{
				transform.position = Vector3.MoveTowards(transform.position,
										new Vector3(transform.position.x + 1,
													transform.position.y,
													transform.position.z),
										1* speed * Time.deltaTime);
			}
			else if (transform.position.x > startPosition.x + range || back)
			{
				back = true;
				transform.position = Vector3.MoveTowards(transform.position,
										new Vector3(startPosition.x - 1,
													transform.position.y,
													transform.position.z),
										1 * speed * Time.deltaTime);
			}
			if(transform.position.x < startPosition.x)
			{
				back = false;
			}
		}
	}
}
