using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

#if UNITY_EDITOR
    [Header("DEBUG")]
    public bool disableMouseMovement = false;
#endif

    #region Singleton
    private static CameraManager _instance = null;

    public static CameraManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<CameraManager>();
        }
        return _instance;
    }
    #endregion

    //Zoom Variables
    [SerializeField] private float _zoomSpeed = 4f;
    [SerializeField] private float _maxZoom = 15f;
    [SerializeField] private float _minZoom = 5f;
    private float pitch = 0f;
    private float _currentZoom;

    //Rotation Variables
    [SerializeField] private float _yawSpeed = 100f;
    [SerializeField] private Vector3 _cameraArm;
    private float _currentYaw;

    //Camera Control Variables
    private bool _isLocked = true;
    private Vector3 _cameraFreeMovement;
    [SerializeField] private float _cameraTranslationSpeed = 5f;

    //Camera Height Adjustments
    public LayerMask inferiorLayerMask = -1;
    public float lerpHeight;

    //Screen Limits
    private float _screenBorder = 20f;

    //Camera Target
    public Transform actorWithFocus;
    private Transform enemyWithFocus;

    private Transform _playerWithFocus;
    

    //Getters
    public Vector2 GetKeyboardInput
    {
        get { return new Vector2(
            Input.GetAxis("LateralCameraMovement") * Settings.GetInstance().SensitivityH,
            Input.GetAxis("ForwardBackCameraMovement") * Settings.GetInstance().SensitivityV
            ); }
    }

    public Vector2 GetMousePosition
    {
        get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
    }

    public Enemy EnemyWithFocus
    {
        get
        {
            if (enemyWithFocus == null)
            {
                return null;
            }
            return enemyWithFocus.GetComponent<Enemy>();
        }
        set
        {
            if (value != null)
            {
                enemyWithFocus = value.transform;
            }
            else
            {
                enemyWithFocus = null;
            }
        }
    }

    public bool IsLocked { get => _isLocked; set => _isLocked = value; }

    #region UNITY methods
    /// <summary>
    /// LateUpdate is called once per frame after every update function were called.
    /// </summary>
    private void LateUpdate()
    {
        if (Time.timeScale <= 0)
        {
            return;
        }
        CameraFollowPlayer();
        CameraZoomAndRotationFreeMode();
        CameraZoomAndRotationLockMode();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (Time.timeScale <= 0)
        {
            return;
        }
        YawCorrection();
        CameraZoomAndRotationWhenLocked();
        CameraMovementFreeModeWithKeyboard();
#if UNITY_EDITOR
        if (!disableMouseMovement)
        {
#endif
            CameraMovementFreeModeWithMouse();
#if UNITY_EDITOR
        }
#endif
                if (Input.GetButtonDown("CameraLockMode"))
        {
            CameraLockerButton();
        }
    }

    #endregion

    #region Class methods

    /// <summary>
    /// Lock the camera on the selected player object
    /// </summary>
    private void CameraFollowPlayer()
    {
        if (PlayerManager.GetInstance().PlayerWithFocus == null)
        {
            return;
        }
        _playerWithFocus = PlayerManager.GetInstance().PlayerWithFocus.transform;
        if (EnemyManager.GetInstance().EnemyWithFocus != null)
        {
            enemyWithFocus = EnemyManager.GetInstance().EnemyWithFocus.transform;
        }

        if (!_isLocked)
        {
            return;
        }

        if (enemyWithFocus != null)
        {            
            actorWithFocus = enemyWithFocus;
        }
        else
        {
            actorWithFocus = _playerWithFocus;
        }

        this.transform.position = actorWithFocus.position - (_cameraArm * _currentZoom);
        this.transform.LookAt(actorWithFocus.position + (Vector3.up * pitch));
        this.transform.RotateAround(actorWithFocus.position, Vector3.up, _currentYaw);
    }

    /// <summary>
    /// Rotate the camera around the focused player object
    /// </summary>
    private void CameraZoomAndRotationWhenLocked()
    {
        if (this._isLocked)
        {
            _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

            _currentYaw += Input.GetAxis("RotateCamera") * _yawSpeed * Time.fixedDeltaTime;           
        }
    }

    /// <summary>
    /// move the camera using the keyboard
    /// </summary>
    private void CameraMovementFreeModeWithKeyboard()
    {
        if (Input.GetButton("ForwardBackCameraMovement") || Input.GetButton("LateralCameraMovement"))
        {
            this._isLocked = false;
            _cameraFreeMovement = new Vector3(GetKeyboardInput.x, 0, GetKeyboardInput.y);
            _cameraFreeMovement = Quaternion.Euler(new Vector3(0, this.transform.eulerAngles.y, 0)) * _cameraFreeMovement * Time.fixedDeltaTime;
            _cameraFreeMovement = this.transform.InverseTransformDirection(_cameraFreeMovement);

            this.transform.Translate(_cameraFreeMovement);
        }
    }

    /// <summary>
    /// Move the camera using the mouse with moving it to the edge of the screen
    /// </summary>
    private void CameraMovementFreeModeWithMouse()
    {
        if (Input.mousePosition.y >= Screen.height - _screenBorder || Input.mousePosition.y <= _screenBorder || Input.mousePosition.x >= Screen.width - _screenBorder || Input.mousePosition.x <= _screenBorder)
        {
            this._isLocked = false;

            Rect leftRect = new Rect(0, 0, _screenBorder, Screen.height);
            Rect rightRect = new Rect(Screen.width - _screenBorder, 0, _screenBorder, Screen.height);
            Rect upperRect = new Rect(0, Screen.height - _screenBorder, Screen.width, _screenBorder);
            Rect lowerRect = new Rect(0, 0, Screen.width, _screenBorder);

            _cameraFreeMovement.x = leftRect.Contains(Input.mousePosition) ? -1 : rightRect.Contains(Input.mousePosition) ? 1 : 0;
            _cameraFreeMovement.z = upperRect.Contains(Input.mousePosition) ? 1 : lowerRect.Contains(Input.mousePosition) ? -1 : 0;
            _cameraFreeMovement.x *= Settings.GetInstance().SensitivityH;
            _cameraFreeMovement.z *= Settings.GetInstance().SensitivityV;

            _cameraFreeMovement = Quaternion.Euler(new Vector3(0f, this.transform.eulerAngles.y, 0f)) * _cameraFreeMovement * Time.fixedDeltaTime;
            _cameraFreeMovement = this.transform.InverseTransformDirection(_cameraFreeMovement);

            this.transform.Translate(_cameraFreeMovement);
        }
    }

    /// <summary>
    /// Deals with rotating and zoom when the camera is in lock mode
    /// </summary>
    private void CameraZoomAndRotationLockMode()
    {

        if (Input.GetButton("CameraMouseRot"))
        {
            Cursor.lockState = CursorLockMode.Locked;
            this._isLocked = false;

            this.transform.Rotate(Vector3.up, GetMousePosition.x * 2 * _yawSpeed * Time.fixedDeltaTime * .3f, Space.World);
            _currentYaw += GetMousePosition.x * _yawSpeed * Time.fixedDeltaTime;


            _currentZoom += GetMousePosition.y * _zoomSpeed / 10.0f;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

            lerpHeight = _currentZoom;
            float difference = 0f;

            if (DistanceToTheGround() != lerpHeight)
            {
                difference = lerpHeight - DistanceToTheGround();
            }

            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, lerpHeight + difference, this.transform.position.z), Time.deltaTime * 5f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }

    /// <summary>
    /// deals with zooming and rotating when the camera is in free mode
    /// </summary>
    private void CameraZoomAndRotationFreeMode()
    {
        if (!this._isLocked )
        {
            if (Input.GetButton("RotateCamera"))
            {
                this.transform.Rotate(Vector3.up, Input.GetAxis("RotateCamera") * 2 * _yawSpeed * Time.fixedDeltaTime * .3f, Space.World);
                _currentYaw += Input.GetAxis("RotateCamera") * _yawSpeed * Time.fixedDeltaTime;                
            }

            _currentZoom += Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

            lerpHeight = _currentZoom;
            float difference = 0f;

            if (DistanceToTheGround() != lerpHeight)
            {
                difference = lerpHeight - DistanceToTheGround();
            }

            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, lerpHeight + difference, this.transform.position.z), Time.deltaTime * 5f);
        }
    }    

    /// <summary>
    /// makes _currentYaw do full cycle from 0 to 360
    /// </summary>
    private void YawCorrection()
    {
        if(_currentYaw > 360)
        {
            _currentYaw = 0;
        }
        else if(_currentYaw <= 0)
        {
            _currentYaw = 360;
        }
    }

    /// <summary>
    /// Caculate the distance to the ground from the this object
    /// </summary>
    /// <returns> returns the distance</returns>
    public float DistanceToTheGround()
    {
        Ray ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, inferiorLayerMask.value))
        {
            return (hit.point - this.transform.position).magnitude;
        }

        return 0f;
    }

    /// <summary>
    /// Set the camera to focus or not
    /// </summary>
    /// <param name="toggle">use toggle mode or not</param>
    /// <param name="setOn">set on(default) or off </param>
    public void CameraLockerButton(bool toggle = true, bool setOn = true)
    {
        if (toggle)
        {
            this._isLocked = !this._isLocked; 
        }
        else
        {
            this._isLocked = setOn;
        }
    }
    #endregion
}
