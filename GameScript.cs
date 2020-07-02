using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
public enum Tetromino { LINE, LFORWARD, LBACKWARD, SQUIGGLEFORWARD, SQUIGGLEBACKWARD, TEE, BOX };
public class GameScript : MonoBehaviour
{
    public Sprite[] tetrominos, pieces;
    public GameObject piecePrefab;
    public ContactFilter2D rotationFilter;
    public float move, fastMove, shiftVal, xStart, yStart, softDropRatio, speedUp;
    public System.Random rnd;
    public int playerPieceLayer, gridPieceLayer, playerLayer, levelUp, maxLevel;
    public LayerMask gridPieceLayerMask, playerPieceLayerMask, playerLayerMask;
    public MainMenuScript ui;
    private void Awake()
    {
        speedUp = 0.05f;
        softDropRatio = 12.5f;
        shiftVal = 0.09f;
        move = 0.25f;
        fastMove = 0.02f;
        playerPieceLayer = LayerMask.NameToLayer("PlayerPiece");
        gridPieceLayer = LayerMask.NameToLayer("GridPiece");
        playerLayer = LayerMask.NameToLayer("Player");
        gridPieceLayerMask = 1 << gridPieceLayer;
        playerPieceLayerMask = 1 << playerPieceLayer;
        playerLayerMask = 1 << playerLayer;
        rnd = new System.Random();
        rotationFilter = new ContactFilter2D();
        rotationFilter.SetLayerMask(gridPieceLayerMask);
        levelUp = 10;
        maxLevel = 10;
    }
}