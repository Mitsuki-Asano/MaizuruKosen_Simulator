using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCharactor : FieldObjectBase
{
    [SerializeField]
    private List<DialogueNode> dialogueNodes;

    protected override IEnumerator OnAction()
{
    int currentNode = 0;
    int currentRoot = 0; // 現在の選択肢ルート

    while (currentNode >= 0 && currentNode < dialogueNodes.Count)
    {
        var node = dialogueNodes[currentNode];

        // 現在のルートに属さないノードはスキップ
        if (node.rootID != currentRoot)
        {
            currentNode++;
            continue;
        }

        if (node.nodeType == DialogueNode.NodeType.Message)
        {
            yield return StartCoroutine(ShowMessageOneByOne(node.message, 0.05f));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
            currentNode++;
        }
        else if (node.nodeType == DialogueNode.NodeType.Choice)
        {
            int chosenIndex = -1;
            yield return StartCoroutine(ShowChoiceNode(node, (index) => chosenIndex = index));

            // 選択肢に応じてルートを変更
            currentRoot = chosenIndex + 1; // 例えば1,2,3...

            // 選択肢の nextNodeIndex にジャンプ
            currentNode = node.nextNodeIndex[chosenIndex];
        }
    }
}


    private IEnumerator ShowMessageOneByOne(string message, float interval)
    {
        string displayed = "";

        for (int i = 0; i < message.Length; i++)
        {
            displayed += message[i];
            showMessage(displayed);
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator ShowChoiceNode(DialogueNode node, System.Action<int> onChoose)
{
    int selected = 0;

    // 1. ノードの説明文を文字送りで表示
    yield return StartCoroutine(ShowMessageOneByOne(node.message, 0.05f));

    // 2. 選択肢の文字送り（選択肢1つずつを少しずつ表示）
    int maxLength = 0;
    foreach (var choice in node.choices) maxLength = Mathf.Max(maxLength, choice.Length);
    
    string[] displayedChoices = new string[node.choices.Count];
    for (int i = 0; i < node.choices.Count; i++) displayedChoices[i] = "";

    for (int charIndex = 0; charIndex < maxLength; charIndex++)
    {
        for (int i = 0; i < node.choices.Count; i++)
        {
            if (charIndex < node.choices[i].Length)
                displayedChoices[i] += node.choices[i][charIndex];
        }

        // 表示更新
        string displayText = node.message + "\n";
        for (int i = 0; i < node.choices.Count; i++)
        {
            displayText += (i == selected ? "> " : "  ") + displayedChoices[i] + "\n";
        }
        showMessage(displayText);

        yield return new WaitForSeconds(0.03f); // 選択肢文字送りの速度
    }

    // 3. 選択可能にする
    while (true)
    {
        string displayText = node.message + "\n";
        for (int i = 0; i < node.choices.Count; i++)
        {
            displayText += (i == selected ? "> " : "  ") + node.choices[i] + "\n";
        }
        showMessage(displayText);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            selected = Mathf.Max(0, selected - 1);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            selected = Mathf.Min(node.choices.Count - 1, selected + 1);

            //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        if (Input.GetKeyDown(KeyCode.Z))
        {
            onChoose?.Invoke(selected);
            break;
        }

        yield return null;
    }
}


/*
    // 選択結果に応じたフラグ・タスク処理
    private void HandleChoiceResult(DialogueNode node, int chosenIndex)
    {
        // ここでフラグやタスクを管理
        // 例: 任務を受けるかどうか
        if (node.message.Contains("任務"))
        {
            if (chosenIndex == 0)
            {
                GameFlags.missionAccepted = true;
                TaskManager.StartMission("任務A");
            }
            else
            {
                GameFlags.missionAccepted = false;
            }
        }

        // 他の選択肢フラグもここで追加可能
    }
    */
}
