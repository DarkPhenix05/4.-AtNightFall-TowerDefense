using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float _panSpeed; 
    public float _borderThicknes;
    public float _scrollSpeed;
    public float _maxDistanceUP;
    public float _maxDistanceDOWN;
    public float _maxDistanceL;
    public float _maxDistanceR;
    public float _maxDistanceZ;
    public float _minDistanceZ;

    public bool _panCamera = false;


    private Vector3 _defPos;
    public Vector3 _curPos;
    private float _distancePosX;
    private float _distancePosY;

    private void Awake()
    {
        _defPos = transform.position;
    }
    void Update()
    {
        if(GameManager._gameOver)
        {
            this.enabled = false;
            return;
        }
        //Get Distance To Origin
        DistanceToOrigin();

        //Swuitch states
        SwitchInputs();

        //Check for movement
            //Zoom
            Zoom();

            //UP,DOWN,LEFT,RIGHT
            Move();
    }

    private void DistanceToOrigin()
    {
        _curPos = transform.position;
        _distancePosX = _curPos.x - _defPos.x;
        _distancePosY = _curPos.z - _defPos.z;
    }

    private void SwitchInputs()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchPan();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            transform.position = _defPos;
            _curPos = transform.position;
        }
    }

    private void Zoom()
    {
        float _scroll = Input.GetAxis("Mouse ScrollWheel");
        //Zoom out
        if (_curPos.y >= _minDistanceZ && _scroll > 0.0f)
        {
            _curPos.y -= _scroll * 1000 * _scrollSpeed * Time.deltaTime;
        }

        //Zoom in
        if (_curPos.y < _maxDistanceZ && _scroll < 0.0f)
        {
            _curPos.y -= _scroll * 1000 * _scrollSpeed * Time.deltaTime;
        }
        transform.position = _curPos;

        if (!_panCamera)
        {
            return;
        }
    }

    private void Move()
    {
        if(!_panCamera)
        {
            return;
        }
        //UP
        if (Input.GetKey("w") && _distancePosX >= -_maxDistanceUP || Input.mousePosition.y >= Screen.height - _borderThicknes && _distancePosX >= -_maxDistanceUP)
        {
            transform.Translate(Vector3.left * _panSpeed * Time.deltaTime, Space.World);
        }

        //DOWN
        if (Input.GetKey("s") && _distancePosX < _maxDistanceDOWN || Input.mousePosition.y <= _borderThicknes && _distancePosX < _maxDistanceDOWN)
        {
            transform.Translate(Vector3.right * _panSpeed * Time.deltaTime, Space.World);
        }

        //RIGHT
        if (Input.GetKey("d") && _distancePosY < _maxDistanceR || Input.mousePosition.x >= Screen.width - _borderThicknes && _distancePosX < _maxDistanceR)
        {
            transform.Translate(Vector3.forward * _panSpeed * Time.deltaTime, Space.World);
        }

        //LEFT
        if (Input.GetKey("a") && _distancePosY >= -_maxDistanceL || Input.mousePosition.x <= _borderThicknes && _distancePosX >= -_maxDistanceL)
        {
            transform.Translate(Vector3.back * _panSpeed * Time.deltaTime, Space.World);
        }
    }
    void SwitchPan()
    {
        _panCamera = !_panCamera;
    }
}
