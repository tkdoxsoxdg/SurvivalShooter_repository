using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;  //プレイヤー動作のスピード

    Vector3 movement;  //3D動作の為の変数宣言（位置や方向）
    Animator anim; //Animatorコンポーネントの変数宣言
    Rigidbody playerRigitbody; //Rigitbodyコンポーネントの変数宣言
    int floormask; //floormaskの変数宣言（レイヤー設定した）
    float camRayLength = 100f; //レイのLength

    void Awake()
    {
        //フロアレイヤーにレイヤーマスクを作成
        floormask = LayerMask.GetMask("Floor");

        //レイヤーマスクの設定。AnimatorとRigidbodyのコンポーネントの呼び出し
        anim = GetComponent<Animator>();
        playerRigitbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        //input axesを入力。Horizontal→水平。Vertical→垂直
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //h,vを引数にMoveメソッドで動かす。
        Move(h, v);

        //マウスカーソルに向かってプレイヤーが振り向く動作をTurning()で動かす。
        Turning();

        //h,vを引数にAnimatingメソッドで動作させる
        Animating(h, v);
    }

   void Move(float h, float v) //プレイヤーの移動動作を設定
    {
        //axis inputの基本情報をセット
        movement.Set(h, 0f, v);
    
        // 時間から動くスピードとVectorを比例し、movementを正規化し代入。
        movement = movement.normalized * speed * Time.deltaTime;

        //playerRigitbody変数で呼び出したRigidbodyから、movement変数を代入したpositionを引数に、動作ポジションを設定。
        playerRigitbody.MovePosition(transform.position + movement);
    }
    
     void Turning() //プレイヤーがマウスポインタ―に対して振り向く動作の設定
    {
        //mousePositionを引数に、カメラのスクリーンポイントからプレイヤの向きを変える。（マウスが画面上にあたっている方向に、プレイヤーが振り向くようにする）
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //レイを飛ばした際の、衝突したオブジェクト情報を得るための変数を定義。
        RaycastHit floorHit;

        //raycastの挙動とフロアレイヤーにヒットした時の処理
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floormask))
        {
            // ヒットしたポイントからプレイヤー位置を減算した値を、playerToMouseに代入しインスタンスを作成。playerToMouse→プレイヤーの視点
            Vector3 playerToMouse = floorHit.point - transform.position;

            //y軸に対しては、無視する（設定しないように、0fを設定）
            playerToMouse.y = 0f;

            //クォータニオンにて回転
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // 回転する際の動作を定義。
            playerRigitbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v) // プレイヤーが移動するときのアニメーションの設定
    {
        // walkingの判定
        bool walking = h != 0f || v != 0f;

        // anim変数に対して判定を設定。その時の引数も同時に定義する。
        anim.SetBool("IsWalking", walking);
    }

}
