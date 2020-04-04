using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField]
    private Transform playerTransform;
	[SerializeField]
    private float smoothing = 5f;	
	[SerializeField]
    private Vector3 offset = new Vector3 (0f, 15f, -22f);
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (this.playerTransform == null)
        {
            this.playerTransform = GameObject.Find("Player").transform;
        }
   }

	void Update ()
	{
        Vector3 targetCamPosition = this.playerTransform.position + this.offset;
        this.transform.position = Vector3.Lerp(this.transform.position, targetCamPosition, this.smoothing * Time.deltaTime);
    }
    #endregion
}

