using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * フィールドオブジェクトの基本処理
 */
public abstract class FieldObjectBase : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI target; // TMP対象
    public Canvas window;

    private bool isContacted = false;
    private IEnumerator coroutine;

    // プレイヤー接触判定
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            isContacted = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            isContacted = false;
    }

    private void Update()
    {
        // Zキーで会話開始
        if (isContacted && coroutine == null && Input.GetKeyDown(KeyCode.Z))
        {
            coroutine = CreateCoroutine();
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator CreateCoroutine()
    {
        // 会話ウィンドウを表示
        window.gameObject.SetActive(true);

        // 子クラスのアクションを実行
        yield return OnAction();

        // 会話終了
        if (target != null)
            target.text = "";

        window.gameObject.SetActive(false);
        coroutine = null;
    }

    protected abstract IEnumerator OnAction();

    // メッセージ表示
    protected void showMessage(string message)
    {
        if (target != null)
            target.text = message;
    }
}
