using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileSelecter : MonoBehaviour {

    public GameObject tileHighlightPrefab;
    public Text coordText;

    private GameObject tileHighlight;

	// Use this for initialization
	void Start () {
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
	}

    public void EnterState() {
        enabled = true;
    }

    private void ExitState(GameObject movingPiece)
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        MoveSelecter move = GetComponent<MoveSelecter>();
        move.EnterState(movingPiece);
    }

    // Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; 
        if (Physics.Raycast(ray, out hit)) {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);
            if (gridPoint.x >= 0 && gridPoint.x <= 4 && gridPoint.y >= 0 && gridPoint.y <= 4)
            {
                coordText.text = point.ToString();

                tileHighlight.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject selectedPiece = GameManager.instance.PieceAtGrid(gridPoint);
                    if (GameManager.instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
                    {
                        GameManager.instance.SelectPiece(selectedPiece);
                        ExitState(selectedPiece);
                    }
                }
                tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
            }
            else
            {
                tileHighlight.SetActive(false);
            }
        }
        else {
            tileHighlight.SetActive(false);
        }
	}
}
