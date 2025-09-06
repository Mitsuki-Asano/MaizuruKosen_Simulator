using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//出入り口の位置
public enum ExitDirection
{
    right, //右方向
    left, //左方向
    down, //下方向
    up, //上方向
}
public class Exit : MonoBehaviour
{
    public string sceneName = "";//移動先のシーン名
    public int doorNumber = 0; //ドア番号
    public ExitDirection direction = ExitDirection.down; //ドアの位置

    //Start is called before the first frame update
    void Start()
    {

    }
    //update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RoomManager.ChangeScene(sceneName, doorNumber);
        }
    }

}