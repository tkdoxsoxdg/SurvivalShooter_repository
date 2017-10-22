using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;  //プレイヤー動作のスピード

    Vector3 movement;  //プレイヤー動作を管理するVector
    Animator anim; //animatorコンポーネント
    Rigidbody playerRigitbody; //player Rigitbodyコンポーネント
    int floormask; //レイヤー上のgameobjectの呼び出し
    float camRayLength = 100f; //シーン上のカメラからのLength

    void Awake()
    {
        //フロアレイヤーにレイヤーマスクを作成
        floormask = LayerMask.GetMask("Floor");

        //レイヤーマスクの設定
        anim = GetComponent<Animator>();
        playerRigitbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        //input axesを入力
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //シーン上でのプレイヤーの動き
        Move(h, v);

        //マウスカーソルに向かってプレイヤーが振り向く
        Turning();

        //プレイヤーのアニメーション
        Animating(h, v);
    }

   void Move(float h, float v)
    {
        //axis inputの基本情報をセット
        movement.Set(h, 0f, v);
    
        // 時間から動くスピードとVectorを比例し、movementを正規化
        movement = movement.normalized * speed * Time.deltaTime;

        //プレイヤーの位置から追加するプレイヤーの動き
        playerRigitbody.MovePosition(transform.position + movement);
    }
    
     void Turning()
    {
        //スクリーン上のマウスカーソルから、カメラのrayを作成
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //floorHit変数を定義
        RaycastHit floorHit;

        //raycastの挙動とフロアレイヤーにヒットした時の処理
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floormask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            //Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation
            playerRigitbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
    }

}
