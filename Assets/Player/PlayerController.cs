using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    //移動スピード
    public float speed = 3.0f;
    //アニメーション名
    public string upAnime = "PlayerUp"; //上向き
    public string downAnime = "PlayerDown"; //下向き
    public string leftAnime = "PlayerLeft"; //左向き
    public string rightAnime = "PlayerRight"; //右向き
    string nowAnimation = "";                // 現在のアニメーション
    string oldAnimation = "";        //以前のアニメーション
    // 停止中に表示するスプライト
    public Sprite idleUp;
    public Sprite idleDown;
    public Sprite idleLeft;
    public Sprite idleRight;

    float axisH; //横軸値(-1.0~0.0~1.0)
    float axisV; //縦軸値
    public float angleZ = -90.0f; //回転角

    Rigidbody2D rbody; //Rigidbody 2D
    bool isMoving = false; //移動中フラグ

    Animator anim;
    SpriteRenderer sr;

    //Start is called before the first frame update
    void Start()
    {
        //Rigidbodyを得る
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        //アニメーション
        oldAnimation = downAnime;
    }
    //Update is called once per frame
    void Update()
    {
        // 入力取得
        if (!isMoving)
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");
        }

        // 移動角度計算
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);

        // 向き判定
        if (angleZ >= -45 && angleZ < 45)
            nowAnimation = rightAnime;
        else if (angleZ >= 45 && angleZ <= 135)
            nowAnimation = upAnime;
        else if (angleZ >= -135 && angleZ < -45)
            nowAnimation = downAnime;
        else
            nowAnimation = leftAnime;

        // アニメーション切替 or 停止中のスプライト表示
        Vector2 move = new Vector2(axisH, axisV);
        if (move != Vector2.zero)
        {
            // 移動中 → Animator 有効
            if (!anim.enabled) anim.enabled = true;
            if (nowAnimation != oldAnimation)
            {
                oldAnimation = nowAnimation;
                anim.Play(nowAnimation);
            }
        }
        else
        {
            // 停止中 → Animator 無効、立ち絵表示
            if (anim.enabled) anim.enabled = false;
            switch (nowAnimation)
            {
                case "PlayerUp": sr.sprite = idleUp; break;
                case "PlayerDown": sr.sprite = idleDown; break;
                case "PlayerLeft": sr.sprite = idleLeft; break;
                case "PlayerRight": sr.sprite = idleRight; break;
            }
        }
        
    }
    void FixedUpdate()
    {
        //移動速度を更新する
        Vector2 move = new Vector2(axisH, axisV);
        if (move != Vector2.zero)
        {
            move = move.normalized;//正規化
        }
        rbody.linearVelocity = move * speed;
    }
    public void SetAxis(float h, float v)
    {
        axisH = h;
        axisV = v;
        if (axisH == 0 && axisV == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }
    //p1からp2の角度を返す
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            //移動中であれば角度を更新する
            //p1からp2への差分　(原点を0にするため)
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            //アークタンジェント2関数で角度(ラジアンを求める)
            float rad = Mathf.Atan2(dy, dx);
            //ラジアンを度に変換して返す
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            //停止中であれば以前の角度を維持
            angle = angleZ;
        }
        return angle;
    }
}
