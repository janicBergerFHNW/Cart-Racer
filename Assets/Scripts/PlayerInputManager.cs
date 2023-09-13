using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private Cart player1;
    [SerializeField] private Cart player2;

    public void OnMovePlayer1(InputValue inputValue)
    {
        player1.OnMove(inputValue);
    }

    public void OnMovePlayer2(InputValue inputValue)
    {
        player2.OnMove(inputValue);
    }

    public void OnResetPlayer1()
    {
        player1.OnReset();
    }
    
    public void OnResetPlayer2()
    {
        player2.OnReset();
    }
}
