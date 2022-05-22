using UnityEngine;

public class circular : MonoBehaviour
{

	Vector3 startPosition = Vector3.zero;
	float progress = 0;
	public float speed;
	public float radius;

	void Start()
	{
		startPosition = transform.position;
	}

	void FixedUpdate()
	{
		progress+=speed/100;

		float new_x = startPosition.x+Mathf.Cos(progress)*radius;
		float new_y = startPosition.y+Mathf.Sin(progress)*radius;
						
		transform.position = Vector3.MoveTowards(transform.position,
									new Vector3(new_x,
												new_y,
												transform.position.z),
									100 * Time.deltaTime);
	}


}
