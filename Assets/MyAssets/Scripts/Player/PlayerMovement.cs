using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Movimiento
    [SerializeField]
    private Transform _currentTransform;
    [SerializeField]
    private float _speed = 5f;
    private float _horizontalInput, _forwardInput;
    #endregion

    #region Rotacion
    [SerializeField]
    private float _rotateSpeed = 5f;
    private float _targetAngle;
    private float _CurrentAngle;
    #endregion

    private PlayerManager _playerManager;
    private void Start()
    {
        _playerManager = GetComponent<PlayerManager>();

        if(_playerManager == null){
            Debug.LogWarning("No se encuentra el componente Player Manager");
        }

        _currentTransform = _playerManager.getPlayer().transform;

        if(_currentTransform == null){
            Debug.LogWarning("Transform del jugador no especificado");
        }

        _targetAngle = _currentTransform.rotation.y;
        _CurrentAngle = _targetAngle;
    }
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _forwardInput = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(_horizontalInput, 0,_forwardInput);
        movimiento.Normalize();

        float speedAnim = Mathf.Max(Mathf.Abs(_horizontalInput),Mathf.Abs(_forwardInput));
        _playerManager.setAnimationFloat(speedAnim);

        #region Actualizar Rotacion
        _CurrentAngle = Mathf.LerpAngle(_CurrentAngle,_targetAngle,_rotateSpeed * Time.deltaTime);
        _currentTransform.rotation = Quaternion.Euler(0,_CurrentAngle,0);
        #endregion

        #region Detectar Teclas

        if(Input.GetKeyDown(KeyCode.E)){
            _playerManager.switchPlayer(true);
            _currentTransform = _playerManager.getPlayer().transform;
            return;
        }else if(Input.GetKeyDown(KeyCode.Q)){
            _playerManager.switchPlayer(false);
            _currentTransform = _playerManager.getPlayer().transform;
            return;
        }
        #endregion

        if(movimiento == Vector3.zero) return;

        #region Actualizar posicion
        switch(_playerManager.getState()){
            case PlayerState.State.GRABBING:
            case PlayerState.State.NORMAL:{
                _currentTransform.Translate(movimiento * _speed * Time.deltaTime,Space.World);
                _targetAngle = Mathf.Atan2(_horizontalInput,_forwardInput) * Mathf.Rad2Deg;
            }
            break;
        }
        #endregion
       
    }

}