using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] public GameObject Player;

    [Header("������� ��������� - ������ ����� �����")]
    [SerializeField] public int Width;
    [SerializeField] public int Height;

    private bool flagX = false;
    private bool flagY = false;


    private void Start()
    {
        Width /= 2;
        Height /= 2;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 playerPosition = Player.transform.position;


        int signX = (int)Mathf.Sign(playerPosition.x);
        int ceilPositiveX = (int)playerPosition.x * signX;

        if (flagX) {
            if (ceilPositiveX != Width - 2)
                flagX = false;
        }
        else if (ceilPositiveX > Width + 1)
        {
            playerPosition.x = -1 * signX * (Width - ceilPositiveX % Width - playerPosition.x * signX + ceilPositiveX);
            flagX = true;
        }


        int signY = (int)Mathf.Sign(playerPosition.y);
        int ceilPositiveY = (int)playerPosition.y * signY;

        if (flagY)
        {
            if (ceilPositiveY != Height - 2)
                flagY = false;
        }
        else if (ceilPositiveY > Height + 1)
        {
            playerPosition.y = -1 * signY * (Height - ceilPositiveY % Height - playerPosition.y * signY + ceilPositiveY);
            flagY = true;
        }

        //if (flagY && randomResidueY != -1 &&  ceilPositiveY % Height > 6 || ceilPositiveY % Height < 4)
        //{
        //    flagY = false;
        //}
        //else if (ceilPositiveX > Height)
        //{
        //    int residueY = Random.Range(0, 3);
        //    if (ceilPositiveY != residueY && ceilPositiveY % Height == residueY)
        //    {
        //        flagY = true;
        //        playerPosition.y *= -1;
        //        randomResidueY = residueY;
        //    }
        //}


        Player.transform.position = playerPosition;
    }
}
