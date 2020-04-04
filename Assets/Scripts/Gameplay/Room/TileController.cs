using System.Collections;
using System.Collections.Generic;
using Kameosa.Cartesian;
using UnityEngine;

public class TileController : MonoBehaviour 
{
    #region Private Variables
    private Coordinate coordinate;
    private bool isOccupied = false;
    #endregion

    #region Properties
    public Coordinate Coordinate
    {
        get
        {
            return this.coordinate;
        }

        set
        {
            this.coordinate = value;
        }
    }

    public bool IsOccupied
    {
        get
        {
            return this.isOccupied;
        }

        set
        {
            this.isOccupied = value;
        }
    }
    #endregion
}
